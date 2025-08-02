using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;


#region Base


/// <summary>
/// �X�e�[�g���
/// �r���ŃX�e�[�g��؂�ւ���
/// �V�[������ɗ��p
/// </summary>
public abstract class SceneStateBase
{
    public abstract SceneNames[] SceneName { get; }

    public virtual SceneStateBase OnEnter()
    {
        for (int i = 0; i < SceneName.Length; i++)
        {
            // �ΏۃV�[�����[�h
            SceneManager.Instance.loadScene(SceneName[i]);
        }
        return null;
    }

    public virtual void OnExit()
    {
        for (int i = 0; i < SceneName.Length; i++)
        {
            // �ΏۃV�[���A�����[�h
            SceneManager.Instance.unloadScene(SceneName[i]);
        }        
    }

    /// <summary>
    /// ���̃X�e�[�g�ɐi�ނ��ǂ������`�F�b�N����
    /// </summary>
    /// <returns></returns>
    public virtual SceneStateBase checkNext()
    {
        return checkNextCommon();
    }

    public virtual SceneStateBase checkNextCommon()
    {
        return null;
    }
}
#endregion Base
#region Master
public class MasterSceneState : SceneStateBase
{
    public override SceneNames[] SceneName => new SceneNames[] { SceneNames.Master};
    public override SceneStateBase checkNext()
    {
        // �^�C�g����
        return new TitleSceneState();
    }
}
#endregion
#region Title

/// <summary>
/// �^�C�g���V�[������
/// </summary>
public class TitleSceneState : SceneStateBase
{
    public override SceneNames[] SceneName => new SceneNames[] { SceneNames.Title };

    public override SceneStateBase OnEnter()
    {
        base.OnEnter();

        FadeManager.Instance.requestStartFade(FadeManager.FadeType.FadeIn);

        SoundManager.Instance.requestPlaySound(BGMKind.Title);

        return null;
    }

    public override SceneStateBase checkNext()
    {
        var result = base.checkNext();
        
        if (checkInput() == true)
        {
            FadeManager.Instance.requestStartFade(FadeManager.FadeType.FadeOut);
            return new GameTutorialState();
        }

        return result;
    }

    /// <summary>
    /// �L�[���͂�����
    /// </summary>
    /// <returns></returns>
    private bool checkInput()
    {
        // �}�E�X�N���b�N
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        // �L�[����
        if (Input.anyKeyDown == true)
        {
            return true;
        }
        return false;
    }
}

#endregion Title

#region Tutorial
public class GameTutorialState : SceneStateBase
{
    public override SceneNames[] SceneName => new SceneNames[] { SceneNames.Tutorial };

    public override SceneStateBase OnEnter()
    {
        // �`���[�g���A���I���Ȃ瑦���[�h�V�[����
        if (checkEndTutorial() == true)
        {
            return new GameLoadSceneState();
        }

        base.OnEnter();

        FadeManager.Instance.requestStartFade(FadeManager.FadeType.FadeIn);

        return null;
    }
    public override SceneStateBase checkNext()
    {
        if (checkEndTutorial() == true)
        {
            FadeManager.Instance.requestStartFade(FadeManager.FadeType.FadeOut);
            return new GameLoadSceneState();
        }

        return base.checkNext();
    }

    /// <summary>
    /// �`���[�g���A���I���`�F�b�N
    /// </summary>
    /// <returns></returns>
    private bool checkEndTutorial()
    {
        if (SceneManager.Instance.checkEndTutorialScene() == true)
        {
            return true;
        }

        return false;
    }
}
#endregion Tutorial

#region Loading
public class GameLoadSceneState : SceneStateBase
{
    public override SceneNames[] SceneName => new SceneNames[] { SceneNames.Loading };

    // �������҂��� (���[�h��ʉ��o�p)
    public const float ForceWaitFrame = 0f;

    public class PreLoadOperationWork
    {
        public AsyncOperation Operation;
    }

    public enum Phase
    {
        UnloadWait,
        Loading,
    }

    private List<PreLoadOperationWork> _OperationWork = new List<PreLoadOperationWork>(4);
    private float _CurrentProgressRate = 0f;
    private Phase _CurrentPhase = Phase.UnloadWait;
    private float _ForceWaitFrameTimer = ForceWaitFrame;

