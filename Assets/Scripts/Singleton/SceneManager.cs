using System.ComponentModel;
using System.Linq;
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

    [Browsable(false)]
    Max_Num,
}

public class SceneManager : SingletonBase<SceneManager>
{
    private SceneNames[] _CurrentScene = new SceneNames[1] { SceneNames.Invalid };
    private SceneStateBase _CurrentState = null;
    private bool _IsPending = false;

    #region Request
    private bool _ToTitleRequest = false;
    #endregion


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
        SceneManager.Instance.setPendingState();
    }

    public void loadScene(SceneNames sceneName)
    {
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (scene.name == sceneName.ToString())
        {
            return;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
    }

    public void unloadScene(SceneNames sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName.ToString());
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
            _ToTitleRequest = false;
        }

        if (nextState != null)
        {
            // 次のステートへ移行
            _CurrentState.OnExit();
            _CurrentState = nextState;
            _CurrentScene = nextState.SceneName;
            _CurrentState.OnEnter();
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

    #region Request
    public void requestToTitle()
    {
        _ToTitleRequest = true;
    }
    #endregion Request
}
