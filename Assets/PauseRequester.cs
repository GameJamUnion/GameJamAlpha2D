using System.Runtime.Serialization.Json;
using UnityEngine;

/// <summary>
/// �|�[�Y��ʊJ�����N�G�X�g�𑗂�I�u�W�F�N�g
/// </summary>
public class PauseRequester : MonoBehaviour
{
    private bool _Paused = false;
    private bool _RequestStart = false;
    private bool _RequestEnd = false;
    // Update is called once per frame
    void Update()
    {
        if (checkEnablePause() == false)
        {
            // �|�[�Y�s��
            return;
        }

        if (checkPauseInput() == true)
        {
            if (PauseManager.Instance.checkPauseAny() == true)
            {
                requestEndPause();
            }
            else
            {
                requestStartPause();
            }
        }

    }

    private void LateUpdate()
    {
        // ���N�G�X�g����
        if (_RequestEnd == true)
        {
            endPause();
        }

        if (_RequestStart == true)
        {
            startPause();
        }

        _RequestStart = false;
        _RequestEnd = false;
    }

    private void OnDestroy()
    {
        endPause();
    }

    /// <summary>
    /// �|�[�Y�J�n����
    /// </summary>
    public void startPause()
    {
        if (_Paused == true)
        {
            // ���Ƀ|�[�Y���N�G�X�g�ς�
            return;
        }
        _Paused = true;

        // �|�[�Y�J�n
        PauseManager.Instance.requestStartPause(new PauseManager.PauseRequestArgs()
        {
            Owner = this.GetType(),
            Type = PauseType.InGamePause,
        });

        // GUI�J��
        GuiManager.Instance.requestOpenGui(GuiManager.GuiType.Pause, new OpenParamBase() { });
    }

    /// <summary>
    /// �|�[�Y�I������
    /// </summary>
    public void endPause()
    {
        if (_Paused == false)
        {
            // �|�[�Y���N�G�X�g���Ă��Ȃ�
            return;
        }
        _Paused = false;

        // GUI����
        GuiManager.Instance.requestCloseGui(GuiManager.GuiType.Pause);

        // �|�[�Y�I��
        PauseManager.Instance.requestEndPause(this.GetType());
    }

    /// <summary>
    /// �|�[�Y�\��Ԃ�
    /// </summary>
    /// <returns></returns>
    private bool checkEnablePause()
    {
        if (SceneManager.Instance.checkInGameScene() == false)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// �|�[�Y�L�[���͂��`�F�b�N
    /// </summary>
    /// <returns></returns>
    private bool checkPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// �|�[�Y�J�n���N�G�X�g
    /// </summary>
    public void requestStartPause()
    {
        _RequestStart = true;
    }

    /// <summary>
    /// �|�[�Y�I�����N�G�X�g
    /// </summary>
    public void requestEndPause()
    {
        _RequestEnd = true;
    }
}
