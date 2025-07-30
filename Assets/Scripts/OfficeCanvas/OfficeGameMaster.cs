using OfficeGameMasterDebug;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OfficeGameMaster : MonoBehaviour
{
    [Header("�A�^�b�`")]
    [SerializeField] GameObject _createUnitPref;
    [SerializeField] GameObject _parentUnits;
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;
    [SerializeField] HiredListManager _hiredListManager;

    [Header("����")]
    [SerializeField] List<BaseUnit> _baseUnits;// �ٗp���̃��j�b�g
    //[SerializeField] List<BaseUnit> _reserveUnits;
    [SerializeField] List<BaseUnit> _inRoomUnits;// �����̒��ɂ��郆�j�b�g
    [SerializeField] List<BaseUnit> _reserveUnits;// ������������Ă��郆�j�b�g

    [SerializeField] int _maxReserveUnits = 6;

    [Header("�I��")]
    [SerializeField] BaseUnit _selectUnit;

    [Header("ID")]
    [SerializeField] int _nextID = 0;

    [Header("UnitContainer")]
    [SerializeField] UnitContainer _unitContainer;
    [Header("NameTable")]
    [SerializeField] NameTable _nameTable;
    [Header("UnitAbilityCanvasUI")]
    [SerializeField] UnitAbilityCanvasUI _unitAbilityCanvasUI;
    [Header("AbilityTable")]
    [SerializeField] AbilityTable _abilityTable;

    [Header("RandomStatusCreation")]
    [SerializeField] TableSwitch _tableSwitch;
    [SerializeField] FullCustomRandomStatus _fullCustomRandomStatus;

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

        for (; _maxReserveUnits > _reserveUnits.Count;)
        {
            CreateUnit();
        }
    }

    void Update()
    {

    }

    private void CreateUnit()
    {
        GameObject newUnit = Instantiate(_createUnitPref);
        if (newUnit == null)
            return;
        newUnit.SetActive(true);

        BaseUnit newBaseUnit = newUnit.GetComponent<BaseUnit>();

        switch (_tableSwitch)
        {
            case TableSwitch.FullCustom:
                newBaseUnit.SetState(CreateFullCustomRamdomStatus());
                break;
        }

        if (newBaseUnit == null)// null�`�F�b�N
        {
            Destroy(newUnit);// ���j�b�g�̏���
            Debug.Log("#Elalice : OfficeGameMaster > CreateUnit // Error");
            return;
        }
        newBaseUnit.IntervieweeUnitAbility = _abilityTable.CreateIntervieweeUnitAbility();
        newBaseUnit.transform.parent = _parentUnits.transform;

        // ���U�[�u�ɓ���Ă���
        _reserveUnits.Add(newBaseUnit);
    }

    #region RandomStatus
    private BaseUnit CreateFullCustomRamdomStatus()
    {
        ProductionEfficiencys productionEfficiencys = _fullCustomRandomStatus.GetRandomProductionEfficiencys();

        BaseUnit newBaseUnit = new BaseUnit();
        newBaseUnit.SetState(
            _nameTable.Names[Random.Range(0, 20000) % _nameTable.Names.Count],
            productionEfficiencys.ProductionEfficiency1,
            productionEfficiencys.ProductionEfficiency2,
            productionEfficiencys.ProductionEfficiency3);

        return newBaseUnit;
    }

    /// <summary>
    /// ���G�Ń����_���X�e�[�^�X����
    /// </summary>
    /// <returns></returns>
    private BaseUnit CreateSimpleAutoRandomStatus()
    {

        return null;
    }

    /// <summary>
    /// ���G�Ń����_���X�e�[�^�X����
    /// </summary>
    /// <returns></returns>
    private BaseUnit CreateComplexAutoRandomStatus()
    {
        return null;
    }

    #endregion RandomStatus

    // �Ăэ���
    public void ComeInUnit()
    {
        // �ٗp�̂��߂ɕ����ɌĂԌ��E����R��
        if (_inRoomUnits.Count >= 1)
            return;

        if (_reserveUnits.Count == 0)
            return;

        BaseUnit newBaseUnit = _reserveUnits[0];

        _reserveUnits.Remove(newBaseUnit);
        CreateUnit();

        if (_resumeInterfaceManager.AssignInactiveResumeInterface)
        {
            newBaseUnit.ResumeInterface = _resumeInterfaceManager.GetInactiveResumeInterface();
            newBaseUnit.ResumeInterface.gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(10f * ((float)_resumeInterfaceManager.GetInactiveResumeInterfaceCount() - 1f), 10f * ((float)_resumeInterfaceManager.GetInactiveResumeInterfaceCount() - 1), 0f);
            newBaseUnit.Initialize(_nextID, this);
            _nextID++;
            _inRoomUnits.Add(newBaseUnit);
            newBaseUnit.gameObject.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
            _unitAbilityCanvasUI.OnDisplay(newBaseUnit.IntervieweeUnitAbility);
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
        _unitAbilityCanvasUI.OnHide();

        // ���Ƃ肠�����u�Ԍٗp
        _unitContainer.Hired(baseUnit);
    }
    public void Reject(BaseUnit baseUnit)
    {
        _unitAbilityCanvasUI.OnHide();
        _inRoomUnits.Remove(baseUnit);// ��������ޏo
        Destroy(baseUnit.gameObject);// Unit�̍폜
    }
    // �Ăяo������
    public void Call()
    {
        if (_baseUnits.Count <= 0)
            return;

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
    // ����
    public void Fire()
    {
        // ���j�b�g�����݂��Ȃ��ꍇ�I��
        if (_baseUnits.Count == 0)
            return;

        // �����ɂ��Ȃ��ꍇ�I��
        if (!_inRoomUnits.Contains(_selectUnit))
            return;

        // ���̃��j�b�g�ɓn�鏈��
        int no = _baseUnits.IndexOf(_selectUnit);
        _baseUnits.Remove(_selectUnit);
        _inRoomUnits.Remove(_selectUnit);
         if (no >= _baseUnits.Count)
        {
            no = 0;
        }
        Destroy(_selectUnit.gameObject);
        if(_baseUnits.Count != 0)
        {
            // ��������Ԃ̌ٗp���X�g�ɕύX
            _selectUnit = _baseUnits[no];
            // �X�V
            _hiredListManager.HiredListUpdate();
        }
        else
        {
            // �����\������Ă��Ȃ���ԂɕύX
            _hiredListManager.SetDefault();
        }
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

namespace OfficeGameMasterDebug
{
    public enum TableSwitch
    {
        FullCustom,
    }
}
