using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  作業場シーンメイン処理
/// </summary>
public class WorkShopSceneMain : MonoBehaviour
    , IGameWaveChangeReceiver
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
    /// ユニット配置の親オブジェクト
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
        GameWaveManager.Instance.registerReceiver(this);
        unitContainer.RegisterEventOnHired(EmployWorker);
        unitContainer.RegisterEventOnRemove(RemoveWorker);
        unitContainer.RegisterEventOnCall(CallWorker);
        unitContainer.RegisterEventOnCallBack(CallBackWorker);
        unitContainer.RegisterEventOnMoveSectionByWorker(MoveSectionByWorker);
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
            // 座標を取得する
            Transform transform = work.GetAvailableUnitPlacement(unit.Origin);

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
            var pos = new Vector3(-72f, -12f, 2.0f);// 画面外開始

            Worker worker = Instantiate<Worker>(workerPrefab, pos, transform.rotation, unitRootTrans);
            worker.InitializeWoker(unit.Origin, unit.PlacementState, status);
            worker.setMoveTarget(transform.position);
            workManager.EmployWoker(worker);
        }
    }

    /// <summary>
    /// 作業員を解雇する
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveWorker(BaseUnit unit)
    {
        workManager.RemoveWorker(unit.Origin, unit.PlacementState);
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

    /// <summary>
    /// 作業場変更イベント
    /// </summary>
    /// <param name="originId"></param>
    /// <param name="beforePlacementState"></param>
    /// <param name="afterPlacementState"></param>
    public void MoveSectionByWorker(int originId, RI.PlacementState beforePlacementState, RI.PlacementState afterPlacementState)
    {
        WorkBase beforeWork = workManager.GetWork(beforePlacementState);
        WorkBase afterWork = workManager.GetWork(afterPlacementState);
        Worker worker = workManager.GetWorker(originId);

        if (beforeWork != null && afterWork != null && worker != null)
        {
            beforeWork.RemoveUnit(originId);
            Transform transform = afterWork.GetAvailableUnitPlacement(originId);
            worker.setMoveTarget(transform.position);
        }
    }

    public void receiveChangeWave(GameWaveChangeReceiveParam param)
    {
        score = param.WorkScore;
    }
}
