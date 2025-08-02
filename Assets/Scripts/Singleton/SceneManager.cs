using System.ComponentModel;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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

    Tutorial,

    [Browsable(false)]
    Max_Num,
}

public class SceneManager : SingletonBase<SceneManager>
{
    /// <summary>
    /// �i�s���̃A�����[�h�f�[�^�Ǘ�
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
    /// �Q�[���Đ����ɌĂ΂��
    /// </summary>
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

        // �o�^����Ă���V�[���ȊO�ŊJ�n�����ꍇ�A�V�[��������s��Ȃ��ҋ@��Ԃɂ���
        SceneManager.Instance.setPendingState();
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
        }
        else if (_RestartInGameRequest == true)
        {
            nextState = new GameLoadSceneState();
        }

        _ToTitleRequest = false;
        _RestartInGameRequest = false;

        if (nextState != null)
        {
            // ���̃X�e�[�g�ֈڍs
            _CurrentState.OnExit();
            _CurrentState = nextState;

            // �X�L�b�v���Ă���Ɏ��̃V�[���Ɉڍs����ꍇ��
            // ����Ɏ��̃V�[���Đ������݂�
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

    /// <summary>
    /// �`���[�g���A���I�����Ă�����
    /// </summary>
    /// <returns></returns>
    public bool checkEndTutorialScene()
    {
        return _EndTutorialRequest == true;
    }

    #region Load

    /// <summary>
    /// �V�[�����[�h
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
    /// �V�[���A�����[�h
    /// </summary>
    /// <param name="sceneName"></param>
    public void unloadScene(SceneNames sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName.ToString());
    }


    #endregion

    #region Request
    /// <summary>
    /// �^�C�g���ɖ߂�
    /// </summary>
    public void requestToTitle()
    {
        _ToTitleRequest = true;
    }

    /// <summary>
    /// �C���Q�[�������X�^�[�g
    /// </summary>
    public void requestRestartInGame()
    {
        _RestartInGameRequest = true;
    }

    /// <summary>
    /// �`���[�g���A���V�[���I�����N�G�X�g
    /// </summary>
    public void requestEndTutorialScene()
    {
        _EndTutorialRequest = true;
    }
    #endregion Request
}
