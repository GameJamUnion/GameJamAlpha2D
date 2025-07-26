using UnityEngine;
using System.Collections.Generic;

public class OfficeGameMaster : MonoBehaviour
{
    [Header("�A�^�b�`")]
    [SerializeField] GameObject _createUnitPref;
    [SerializeField] GameObject _parentUnits;
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;

    [Header("����")]
    [SerializeField] List<BaseUnit> _baseUnits;
    [SerializeField] List<BaseUnit> _reserveUnits;

    [Header("�I��")]
    [SerializeField] BaseUnit _selectUnit;

    #region �v���p�e�B
    public List<BaseUnit> ReserveUnits
    {
        get { return _reserveUnits; }
    }
    #endregion

    public void Initialize()
    {
        CreateUnit();
    }

    void Update()
    {
        
    }

    void CreateUnit()
    {
        // �ٗp�̂��߂ɕ����ɌĂԌ��E����R��
        if (_reserveUnits.Count >= 3)
            return;

        GameObject newUnit = Instantiate(_createUnitPref);
        if (newUnit == null)
            return;

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
            newBaseUnit.Initialize(this);
            newBaseUnit.transform.parent = _parentUnits.transform;
            _reserveUnits.Add(newBaseUnit);
            _baseUnits.Add(newBaseUnit);
        }
    }

    public void Employment(BaseUnit baseUnit)
    {
        _reserveUnits.Remove(baseUnit);
    }
}
