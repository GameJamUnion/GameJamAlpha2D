using UnityEngine;
using UnityEngine.UI;
using TMPro;

// �������C���^�[�t�F�C�X�������Ă���
public class ResumeInterface : MonoBehaviour
{
    [SerializeField] OfficeGameMaster _officeGameMaster;

    [SerializeField] bool _active = false;

    [SerializeField] BaseResume _baseResume;
    [SerializeField] TextMeshProUGUI _tName;
    [SerializeField] TextMeshProUGUI _tSection1Rank;
    [SerializeField] TextMeshProUGUI _tSection2Rank;
    [SerializeField] TextMeshProUGUI _tSection3Rank;
    [SerializeField] GameObject _stamp;

    [Header("�ő�ړ�����")]
    [SerializeField] Vector2 _maxTopBottom;
    [SerializeField] Vector2 _maxRightLeft;

    [Header("�A�^�b�`")]
    [SerializeField] StampCustomButton _stampCustomButton;
    [SerializeField] ResumeInterfaceCustomButton _resumeInterfaceCustomButton;
    [SerializeField] RIStampManager _riStampManager;
    [SerializeField] RIPlacementManager _riPlacementManager;

    #region �v���p�e�B
    public BaseResume BaseResume
    {
        get { return _baseResume; }
    }
    public bool Active
    {
        get { return _active; }
    }
    public bool HoldFlg
    {
        get { return _resumeInterfaceCustomButton.HoldTap; }
    }
    #endregion

    private void Update()
    {
        if (_resumeInterfaceCustomButton.HoldTap)
        {
            Hold();
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    public void Initialize(BaseResume baseResume)
    {
        _active = true;
        _baseResume = baseResume;
        if (_baseResume != null)
        {
            SetResumeInterface();
        }
        else
        {
            Debug.Log("#Elalice:ResumeInterface > Initialize /// Error");
        }
    }

    public void SetDefault()
    {
        _tName.text = "NoData";
        _tSection1Rank.text = "-No Data-";
        _tSection2Rank.text = "-No Data-";
        _tSection3Rank.text = "-No Data-";
        _riPlacementManager.ChangePlacementState(RI.PlacementState.NONE);
        _stamp.SetActive(false);
    }

    private void SetResumeInterface()
    {
        gameObject.SetActive(_active);

        _tName.text = _baseResume.Name;
        _tSection1Rank.text = _baseResume.ProductionEfficiency1;
        _tSection2Rank.text = _baseResume.ProductionEfficiency2;
        _tSection3Rank.text = _baseResume.ProductionEfficiency3;

        _stamp.SetActive(_baseResume.OnStamp);
        _stamp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, _baseResume.StampRotation);

        DisplayAtTheTop();
        // �Z�N�V������UI������
        RIPlacementUpdate(true);
    }

    public void OnStamp()
    {
        // �X�^���v�{�^���������ꂽ�ꍇ�̏���
        _baseResume.OnStamp = true;
        _baseResume.OnStampProcess();

        SoundManager.Instance.requestPlaySound(SEKind.Stamp);

        DisplayAtTheTop();
    }
    public void SectionChange()
    {
        _baseResume.BaseUnit.ChangePlacementState();
        RIPlacementUpdate(false);

        DisplayAtTheTop();
    }
    public void Close()
    {
        _active = false;
    }
    public void Hold()
    {
        // �}�E�X�̈ړ������擾
        Vector3 newPos = _resumeInterfaceCustomButton.SaveRusumeInterfacePos - (_resumeInterfaceCustomButton.CurrentMousePos - Input.mousePosition);
        // �͈͊O�`�F�b�N
        newPos.x = Mathf.Clamp(newPos.x, _maxRightLeft.y, _maxRightLeft.x);
        newPos.y = Mathf.Clamp(newPos.y, _maxTopBottom.y, _maxTopBottom.x);

        // �ړ����������W�����炷
        this.gameObject.GetComponent<RectTransform>().transform.localPosition =  newPos;

        DisplayAtTheTop();
    }
    public void Rejected()
    {
        _active = false;
        gameObject.SetActive(false);

        _officeGameMaster.Reject(_baseResume.BaseUnit);
    }

    // Update
    void RIPlacementUpdate(bool init)
    {
        if (_officeGameMaster.AmIInTheRoom(_baseResume.BaseUnit) || init)
        {
            _riPlacementManager.ChangePlacementState(_baseResume.BaseUnit.PlacementState);
        }
        else
        {
            Debug.Log("�������ɂ��Ȃ�����܂��͌Ă΂Ȃ���...");
        }
    }

    public void DisplayAtTheTop()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
