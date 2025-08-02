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
    [SerializeField] CustomSimpleAutoRandomStatus _customSimpleAutoRandomStatus;
    [SerializeField] CAPointTable _caPointTable;
    [SerializeField] CADistributionTable _caDistributionTable;

    [Header("Config")]
    [SerializeField] private int _maxUnhearCount = 10;
    private int _currentUnhearCount = 0;

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
        newUnit.SetActive(false);

        BaseUnit newBaseUnit = newUnit.GetComponent<BaseUnit>();

        switch (_tableSwitch)
        {
            case TableSwitch.FullCustom:
                newBaseUnit.SetState(CreateFullCustomRamdomStatus());
                break;

            case TableSwitch.SimpleAuto:
                newBaseUnit.SetState(CreateSimpleAutoRandomStatus());
                break;

            case TableSwitch.ComplexAuto:
                newBaseUnit.SetState(CreateComplexAutoRandomStatus());
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
            productionEfficiencys.ProductionEfficiency3,
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f));

        return newBaseUnit;
    }

    /// <summary>
    /// シンプル版自動ランダムステータス生成
    /// </summary>
    /// <returns>ユニットデータ</returns>
    private BaseUnit CreateSimpleAutoRandomStatus()
    {
        BaseUnit newBaseUnit = new BaseUnit();

        int sumWeight = 0;
        foreach (var data in _customSimpleAutoRandomStatus.Datas)
        {
            sumWeight += data.Weight;
        }

        // ProductionEfficiency1
        // 区間の取り出し
        SimpleAutoRandomStatusSectionData useData = ExtractingSection(sumWeight);
        // 値の取り出し
        float s1 = Random.Range(useData.Max, useData.Min);

        // ProductionEfficiency2
        useData = ExtractingSection(sumWeight);
        float s2 = Random.Range(useData.Max, useData.Min);

        // ProductionEfficiency3
        useData = ExtractingSection(sumWeight);
        float s3 = Random.Range(useData.Max, useData.Min);

        newBaseUnit.SetState(
            _nameTable.Names[Random.Range(0, 20000) % _nameTable.Names.Count],
            s1,
            s2,
            s3,
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f));

        return newBaseUnit;
    }
    private SimpleAutoRandomStatusSectionData ExtractingSection(int sumWeigh)
    {
        // 区間の指定
        int random = Random.Range(0, sumWeigh);
        int currentRandom = random;
        SimpleAutoRandomStatusSectionData useData = null;
        // 区間の取り出し
        for (int i = 0; i < _customSimpleAutoRandomStatus.Datas.Count; i++)
        {
            currentRandom -= _customSimpleAutoRandomStatus.Datas[i].Weight;
            if (currentRandom <= 0)
            {
                useData = _customSimpleAutoRandomStatus.Datas[i];
                break;
            }
        }

        if (useData == null)
            return null;

        return useData;
    }

    /// <summary>
    /// 複雑版自動ランダムステータス生成
    /// </summary>
    /// <returns>ユニットデータ</returns>
    private BaseUnit CreateComplexAutoRandomStatus()
    {
        BaseUnit newBaseUnit = new BaseUnit();

        float p1, p2, p3;
        float s1, s2, s3;

        int sumWeightCAD = 0;
        int sumWeightCAP = 0;

        foreach (var data in _caDistributionTable.Datas)
        {
            sumWeightCAD += data.Weight;
        }
        foreach (var data in _caPointTable.Datas)
        {
            sumWeightCAP += data.Weight;
        }

        DistributionTableData useDTD = null;
        PointTableData usePTD = null;

        // p1
        useDTD = ExtractingSectionCAD(sumWeightCAD);

        if (useDTD == null)
            return null;

        p1 = Random.Range(useDTD.Min, useDTD.Max);

        // p2
        useDTD = ExtractingSectionCAD(sumWeightCAD);

        if (useDTD == null)
            return null;

        p2 = (1f - p1)/*残数*/ * Random.Range(useDTD.Min, useDTD.Max);

        // p3
        p3 = 1 - p1 - p2;

        /* p1,p2,p3 ポイント配布比率の計算終了 */

        usePTD = ExtractingSectionCAP(sumWeightCAP);
        float point = Random.Range(usePTD.Min, usePTD.Max);

        Debug.Log("Point : " + point);

        // s1
        if (usePTD == null)
            return null;

        // とりあえずマジックナンバーは随時変えつつで... (現在の想定 -1～1)
        s1 = p1 * point - 1f;
        s2 = p2 * point - 1f;
        s3 = p3 * point - 1f;
        newBaseUnit.SetState(_nameTable.Names[Random.Range(0, 20000) % _nameTable.Names.Count], s1, s2, s3, Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));

        return newBaseUnit;
    }
    private PointTableData ExtractingSectionCAP(int sumWeigh)
    {
        // 区間の指定
        int random = Random.Range(0, sumWeigh);
        int currentRandom = random;
        PointTableData useData = null;
        // 区間の取り出し
        for (int i = 0; i < _caPointTable.Datas.Count; i++)
        {
            currentRandom -= _caPointTable.Datas[i].Weight;
            if (currentRandom <= 0)
            {
                useData = _caPointTable.Datas[i];
                break;
            }
        }

        if (useData == null)
            return null;

        return useData;
    }
    private DistributionTableData ExtractingSectionCAD(int sumWeigh)
    {
        // 区間の指定
        int random = Random.Range(0, sumWeigh);
        int currentRandom = random;
        DistributionTableData useData = null;
        // 区間の取り出し
        for (int i = 0; i < _caDistributionTable.Datas.Count; i++)
        {
            currentRandom -= _caDistributionTable.Datas[i].Weight;
            if (currentRandom <= 0)
            {
                useData = _caDistributionTable.Datas[i];
                break;
            }
        }

        if (useData == null)
            return null;

        return useData;
    }

    #endregion RandomStatus

    // 呼び込む
    public void ComeInUnit()
    {
        SoundManager.Instance.requestPlaySound(SEKind.Bell);

        // 雇用のために部屋に呼ぶ限界上限３体
        if (_inRoomUnits.Count >= 1)
            return;

        if (_reserveUnits.Count == 0)
            return;

        BaseUnit newBaseUnit = _reserveUnits[0];

        // 聴力によって聞こえない確率
        if (_reserveUnits[0].HearingPower < 0)// 聴力が負の時聞こえない可能性がある
        {
            float unhear = Random.Range(0f, 1f);
            if (_reserveUnits[0].HearingPower < -0.8f)
            {
                if (unhear <= 0.95f)// 95%で聞こえない
                {
                    Debug.Log("Hear : " + unhear + " /// 95%");
                    _currentUnhearCount++;
                    if (_currentUnhearCount < _maxUnhearCount)
                        return;
                }
            }
            else if (_reserveUnits[0].HearingPower < -0.6f)
            {
                if (unhear <= 0.85f)// 85%で聞こえない
                {
                    Debug.Log("Hear : " + unhear + " /// 90%");
                    _currentUnhearCount++;
                    if (_currentUnhearCount < _maxUnhearCount)
                        return;
                }
            }
            else if (_reserveUnits[0].HearingPower < -0.4f)
            {
                if (unhear <= 0.7f)// 70%で聞こえない
                {
                    Debug.Log("Hear : " + unhear + " /// 85%");
                    _currentUnhearCount++;
                    if (_currentUnhearCount < _maxUnhearCount)
                        return;
                }
            }
            else if (_reserveUnits[0].HearingPower < -0.2f)
            {
                if (unhear <= 0.6f)// 60%で聞こえない
                {
                    Debug.Log("Hear : " + unhear + " /// 70%");
                    _currentUnhearCount++;
                    if (_currentUnhearCount < _maxUnhearCount)
                        return;
                }
            }
            Debug.Log("Hear : " + unhear);
        }

        _currentUnhearCount = 0;

        _reserveUnits.Remove(newBaseUnit);
        CreateUnit();

        newBaseUnit.gameObject.SetActive(true);

        if (_resumeInterfaceManager.AssignInactiveResumeInterface)
        {
            newBaseUnit.ResumeInterface = _resumeInterfaceManager.GetInactiveResumeInterface();
            newBaseUnit.ResumeInterface.gameObject.GetComponent<RectTransform>().transform.localPosition = new Vector3(970f + 10f * ((float)_resumeInterfaceManager.GetInactiveResumeInterfaceCount() - 1f), 10f * ((float)_resumeInterfaceManager.GetInactiveResumeInterfaceCount() - 1), 0f);
            newBaseUnit.Initialize(_nextID, this);
            _nextID++;
            _inRoomUnits.Add(newBaseUnit);
            newBaseUnit.gameObject.GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
            _unitAbilityCanvasUI.OnDisplay(newBaseUnit.IntervieweeUnitAbility);
        }

        SoundManager.Instance.requestPlaySound(SEKind.DoorOpen);
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

        SoundManager.Instance.requestPlaySound(SEKind.DoorClose);
    }
    public void Reject(BaseUnit baseUnit)
    {
        _unitAbilityCanvasUI.OnHide();
        _inRoomUnits.Remove(baseUnit);// 部屋から退出
        Destroy(baseUnit.gameObject);// Unitの削除
        SoundManager.Instance.requestPlaySound(SEKind.DoorClose);
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
                SoundManager.Instance.requestPlaySound(SEKind.DoorClose);
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

        SoundManager.Instance.requestPlaySound(SEKind.DoorOpen);
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

        SoundManager.Instance.requestPlaySound(SEKind.Paper);
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

        SoundManager.Instance.requestPlaySound(SEKind.Paper);
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
        SimpleAuto,
        ComplexAuto
    }
}
