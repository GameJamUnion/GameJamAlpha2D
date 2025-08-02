using System.ComponentModel;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BuildProfilesのSceneListと一致させる (手作業)
/// </summary>
public enum SceneNames :int
{
    [Browsable(false)]
    Invalid = -1,

    Master = 0,
    GameMastering,
    Title,
    Loading,
    komugi_workshop,
    dev_ozaki,

    Tutorial,

    [Browsable(false)]
    Max_Num,
}

public class SceneManager : SingletonBase<SceneManager>
{
    /// <summary>
    /// 進行中のアンロードデータ管理
    /// </summary>
    public class UnLoadProgressParam
    {
        public SceneNames SceneName;
        public float Progress;
    }


    private SceneNames[] _CurrentScene = new SceneNames[1] { SceneNames.Invalid };
    private SceneStateBase _CurrentState = null;
    private bool _IsPending = false;
    #region Request
    private bool _ToTitleRequest = false;
    private bool _RestartInGameRequest = false;
    private bool _EndTutorialRequest = false;
    #endregion

    /// <summary>
    /// ゲーム再生時に呼ばれる
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void SceneInitialized()
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var sceneCount = (int)SceneNames.Max_Num;
        
        // 常駐シーンをロード
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(SceneNames.GameMastering.ToString(), LoadSceneMode.Additive);



        for (int i = 0; i < sceneCount; i++)
        {
            var sceneName = (SceneNames)i;
            if (scene.name == sceneName.ToString())
            {
                var loadScene = sceneName;
                SceneManager.Instance.setDefaultSceneState(loadScene);
                return;
            }
        }

        // 登録されているシーン以外で開始した場合、シーン制御を行わない待機状態にする
        SceneManager.Instance.setPendingState();
    }

    #region LateUpdate
    public override void LateUpdate()
    {
        updateSceneState();
    }

    /// <summary>
    /// Stateの更新
    /// </summary>
    private void updateSceneState()
    {
        if (_IsPending)
        {
            return;
        }
        // 現在のシーンステートを更新
        var nextState = _CurrentState?.checkNext();

        if (_ToTitleRequest == true)
        {
            // タイトルへ戻るリクエストがあればタイトルへ移行
            nextState = new TitleSceneState();
        }
        else if (_RestartInGameRequest == true)
        {
            nextState = new GameLoadSceneState();
        }

        _ToTitleRequest = false;
        _RestartInGameRequest = false;

        if (nextState != null)
        {
            // 次のステートへ移行
            _CurrentState.OnExit();
            _CurrentState = nextState;

            // スキップしてさらに次のシーンに移行する場合は
            // さらに次のシーン再生を試みる
            void onEnter()
            {
                var result = _CurrentState.OnEnter();
                if (result != null)
                {
                    _CurrentState = result;
                    _CurrentScene = _CurrentState.SceneName;
                    onEnter();
                }
            }

            onEnter();
        }
    }
    #endregion
    /// <summary>
    /// インゲームシーンか
    /// </summary>
    /// <returns></returns>
    public bool checkInGameScene()
    {
        switch (_CurrentState)
        {
            case InGameSceneState0 _:
                return true;
        }
        return false;
    }

    /// <summary>
    /// 開始シーンステートを設定
    /// </summary>
    /// <param name="sceneName"></param>
    private void setDefaultSceneState(SceneNames sceneName)
    {
        var states = System.Reflection.Assembly.GetAssembly(typeof(SceneStateBase))
            .GetTypes().Where(x => x.IsSubclassOf(typeof(SceneStateBase)) && !x.IsAbstract).ToArray();

        for (int i = 0; i < states.Length; i++)
        {
            var state = (SceneStateBase)System.Activator.CreateInstance(states[i]);

            // 指定シーンをロードする
            if (state.SceneName.Contains(sceneName))
            {
                _CurrentState = state;
                _CurrentScene = state.SceneName;

                _CurrentState.OnEnter();
                return;
            }
        }
        setPendingState();
    }

    /// <summary>
    /// 動作停止状態にする
    /// </summary>
    private void setPendingState()
    {
        // 不正状態
        // 開発シーンとかなので切り替え停止
        _IsPending = true;
    }

    /// <summary>
    /// チュートリアル終了していいか
    /// </summary>
    /// <returns></returns>
    public bool checkEndTutorialScene()
    {
        return _EndTutorialRequest == true;
    }

    #region Load

    /// <summary>
    /// シーンロード
    /// </summary>
    /// <param name="sceneName"></param>
    public void loadScene(SceneNames sceneName)
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName.ToString());
        if (scene.isLoaded == true)
        {
            return;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
    }

    /// <summary>
    /// シーンアンロード
    /// </summary>
    /// <param name="sceneName"></param>
    public void unloadScene(SceneNames sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName.ToString());
    }


    #endregion

    #region Request
    /// <summary>
    /// タイトルに戻る
    /// </summary>
    public void requestToTitle()
    {
        _ToTitleRequest = true;
    }

    /// <summary>
    /// インゲームをリスタート
    /// </summary>
    public void requestRestartInGame()
    {
        _RestartInGameRequest = true;
    }

    /// <summary>
    /// チュートリアルシーン終了リクエスト
    /// </summary>
    public void requestEndTutorialScene()
    {
        _EndTutorialRequest = true;
    }
    #endregion Request
}
