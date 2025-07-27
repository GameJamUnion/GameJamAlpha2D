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

    public virtual void OnEnter()
    {
        for (int i = 0; i < SceneName.Length; i++)
        {
            // �ΏۃV�[�����[�h
            SceneManager.Instance.loadScene(SceneName[i]);
        }
        
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

    public override void OnEnter()
    {
        base.OnEnter();

        SoundManager.Instance.requestPlaySound(BGMKind.Title);
    }

    public override SceneStateBase checkNext()
    {
        var result = base.checkNext();
        
        if (checkInput() == true)
        {
            return new GameLoadScene();
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

#region Setup
public class GameLoadScene : SceneStateBase
{
    public override SceneNames[] SceneName => new SceneNames[] { SceneNames.Loading };

    public class PreLoadOperationWork
    {
        public AsyncOperation Operation;
    }

    private List<PreLoadOperationWork> _OperationWork = new List<PreLoadOperationWork>(4);
    private float _CurrentProgressRate = 0f;

    public override void OnEnter()
    {
        base.OnEnter();

        var startScene = getStartInGameScene();
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

        PauseManager.Instance.requestStartPause(new PauseManager.PauseRequestArgs()
        {
            Owner = this.GetType(),
            Type = PauseType.InGamePause,
        });
    }

    public override void OnExit()
    {
        base.OnExit();
        PauseManager.Instance.requestEndPause(this.GetType());
        _OperationWork.Clear();
    }

    public override SceneStateBase checkNext()
    {
        updateWork();

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
    public InGameSceneStateBase getStartInGameScene()
    {
        return new InGameSceneState0();
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
    }
}
#endregion

#region InGame
public abstract class InGameSceneStateBase : SceneStateBase
{
    public override void OnEnter()
    {
        base.OnEnter();

        SoundManager.Instance.requestPlaySound(BGMKind.MainGame);
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
