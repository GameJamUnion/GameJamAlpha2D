using System.Collections.Generic;
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
    /// ユニットコンテナ
    /// </summary>
    [SerializeField]
    private UnitContainer unitContainer;

    /// <summary>
    /// スコア
    /// </summary>
    [SerializeField]
    private WorkScore score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        unitContainer.RegisterEventOnHired(EmployWorker);
        unitContainer.RegisterEventOnRemove(RemoveWorker);
        unitContainer.RegisterEventOnCall(CallWorker);
        unitContainer.RegisterEventOnCallBack(CallBackWorker);
        unitContainer.RegisterEventInterfereOn(InterfereWorker);
        unitContainer.RegisterEventInterfereOff(StopInterfereWorker);
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (CheckDebugInput())
        {
            score.AddScore(1);
        }
        #endif
    }

    /// <summary>
    /// Pボタンをチェック
    /// </summary>
    /// <returns></returns>
    private bool CheckDebugInput()
    {
        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 作業員を雇用する
    /// </summary>
    /// <param name="unit"></param>
    public void EmployWorker(BaseUnit unit)
    {
        if (workManager.GetWorker(unit.Origin) != null)
        {
            Debug.LogWarning("雇用するやつの作業員IDもうおるから雇用キャンセルしたぞ");
            return;
        }

        WorkBase work = workManager.GetWork(unit.PlacementState);

        if (work != null)
        {
            // 座標を設定
            // TODO とりあえず作業場の隣に並べてる
            Vector3 pos = work.transform.position;
            pos.x += 25 + (workManager.GetWokerList(unit.PlacementState).Count * 10);
            Quaternion rot = Quaternion.identity;

            Dictionary<RI.PlacementState, float> powerDictionary = new Dictionary<RI.PlacementState, float>
            {
                { RI.PlacementState.Section1, unit.ProductionEfficiency1 },
                { RI.PlacementState.Section2, unit.ProductionEfficiency2 },
                { RI.PlacementState.Section3, unit.ProductionEfficiency3 }
            };

            // TODO 渡す値は調整
            float physical = 0.0f;
            float listeningPower = 0.0f;
            float lyingPower = 0.5f;
            float obstaclePower = 0.5f;
            float speed = 1.0f;

            WokerStatus status = new WokerStatus(powerDictionary,
                physical,
                listeningPower,
                lyingPower,
                obstaclePower,
                speed);

            // 作業員を複製
            Worker worker = Instantiate<Worker>(workerPrefab, pos, rot, unitRootTrans);
            worker.InitializeWoker(unit.Origin, unit.PlacementState, status);

            workManager.EmployWoker(worker);
        }
    }

    /// <summary>
    /// 作業員を解雇する
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveWorker(BaseUnit unit)
    {
        workManager.RemoveWorker(unit.Origin);
    }

    /// <summary>
    /// 作業員を呼び出す
    /// </summary>
    /// <param name="unit"></param>
    public void CallWorker(BaseUnit unit)
    {
        RemoveWorker(unit);
    }

    /// <summary>
    /// 作業員が呼び出しから戻ってくる
    /// </summary>
    /// <param name="unit"></param>
    public void CallBackWorker(BaseUnit unit)
    {
        EmployWorker(unit);
    }

    /// <summary>
    /// 妨害する
    /// </summary>
    /// <param name="interfereOriginId"></param>
    public void InterfereWorker(int interfereOriginId)
    {
        workManager.InterfereWorker(interfereOriginId);
    }

    /// <summary>
    /// 妨害を終了する
    /// </summary>
    /// <param name="interfereOriginId"></param>
    /// <param name="beInterferedOriginID"></param>
    public void StopInterfereWorker(int interfereOriginId, int beInterferedOriginID)
    {
        workManager.StopInterfereWorker(interfereOriginId, beInterferedOriginID);
    }
}
