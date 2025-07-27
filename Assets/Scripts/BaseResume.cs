using UnityEngine;

// ユニットが持ている
public class BaseResume : MonoBehaviour
{
    [SerializeField] BaseUnit _baseUnit;
    [SerializeField] OfficeGameMaster _officeGameMaster;

    [SerializeField] string _name = "***NoData***";

    [SerializeField] string _productionEfficiency1 = "---";
    [SerializeField] string _productionEfficiency2 = "---";
    [SerializeField] string _productionEfficiency3 = "---";

    [SerializeField] bool _onStamp = false;
    [SerializeField] float _stampRotaion = 0f;

    #region プロパティ
    public BaseUnit BaseUnit
    {
        get { return _baseUnit; }
    }
    public string Name
    {
        get { return _name; }
    }
    public string ProductionEfficiency1
    {
        get { return _productionEfficiency1; }
    }
    public string ProductionEfficiency2
    {
        get { return _productionEfficiency2; }
    }
    public string ProductionEfficiency3
    {
        get { return _productionEfficiency3; }
    }
    public bool OnStamp
    {
        get { return _onStamp; }
        set { _onStamp = value; }
    }
    public float StampRotation
    {
        get { return _stampRotaion; }
    }
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // レジュメ作成
    public void SetResume(BaseUnit baseUnit)
    {
        _baseUnit = baseUnit;

        _name = baseUnit.Name;

        _productionEfficiency1 = baseUnit.GetProductionEfficiencyRank(ProductionEfficiency.SECTION_1);
        _productionEfficiency2 = baseUnit.GetProductionEfficiencyRank(ProductionEfficiency.SECTION_2);
        _productionEfficiency3 = baseUnit.GetProductionEfficiencyRank(ProductionEfficiency.SECTION_3);
    }
    public void SetOfficeGameMaster(OfficeGameMaster officeGameMaster)
    {
        _officeGameMaster = officeGameMaster;
    }
    public void OnStampProcess()
    {
        // 採用スタンプを押した際に発生する処理
        if (!_onStamp)
        {
            _onStamp = true;
            _stampRotaion = Random.Range(-10f, 10f);
        }
    }
}
