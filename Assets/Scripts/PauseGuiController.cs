using System;
using System.ComponentModel;
using UnityEngine;

public class PauseGuiController : GuiControllerBase
{
    public class SelectWaitParam
    {
        public Action onDecide;
        public Action onCancel;
    }

    public override GuiManager.GuiType GuiType => GuiManager.GuiType.Pause;


    #region Property
    [DisplayName("�͂�/�������I���p�l��")]
    public GameObject SelectPanel
    {
        get => _SelectPanel;
        set => _SelectPanel = value;
    }
    [SerializeField]
    private GameObject _SelectPanel = null;

    #endregion Property

    private PauseRequester _PauseRequester = null;
    private SelectWaitParam _SelectWaitParam = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.TryGetComponent(typeof(PauseRequester), out var requester);
        if (requester != null)
        {
            _PauseRequester = requester as PauseRequester;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void onClose()
    {
        base.onClose();

        _SelectPanel.SetActive(false);
        _SelectWaitParam = null;
    }

    /// <summary>
    /// �|�[�Y��߂�{�^���N���b�N
    /// </summary>
    public void onResumeClick()
    {
        if (_PauseRequester != null)
        {
            // �|�[�Y�I��
            _PauseRequester.endPause();
        }
    }

    /// <summary>
    /// �^�C�g���ɖ߂�{�^���N���b�N
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
            // �^�C�g����
            SceneManager.Instance.requestToTitle();

            // �|�[�Y�I��
            if (_PauseRequester != null)
            {
                _PauseRequester.endPause();
            }
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
            onCancel = onCancel,
        };

        _SelectPanel.SetActive(true);
        setActive(false);
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
