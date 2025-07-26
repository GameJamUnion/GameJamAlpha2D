using UnityEngine;
using System.Collections.Generic;

public class OfficeGameMaster : MonoBehaviour
{
    [Header("アタッチ")]
    [SerializeField] GameObject _createUnitPref;
    [SerializeField] GameObject _parentUnits;
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;

    [Header("生成")]
    [SerializeField] List<BaseUnit> _baseUnits;
    [SerializeField] List<BaseUnit> _reserveUnits;

    [Header("選択")]
    [SerializeField] BaseUnit _selectUnit;

    #region プロパティ
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
        // 雇用のために部屋に呼ぶ限界上限３体
        if (_reserveUnits.Count >= 3)
            return;

        GameObject newUnit = Instantiate(_createUnitPref);
        if (newUnit == null)
            return;

        BaseUnit newBaseUnit = newUnit.GetComponent<BaseUnit>();

        if (newBaseUnit == null)// 失敗
        {
            Destroy(newUnit);// ユニットの消去
            Debug.Log("#Elalice : OfficeGameMaster > CreateUnit // Error");
            return;
        }

        // 成功
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
