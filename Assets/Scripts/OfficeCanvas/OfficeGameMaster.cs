using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class OfficeGameMaster : MonoBehaviour
{
    [Header("アタッチ")]
    [SerializeField] GameObject _createUnitPref;
    [SerializeField] GameObject _parentUnits;
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;
    [SerializeField] HiredListManager _hiredListManager;

    [Header("生成")]
    [SerializeField] List<BaseUnit> _baseUnits;
    [SerializeField] List<BaseUnit> _reserveUnits;

    [Header("選択")]
    [SerializeField] BaseUnit _selectUnit;

    [Header("ID")]
    [SerializeField] int _nextID = 0;

    #region プロパティ
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
        // 雇用のために部屋に呼ぶ限界上限３体
        if (_reserveUnits.Count >= 3)
            return;

        GameObject newUnit = Instantiate(_createUnitPref);
        if (newUnit == null)
            return;
        newUnit.SetActive(true);

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
            newBaseUnit.Initialize(_nextID, this);
            _nextID++;
            newBaseUnit.transform.parent = _parentUnits.transform;
            _reserveUnits.Add(newBaseUnit);
            newUnit.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
        }
    }

    public void Employment(BaseUnit baseUnit)
    {
        _baseUnits.Add(baseUnit);// 雇用
        if (_selectUnit == null)// もしユニットが選択されていない場合
        {
            _selectUnit = _baseUnits[0];// ユニットのセット
            _hiredListManager.HiredListUpdate();// 更新
        }
        baseUnit.ResumeInterface.Close();
        _reserveUnits.Remove(baseUnit);// リザーブから消去
        baseUnit.ResumeInterface.gameObject.SetActive(false);// 非表示に変更
    }
    public void Reject(BaseUnit baseUnit)
    {
        _reserveUnits.Remove(baseUnit);// リザーブから消去
        Destroy(baseUnit.gameObject);// Unitの削除
    }

    // SelectUnit関連
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
