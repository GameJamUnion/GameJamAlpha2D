using UnityEngine;


#region Base


/// <summary>
/// �X�e�[�g���
/// �r���ŃX�e�[�g��؂�ւ���
/// �V�[������ɗ��p
/// </summary>
public abstract class SceneStateBase
{
    public abstract SceneNames SceneName { get; }

    public void OnEnter()
    {
        // �ΏۃV�[�����[�h
        SceneManagementManager.Instance.loadScene(SceneName);
    }

    public void OnExit()
    {
        // �ΏۃV�[���A�����[�h
        SceneManagementManager.Instance.unloadScene(SceneName);
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

#region Title

/// <summary>
/// �^�C�g���V�[������
/// </summary>
public class TitleSceneState : SceneStateBase
{
    public override SceneNames SceneName => SceneNames.Title;

    public override SceneStateBase checkNext()
    {
        var result = base.checkNext();
        
        if (checkInput() == true)
        {
            // �C���Q�[���V�[����
            return new InGameSceneState0();
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

#region InGame
public abstract class InGameSceneStateBase : SceneStateBase
{
}

public class InGameSceneState0 : InGameSceneStateBase
{
    public override SceneNames SceneName => SceneNames.komugi_workshop;
    public override SceneStateBase checkNext()
    {
        var result = base.checkNext();
        
        return result;
    }
}
#endregion InGame
