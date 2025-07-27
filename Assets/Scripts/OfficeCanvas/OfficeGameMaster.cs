using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class OfficeGameMaster : MonoBehaviour
{
    [Header("�A�^�b�`")]
    [SerializeField] GameObject _createUnitPref;
    [SerializeField] GameObject _parentUnits;
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;
    [SerializeField] HiredListManager _hiredListManager;

    [Header("����")]
    [SerializeField] List<BaseUnit> _baseUnits;
    //[SerializeField] List<BaseUnit> _reserveUnits;
    [SerializeField] List<BaseUnit> _inRoomUnits;

    [Header("�I��")]
    [SerializeField] BaseUnit _selectUnit;

    [Header("ID")]
    [SerializeField] int _nextID = 0;

    [Header("UnitContainer")]
    [SerializeField] UnitContainer _unitContainer;
    [Header("NameTable")]
    [SerializeField] NameTable _nameTable;

    #region �v���p�e�B
    //public List<BaseUnit> ReserveUnits
    //{
    //    get { return _reserveUnits; }
    //}
    public BaseUnit SelectUnit
    {
        get { return _selectUnit; }
    }
    #endregion

    public void Initialize()
    {
        //CreateUnit();
    }

    void Update()
    {

    }

    public void CreateUnit()
    {
        // �ٗp�̂��߂ɕ����ɌĂԌ��E����R��
        if (_inRoomUnits.Count >= 3)
            return;

        GameObject newUnit = Instantiate(_createUnitPref);
        if (newUnit == null)
            return;
        newUnit.SetActive(true);

        BaseUnit newBaseUnit = newUnit.GetComponent<BaseUnit>();

        // ���j�b�g�̃X�e�[�^�X�������_����
        newBaseUnit.SetState(
            _nameTable.Names[Random.Range(0, 20000) % _nameTable.Names.Count],
            Random.Range(0f, 2f),
            Random.Range(0f, 2f),
            Random.Range(0f, 2f)
            );

        if (newBaseUnit == null)// ���s
        {
            Destroy(newUnit);// ���j�b�g�̏���
            Debug.Log("#Elalice : OfficeGameMaster > CreateUnit // Error");
            return;
        }

        // ����
        if (_resumeInterfaceManager.AssignInactiveResumeInterface)
        {
            newBaseUnit.ResumeInterface = _resumeInterfaceManager.GetInactiveResumeInterface();
            newBaseUnit.Initialize(_nextID, this);
            _nextID++;
            newBaseUnit.transform.parent = _parentUnits.transform;
            _inRoomUnits.Add(newBaseUnit);
            newUnit.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
        }
    }

    // �ٗp����
    public void Employment(BaseUnit baseUnit)
    {
        _baseUnits.Add(baseUnit);// �ٗp
        if (_selectUnit == null)// �������j�b�g���I������Ă��Ȃ��ꍇ
        {
            _selectUnit = _baseUnits[0];// ���j�b�g�̃Z�b�g
            _hiredListManager.HiredListUpdate();// �X�V
        }
        baseUnit.ResumeInterface.Close();
        _inRoomUnits.Remove(baseUnit);// ��������ޏo
        baseUnit.ResumeInterface.gameObject.SetActive(false);// ��\���ɕύX
        baseUnit.gameObject.SetActive(false);// �Ƃ肠�����ق������\��

        // ���Ƃ肠�����u�Ԍٗp
        _unitContainer.Hired(baseUnit);
    }
    public void Reject(BaseUnit baseUnit)
    {
        _inRoomUnits.Remove(baseUnit);// ��������ޏo
        Destroy(baseUnit.gameObject);// Unit�̍폜
    }
    // �Ăяo������
    public void Call()
    {
        foreach (var item in _inRoomUnits)
        {
            if(item.Origin == _selectUnit.Origin)
            {
                // ���łɌĂяo����Ă���
                Debug.Log("���̍�Ə�ɖ߂�Ȃ����B");
                _unitContainer.CallBack(_selectUnit);
                _inRoomUnits.Remove(_selectUnit);
                _selectUnit.gameObject.SetActive(false);
                return;
            }
        }
        
        Debug.Log("�������������ɗ��Ȃ����B");
        // ��������
        if (_inRoomUnits.Count >= 3)
        {
            return;
        }

        // ���u�ԓI�ɌĂяo�����
        _unitContainer.Call(_selectUnit);
        _inRoomUnits.Add(_selectUnit);
        _selectUnit.gameObject.SetActive(true);
    }

    // SelectUnit�֘A
    public void NextSelectUnit()
    {
        if (_baseUnits.Count == 0)
            return;

        int no = _baseUnits.IndexOf(_selectUnit) + 1;
        if (no >= _baseUnits.Count)
        {
            no = 0;
        }
        _selectUnit = _baseUnits[no];
        _hiredListManager.HiredListUpdate();
    }
    public void PrevSelectUnit()
    {
        if (_baseUnits.Count == 0)
            return;

        int no = _baseUnits.IndexOf(_selectUnit) - 1;
        if (no < 0)
        {
            no = _baseUnits.Count - 1;
        }
        _selectUnit = _baseUnits[no];
        _hiredListManager.HiredListUpdate();
    }

    public bool AmIInTheRoom(BaseUnit baseUnit)
    {
        foreach (var item in _inRoomUnits)
        {
            if(item.Origin == baseUnit.Origin)
            {
                return true;
            }
        }
        return false;
    }
}
