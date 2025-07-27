using GuiUtil;
using System.ComponentModel;
using UnityEngine;

public class GameClearGuiController : GuiControllerBase
{
    public class  OpenParam : OpenParamBase
    {
        
    }

    public override GuiManager.GuiType GuiType => GuiManager.GuiType.GameClear;

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

    #region Field
    private SelectWaitParam _SelectWaitParam = null;
    #endregion Field
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public override void onClose()
    {
        base.onClose();

        _SelectPanel.SetActive(false);
        _SelectWaitParam = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���X�^�[�g�{�^���N���b�N
    /// </summary>
    public void onRestartClick()
    {
        // ���X�^�[�g���N�G�X�g
        SceneManager.Instance.requestRestartInGame();

        // �Q�[���N���A��ʏI��
        GameClearManager.Instance.requestEndGameClear();
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

            //�Q�[���N���A��ʏI��
            GameClearManager.Instance.requestEndGameClear();
        }


        void onCancel()
        {
            // �O�̉�ʂɖ߂�
            _SelectPanel.SetActive(false);
            setActive(true);
            _SelectWaitParam = null;
        }

        _SelectWaitParam = new SelectWaitParam()
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
