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
    //[SerializeField] List<BaseUnit> _reserveUnits;
    [SerializeField] List<BaseUnit> _inRoomUnits;

    [Header("選択")]
    [SerializeField] BaseUnit _selectUnit;

    [Header("ID")]
    [SerializeField] int _nextID = 0;

    [Header("UnitContainer")]
    [SerializeField] UnitContainer _unitContainer;
    [Header("NameTable")]
    [SerializeField] NameTable _nameTable;

    #region プロパティ
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
        // 雇用のために部屋に呼ぶ限界上限３体
        if (_inRoomUnits.Count >= 3)
            return;

        GameObject newUnit = Instantiate(_createUnitPref);
        if (newUnit == null)
            return;
        newUnit.SetActive(true);

        BaseUnit newBaseUnit = newUnit.GetComponent<BaseUnit>();

        // ユニットのステータスをランダム化
        newBaseUnit.SetState(
            _nameTable.Names[Random.Range(0, 20000) % _nameTable.Names.Count],
            Random.Range(0f, 2f),
            Random.Range(0f, 2f),
            Random.Range(0f, 2f)
            );

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
            _inRoomUnits.Add(newBaseUnit);
            newUnit.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
        }
    }

    // 雇用処理
    public void Employment(BaseUnit baseUnit)
    {
        _baseUnits.Add(baseUnit);// 雇用
        if (_selectUnit == null)// もしユニットが選択されていない場合
        {
            _selectUnit = _baseUnits[0];// ユニットのセット
            _hiredListManager.HiredListUpdate();// 更新
        }
        baseUnit.ResumeInterface.Close();
        _inRoomUnits.Remove(baseUnit);// 部屋から退出
        baseUnit.ResumeInterface.gameObject.SetActive(false);// 非表示に変更
        baseUnit.gameObject.SetActive(false);// とりあえず雇ったら非表示

        // ※とりあえず瞬間雇用
        _unitContainer.Hired(baseUnit);
    }
    public void Reject(BaseUnit baseUnit)
    {
        _inRoomUnits.Remove(baseUnit);// 部屋から退出
        Destroy(baseUnit.gameObject);// Unitの削除
    }
    // 呼び出し処理
    public void Call()
    {
        foreach (var item in _inRoomUnits)
        {
            if(item.Origin == _selectUnit.Origin)
            {
                // すでに呼び出されている
                Debug.Log("元の作業場に戻りなさい。");
                _unitContainer.CallBack(_selectUnit);
                _inRoomUnits.Remove(_selectUnit);
                _selectUnit.gameObject.SetActive(false);
                return;
            }
        }
        
        Debug.Log("今すぐ執務室に来なさい。");
        // 入室制限
        if (_inRoomUnits.Count >= 3)
        {
            return;
        }

        // ※瞬間的に呼び出される
        _unitContainer.Call(_selectUnit);
        _inRoomUnits.Add(_selectUnit);
        _selectUnit.gameObject.SetActive(true);
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
