using OfficeGameMasterDebug;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OfficeGameMaster : MonoBehaviour
{
    [Header("アタッチ")]
    [SerializeField] GameObject _createUnitPref;
    [SerializeField] GameObject _parentUnits;
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;
    [SerializeField] HiredListManager _hiredListManager;

    [Header("生成")]
    [SerializeField] List<BaseUnit> _baseUnits;// 雇用中のユニット
    //[SerializeField] List<BaseUnit> _reserveUnits;
    [SerializeField] List<BaseUnit> _inRoomUnits;// 部屋の中にいるユニット
    [SerializeField] List<BaseUnit> _reserveUnits;// 生成だけされているユニット

    [SerializeField] int _maxReserveUnits = 6;

    [Header("選択")]
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

        if (newBaseUnit == null)// nullチェック
        {
            Destroy(newUnit);// ユニットの消去
            Debug.Log("#Elalice : OfficeGameMaster > CreateUnit // Error");
            return;
        }
        newBaseUnit.IntervieweeUnitAbility = _abilityTable.CreateIntervieweeUnitAbility();
        newBaseUnit.transform.parent = _parentUnits.transform;

        // リザーブに入れておく
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
    /// 複雑版ランダムステータス生成
    /// </summary>
    /// <returns></returns>
    private BaseUnit CreateSimpleAutoRandomStatus()
    {

        return null;
    }

    /// <summary>
    /// 複雑版ランダムステータス生成
    /// </summary>
    /// <returns></returns>
    private BaseUnit CreateComplexAutoRandomStatus()
    {
        return null;
    }

    #endregion RandomStatus

    // 呼び込む
    public void ComeInUnit()
    {
        // 雇用のために部屋に呼ぶ限界上限３体
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
        _unitAbilityCanvasUI.OnHide();

        // ※とりあえず瞬間雇用
        _unitContainer.Hired(baseUnit);
    }
    public void Reject(BaseUnit baseUnit)
    {
        _unitAbilityCanvasUI.OnHide();
        _inRoomUnits.Remove(baseUnit);// 部屋から退出
        Destroy(baseUnit.gameObject);// Unitの削除
    }
    // 呼び出し処理
    public void Call()
    {
        if (_baseUnits.Count <= 0)
            return;

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
    // 解雇
    public void Fire()
    {
        // ユニットが存在しない場合終了
        if (_baseUnits.Count == 0)
            return;

        // 部屋にいない場合終了
        if (!_inRoomUnits.Contains(_selectUnit))
            return;

        // 次のユニットに渡る処理
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
            // 抜いた状態の雇用リストに変更
            _selectUnit = _baseUnits[no];
            // 更新
            _hiredListManager.HiredListUpdate();
        }
        else
        {
            // 何も表示されていない状態に変更
            _hiredListManager.SetDefault();
        }
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

namespace OfficeGameMasterDebug
{
    public enum TableSwitch
    {
        FullCustom,
    }
}
