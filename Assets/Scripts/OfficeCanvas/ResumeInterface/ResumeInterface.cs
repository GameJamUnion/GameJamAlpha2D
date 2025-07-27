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
   
    private void SetResumeInterface()
    {
        gameObject.SetActive(_active);

        _tName.text = _baseResume.Name;
        _tSection1Rank.text = _baseResume.ProductionEfficiency1;
        _tSection2Rank.text = _baseResume.ProductionEfficiency2;
        _tSection3Rank.text = _baseResume.ProductionEfficiency3;

        _stamp.SetActive(_baseResume.OnStamp);
        _stamp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, _baseResume.StampRotation);

        // �Z�N�V������UI������
        RIPlacementUpdate();
    }

    public void OnStamp()
    {
        // �X�^���v�{�^���������ꂽ�ꍇ�̏���
        _baseResume.OnStamp = true;
        _baseResume.OnStampProcess();
        _officeGameMaster.Employment(_baseResume.BaseUnit);
    }
    public void SectionChange()
    {
        _baseResume.BaseUnit.ChangePlacementState();
        RIPlacementUpdate();
    }
    public void Close()
    {
        
    }
    public void Hold()
    {
        // �}�E�X�̈ړ������擾
        Vector3 newPos = _resumeInterfaceCustomButton.CurrentMousePos - Input.mousePosition;
        // �ړ����������W�����炷
        this.gameObject.transform.localPosition = _resumeInterfaceCustomButton.SaveRusumeInterfacePos - newPos;
    }

    // Update
    void RIPlacementUpdate()
    {
        _riPlacementManager.ChangePlacementState(_baseResume.BaseUnit.PlacementState);
    }
}
