using UnityEngine;

/// <summary>
///  作業場シーンメイン処理
/// </summary>
public class WorkShopSceneMain : MonoBehaviour
{
    /// <summary>
    /// 作業員プレハブ
    /// </summary>
    [SerializeField]
    private Worker workerPrefab;

    /// <summary>
    /// 作業場の管理クラス
    /// </summary>
    [SerializeField]
    private WorkManager workManager;

    /// <summary>
    /// シーンの親オブジェクト
    /// </summary>
    [SerializeField]
    private Transform unitRootTrans;

    /// <summary>
    /// ユニット
    /// </summary>
    [SerializeField]
    private UnitContainer unitContainer;

    /// <summary>
    /// 作業力倍率
    /// </summary>
    [SerializeField]
    private float workPowerRate;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        unitContainer.RegisterEventOnHired(employWorker);
        unitContainer.RegisterEventOnRemove(removeWorker);
        unitContainer.RegisterEventOnCall(callWorker);
        unitContainer.RegisterEventOnCallBack(callBackWorker);
    }

    /// <summary>
    /// 作業員を雇用する
    /// </summary>
    /// <param name="unit"></param>
    public void employWorker(BaseUnit unit)
    {
        WorkBase work = workManager.GetWork(unit.PlacementState);

        if (work != null)
        {
            // 座標を設定
            // TODO とりあえず作業場の隣に並べてる
            Vector3 pos = work.transform.position;
            pos.x += 25 + (work.workerCount() * 10);
            Quaternion rot = Quaternion.identity;

            

            // 作業員を複製
            Worker worker = Instantiate<Worker>(workerPrefab, pos, rot, unitRootTrans);
            worker.OriginId = unit.Origin;
            worker.WorkPower = getWorkPower(unit, unit.PlacementState);

            work.addWorker(worker);
        }
    }

    /// <summary>
    /// 作業員を解雇する
    /// </summary>
    /// <param name="unit"></param>
    public void removeWorker(BaseUnit unit)
    {
        workManager.removeWorker(unit.Origin);
    }

    /// <summary>
    /// 作業員を呼び出す
    /// </summary>
    /// <param name="unit"></param>
    public void callWorker(BaseUnit unit)
    {
        removeWorker(unit);
    }

    /// <summary>
    /// 作業員が呼び出しから戻ってくる
    /// </summary>
    /// <param name="unit"></param>
    public void callBackWorker(BaseUnit unit)
    {
        employWorker(unit);
    }

    /// <summary>
    /// 作業力を取得する
    /// TODO 定義と取得の方法は考える必要あり
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="placementState"></param>
    /// <returns></returns>
    private float getWorkPower(BaseUnit unit, RI.PlacementState placementState)
    {
        float workPower = 0.0f;

        switch (placementState)
        {
            case RI.PlacementState.NONE:
                // 初期値でそのまま
                break;

            case RI.PlacementState.Section1:
                workPower = unit.ProductionEfficiency1;
                break;

            case RI.PlacementState.Section2:
                workPower = unit.ProductionEfficiency2;
                break;

            case RI.PlacementState.Section3:
                workPower = unit.ProductionEfficiency3;
                break;

            case RI.PlacementState.END:
                // 初期値でそのまま
                break;

        }

        if (workPower < 0.0f)
        {
            workPower = 0.0f;
        }

        workPower = workPower * workPowerRate;

        return workPower;
    }
}
