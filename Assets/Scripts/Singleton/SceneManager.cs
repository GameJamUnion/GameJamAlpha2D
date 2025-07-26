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
    GameMastering = 1,
    Title = 2,
    komugi_workshop = 3,
    dev_ozaki = 4,

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
        SceneNames StartupSceneName = SceneNames.Title;
        var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        var sceneCount = (int)SceneNames.Max_Num;
        for (int i = 0; i < sceneCount; i++)
        {
            var sceneName = (SceneNames)i;
            if (scene.name == sceneName.ToString())
            {
                var loadScene = sceneName;
                if (sceneName == SceneNames.Master)
                {
                    // Masterシーン開始は指定シーンから開始
                    loadScene = StartupSceneName;
                }

                SceneManager.Instance.setDefaultSceneState(loadScene, loadScene != sceneName);
                return;
            }
        }
        SceneManager.Instance.setPendingState();
    }

    public void loadScene(SceneNames sceneName)
    {
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
    private void setDefaultSceneState(SceneNames sceneName, bool loadRequest)
    {
        var states = System.Reflection.Assembly.GetAssembly(typeof(SceneStateBase))
            .GetTypes().Where(x => x.IsSubclassOf(typeof(SceneStateBase)) && !x.IsAbstract).ToArray();

        for (int i = 0; i < states.Length; i++)
        {
            var state = (SceneStateBase)System.Activator.CreateInstance(states[i]);

            // HACK : シーンが複数ある場合微妙
            // TODO : 今のシーンを被る場合アンロードする？
            if (state.SceneName[0] == sceneName)
            {
                _CurrentState = state;
                _CurrentScene = state.SceneName;

                // 開始シーンとロード対象シーンが違えばロード処理呼び出す
                if (loadRequest)
                {
                    _CurrentState.OnEnter();
                }
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
