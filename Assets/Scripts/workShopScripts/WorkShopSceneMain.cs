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
    /// 
    /// </summary>
    [SerializeField]
    private UnitContainer unitContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        //unitContainer.RegisterEventOnHired(employWorker);
        //unitContainer.RegisterEventOnRemove(removeWorker);
        //unitContainer.RegisterEventOnCall(callWorker);
    }

    /// <summary>
    /// 作業員を雇用する
    /// </summary>
    public void employWorker(RI.PlacementState placementState, int originId, int workPower)
    {
        WorkBase work = workManager.GetWork(placementState);

        if (work != null)
        {
            // 座標を設定
            // TODO とりあえず作業場の隣に並べてる
            Vector3 pos = work.transform.position;
            pos.x += 25 + (work.workerCount() * 10);
            Quaternion rot = Quaternion.identity;

            // 作業員を複製
            Worker worker = Instantiate<Worker>(workerPrefab, pos, rot, unitRootTrans);
            worker.OriginId = originId;
            worker.WorkPower = workPower;

            work.addWorker(worker);
        }
    }

    /// <summary>
    /// 作業員を解雇する
    /// </summary>
    /// <param name="originId"></param>
    public void removeWorker(int originId)
    {
        workManager.removeWorker(originId);
    }

    /// <summary>
    /// 作業員を呼び出す
    /// </summary>
    /// <param name="originId"></param>
    public void callWorker(int originId)
    {
        removeWorker(originId);
    }

    /// <summary>
    /// 作業員が呼び出しから戻ってくる
    /// </summary>
    public void callBackWorker(RI.PlacementState placementState, int originId, int workPower)
    {
        employWorker(placementState, originId, workPower);
    }


    [ContextMenu("section1")]
    public void test1()
    {
        employWorker(RI.PlacementState.Section1, 1, 50);
    }

    [ContextMenu("section2")]
    public void test2()
    {
        employWorker(RI.PlacementState.Section2, 2, 50);
    }

    [ContextMenu("section3")]
    public void test3()
    {
        employWorker(RI.PlacementState.Section3, 3, 50);
    }

    [ContextMenu("remove1")]
    public void removeTest1()
    {
        removeWorker(1);
    }

    [ContextMenu("remove2")]
    public void removeTest()
    {
        removeWorker(2);
    }

    [ContextMenu("remove3")]
    public void removeTest3()
    {
        removeWorker(3);
    }
}
