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
        
        // �풓�V�[�������[�h
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
    /// �J�n�V�[���X�e�[�g��ݒ�
    /// </summary>
    /// <param name="sceneName"></param>
    private void setDefaultSceneState(SceneNames sceneName)
    {
        var states = System.Reflection.Assembly.GetAssembly(typeof(SceneStateBase))
            .GetTypes().Where(x => x.IsSubclassOf(typeof(SceneStateBase)) && !x.IsAbstract).ToArray();

        for (int i = 0; i < states.Length; i++)
        {
            var state = (SceneStateBase)System.Activator.CreateInstance(states[i]);

            // �w��V�[�������[�h����
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
        var nextState = _CurrentState?.checkNext();

        if (_ToTitleRequest == true)
        {
            // �^�C�g���֖߂郊�N�G�X�g������΃^�C�g���ֈڍs
            nextState = new TitleSceneState();
            _ToTitleRequest = false;
        }

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
    /// <summary>
    /// �C���Q�[���V�[����
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
