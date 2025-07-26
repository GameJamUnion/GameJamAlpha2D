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

    [Browsable(false)]
    Max_Num,
}

public class SceneManagementManager : SingletonBase<SceneManagementManager>
{
    private SceneNames _CurrentScene = SceneNames.Invalid;
    private SceneStateBase _CurrentState = null;
    private bool _IsPending = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void SceneInitialized()
    {
        SceneNames StartupSceneName = SceneNames.Title;
        var scene = SceneManager.GetActiveScene();
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

                SceneManagementManager.Instance.setDefaultSceneState(loadScene, loadScene != sceneName);
                return;
            }
        }
        SceneManagementManager.Instance.setPendingState();
    }

    public void loadScene(SceneNames sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
    }

    public void unloadScene(SceneNames sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName.ToString());
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

            if (state.SceneName == sceneName)
            {
                _CurrentState = state;
                _CurrentScene = sceneName;

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
        var nextState = _CurrentState.checkNext();
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
}
