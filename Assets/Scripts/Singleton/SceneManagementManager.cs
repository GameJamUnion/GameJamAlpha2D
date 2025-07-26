using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BuildProfiles��SceneList�ƈ�v������ (����)
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
                    // Master�V�[���J�n�͎w��V�[������J�n
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
    /// �J�n�V�[���X�e�[�g��ݒ�
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

                // �J�n�V�[���ƃ��[�h�ΏۃV�[�����Ⴆ�΃��[�h�����Ăяo��
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
    /// �����~��Ԃɂ���
    /// </summary>
    private void setPendingState()
    {
        // �s�����
        // �J���V�[���Ƃ��Ȃ̂Ő؂�ւ���~
        _IsPending = true;
    }

    #region LateUpdate
    public override void LateUpdate()
    {
        updateSceneState();
    }

    /// <summary>
    /// State�̍X�V
    /// </summary>
    private void updateSceneState()
    {
        if (_IsPending)
        {
            return;
        }
        // ���݂̃V�[���X�e�[�g���X�V
        var nextState = _CurrentState.checkNext();
        if (nextState != null)
        {
            // ���̃X�e�[�g�ֈڍs
            _CurrentState.OnExit();
            _CurrentState = nextState;
            _CurrentScene = nextState.SceneName;
            _CurrentState.OnEnter();
        }
    }
    #endregion
}
