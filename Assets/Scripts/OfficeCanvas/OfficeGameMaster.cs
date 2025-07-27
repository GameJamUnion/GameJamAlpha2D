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
    [SerializeField] List<BaseUnit> _reserveUnits;

    [Header("�I��")]
    [SerializeField] BaseUnit _selectUnit;

    [Header("ID")]
    [SerializeField] int _nextID = 0;

    #region �v���p�e�B
    public List<BaseUnit> ReserveUnits
    {
        get { return _reserveUnits; }
    }
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
        if (_reserveUnits.Count >= 3)
            return;

        GameObject newUnit = Instantiate(_createUnitPref);
        if (newUnit == null)
            return;
        newUnit.SetActive(true);

        BaseUnit newBaseUnit = newUnit.GetComponent<BaseUnit>();

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
            _reserveUnits.Add(newBaseUnit);
            newUnit.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
        }
    }

    public void Employment(BaseUnit baseUnit)
    {
        _baseUnits.Add(baseUnit);// �ٗp
        if (_selectUnit == null)// �������j�b�g���I������Ă��Ȃ��ꍇ
        {
            _selectUnit = _baseUnits[0];// ���j�b�g�̃Z�b�g
            _hiredListManager.HiredListUpdate();// �X�V
        }
        baseUnit.ResumeInterface.Close();
        _reserveUnits.Remove(baseUnit);// ���U�[�u�������
        baseUnit.ResumeInterface.gameObject.SetActive(false);// ��\���ɕύX
    }
    public void Reject(BaseUnit baseUnit)
    {
        _reserveUnits.Remove(baseUnit);// ���U�[�u�������
        Destroy(baseUnit.gameObject);// Unit�̍폜
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
}