    public override SceneStateBase OnEnter()
    {
        base.OnEnter();
        _CurrentPhase = Phase.UnloadWait;
        _ForceWaitFrameTimer = ForceWaitFrame;

        PauseManager.Instance.requestStartPause(new PauseManager.PauseRequestArgs()
        {
            Owner = this.GetType(),
            Type = PauseType.InGamePause,
        });

        return null;
    }

    public override void OnExit()
    {
        base.OnExit();
        PauseManager.Instance.requestEndPause(this.GetType());
        _OperationWork.Clear();
    }

    public override SceneStateBase checkNext()
    {
        switch (_CurrentPhase)
        {
            case Phase.UnloadWait:
                updateWait();
                break;
            case Phase.Loading:
                updateWork();
                break;
            default:
                break;
        }
        

        if (_CurrentProgressRate >= 1f)
        {
            // �C���Q�[���V�[����
            return new InGameSceneState0();
        }
        return null;
    }

    /// <summary>
    /// �J�n�C���Q�[���V�[�����擾
    /// </summary>
    /// <returns></returns>
    public InGameSceneStateBase getStartInGameSceneState()
    {
        return new InGameSceneState0();
    }

    /// <summary>
    /// �ҋ@�X�V
    /// </summary>
    private void updateWait()
    {
        var unloading = false;
        var nextState = getStartInGameSceneState();
        if (nextState != null)
        {
            var scenes = nextState.SceneName;
            for (int i = 0; i < scenes.Length; i++)
            {
                var isLoad = UnityEngine.SceneManagement.SceneManager.GetSceneByName(scenes[i].ToString()).isLoaded;
                unloading |= isLoad;
            }
        }

        // ���[�h�\��̃V�[�����ǂݍ��܂�Ă��Ȃ���΃��[�h�J�n����
        if (unloading == false)
        {
            changePhase(Phase.Loading);
        }
    }

    /// <summary>
    /// OperationWork�̍X�V
    /// </summary>
    private void updateWork()
    {
        var count = _OperationWork.Count;
        if (count == 0)
        {
            _CurrentProgressRate = 1f;
            return;
        }

        var rateSum = 0f;
        for (int i = 0; i < count; i++)
        {
            var work = _OperationWork[i];
            rateSum += work.Operation.progress;
        }

        _CurrentProgressRate = rateSum / (float)count;

        _ForceWaitFrameTimer = Mathf.Max(0, _ForceWaitFrameTimer - 1);
        if (_ForceWaitFrameTimer > 0)
        {
            _CurrentProgressRate = Mathf.Max(0, _CurrentProgressRate - 0.1f);
        }
    }

    /// <summary>
    /// Phase�؂�ւ�
    /// </summary>
    /// <param name="next"></param>
    private void changePhase(Phase next)
    {
        switch (next)
        {
            case Phase.UnloadWait:
                break;
            case Phase.Loading:
                startLoad();
                break;
            default:
                break;
        }

        _CurrentPhase = next;
    }

    /// <summary>
    /// �V�[�����[�h�J�n
    /// </summary>
    private void startLoad()
    {
        var startScene = getStartInGameSceneState();
        if (startScene != null)
        {
            var preLoadScene = startScene.SceneName;
            var sceneCount = preLoadScene.Length;
            if (sceneCount > 0)
            {
                for (int i = 0; i < sceneCount; i++)
                {
                    var scene = preLoadScene[i];
                    var work = new PreLoadOperationWork();
                    work.Operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);
                    _OperationWork.Add(work);
                }
            }
        }
    }
}
#endregion

#region InGame
public abstract class InGameSceneStateBase : SceneStateBase
{
    public override SceneStateBase OnEnter()
    {
        base.OnEnter();
        FadeManager.Instance.requestStartFade(FadeManager.FadeType.FadeIn);
        SoundManager.Instance.requestPlaySound(BGMKind.MainGame);

        return null;
    }
}

public class InGameSceneState0 : InGameSceneStateBase
{
    public override SceneNames[] SceneName => new SceneNames[] { SceneNames.komugi_workshop, SceneNames.dev_ozaki };
    public override SceneStateBase checkNext()
    {
        var result = base.checkNext();
        
        return result;
    }
}
#endregion InGame
