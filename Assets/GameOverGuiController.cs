using GuiUtil;
using System;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverGuiController : GuiControllerBase
{


    public class OpenParam : OpenParamBase
    {
        public int Score { get; set; } = 0;
        public float RemainingTime { get; set; } = 0.0f;
    }

    public override GuiManager.GuiType GuiType => GuiManager.GuiType.GameOver;

    #region Property
    [DisplayName("�͂�/�������I���p�l��")]
    public GameObject SelectPanel
    {
        get => _SelectPanel;
        set => _SelectPanel = value;
    }
    [SerializeField]
    private GameObject _SelectPanel = null;

    [DisplayName("�X�R�A�\���I�u�W�F�N�g")]
    public TextMeshProUGUI ScoreObject
    {
        get => _ScoreObject;
        set => _ScoreObject = value;
    }
    [SerializeField]
    private TextMeshProUGUI _ScoreObject = null;

    [DisplayName("�c�莞�ԕ\���I�u�W�F�N�g")]
    public TextMeshProUGUI RemainingTimeObject
    {
        get => _RemainingTimeObject;
        set => _RemainingTimeObject = value;
    }
    [SerializeField]
    private TextMeshProUGUI _RemainingTimeObject = null;
    #endregion

    #region Field
    private SelectWaitParam _SelectWaitParam = null;
    #endregion
    public override void onOpen(OpenParamBase openParam = null)
    {
        base.onOpen(openParam);
        var param = openParam as OpenParam;
        setParam(param);
    }

    /// <summary>
    /// ���ʕ񍐕\���ݒ�
    /// </summary>
    /// <param name="param"></param>
    private void setParam(OpenParam param)
    {
        if (ScoreObject != null)
        {
            ScoreObject.text = param.Score.ToString();
        }

        if (RemainingTimeObject != null)
        {
            RemainingTimeObject.text = param.RemainingTime.ToString("F0") + "�b";
        }
    }

    public override void onClose()
    {
        base.onClose();

        _SelectPanel.SetActive(false);
        _SelectWaitParam = null;
    }

    /// <summary>
    /// ���X�^�[�g�{�^���N���b�N
    /// </summary>
    public void onRestartClick()
    {
        // ���X�^�[�g���N�G�X�g
        SceneManager.Instance.requestRestartInGame();

        // �Q�[���I�[�o�[��ʏI��
        GameOverManager.Instance.requestEndGameOver();
    }

    /// <summary>
    /// �^�C�g���֖߂�{�^���N���b�N
    /// </summary>
    public void onToTitleClick()
    {
        if (_SelectPanel == null)
        {
            Debug.LogError("SelectPanel �� null");
            return;
        }

        void onDecide()
        {
            // �^�C�g���֖߂郊�N�G�X�g
            SceneManager.Instance.requestToTitle();

            // �Q�[���I�[�o�[��ʏI��
            GameOverManager.Instance.requestEndGameOver();
        }

        void onCancel()
        {
            // �O�̉�ʂɖ߂�
            _SelectPanel.SetActive(false);
            setActive(true);
            _SelectWaitParam = null;
        }

        _SelectWaitParam = new SelectWaitParam
        {
            onDecide = onDecide,
            onCancel = onCancel
        };

        _SelectPanel.SetActive(true);
    }

    /// <summary>
    /// �u�͂��v�{�^���N���b�N
    /// </summary>
    public void onDecideClick()
    {
        if (_SelectWaitParam != null)
        {
            _SelectWaitParam.onDecide?.Invoke();
        }
    }

    /// <summary>
    /// �u�������v�{�^���N���b�N
    /// </summary>
    public void onCancelClick()
    {
        if (_SelectWaitParam != null)
        {
            _SelectWaitParam.onCancel?.Invoke();
        }
    }
}
