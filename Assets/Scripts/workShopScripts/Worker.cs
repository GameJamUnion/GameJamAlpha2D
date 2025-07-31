using System;
using UnityEngine;

/// <summary>
/// 作業員クラス
/// </summary>
public class Worker : ObjBase
{
    /// <summary>
    /// ユニットコンテナ
    /// </summary>
    [SerializeField]
    private UnitContainer unitContainer;

    /// <summary>
    /// ベース疲労値
    /// </summary>
    [SerializeField]
    private float baseDamage;

    /// <summary>
    /// 疲労値の幅(割合)
    /// </summary>
    [SerializeField]
    private float damageVariationRate;

    /// <summary>
    /// ベース疲労回復値
    /// </summary>
    [SerializeField]
    private float baseRecovery;

    /// <summary>
    /// 最大体力
    /// </summary>
    [SerializeField]
    private float maxHitPoint;

    /// <summary>
    /// トラブルイベント頻度
    /// </summary>
    [SerializeField]
    private int troubleInterval;

    /// <summary>
    /// トラブルイベント発火カウント
    /// </summary>
    private int troubleCount;

    /// <summary>
    /// 作業員ID
    /// </summary>
    private int originId;

    /// <summary>
    /// 作業員のステータス
    /// </summary>
    private WokerStatus workerStatus;

    /// <summary>
    /// 体力
    /// </summary>
    [SerializeField]
    private float hitPoint;

    /// <summary>
    /// 配属している作業場のID
    /// </summary>
    [SerializeField]
    private RI.PlacementState assignWorkId;

    /// <summary>
    /// 作業員の状態
    /// </summary>
    [SerializeField]
    private WorkCommon.WorkerState workerState;

    #region Property
    /// <summary>
    /// 作業員ID
    /// </summary>
    public int OriginId
    {
        get { return originId; }
        set { originId = value; }
    }

    /// <summary>
    /// 作業員のステータス
    /// </summary>
    public WokerStatus WokerStatus
    {
        get { return workerStatus; }
        set { workerStatus = value; }
    }

    /// <summary>
    /// 配属している作業場のID
    /// </summary>
    public RI.PlacementState AssingWorkId
    {
        get { return assignWorkId; }
        set { assignWorkId = value; }
    }
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void workPerSeconds()
    {
        if (workerState == WorkCommon.WorkerState.WORKING)
        {
            takeDamage();

            if (hitPoint <= 0.0f)
            {
                workerState = WorkCommon.WorkerState.BREAK;
            }

            troubleCount++;
            if (troubleCount > troubleInterval)
            {
                troubleCount = 0;
                // 嘘つきイベントが発生しなかった場合のみ邪魔イベント判定を実行
                if (!lyingEvent())
                {
                    obstacleEvent();
                }
            }
        }
        else if (workerState == WorkCommon.WorkerState.BREAK)
        {
            recoveryHitPoint();

            if (hitPoint >= maxHitPoint)
            {
                workerState = WorkCommon.WorkerState.WORKING;
            }
        }
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void workPerFlame()
    {
        
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="originId"></param>
    /// <param name="assignWorkId"></param>
    /// <param name="wokerStatus"></param>
    public void initializeWoker(int originId, RI.PlacementState assignWorkId, WokerStatus wokerStatus)
    {
        OriginId = originId;
        AssingWorkId = assignWorkId;
        WokerStatus = wokerStatus;
        hitPoint = maxHitPoint;
        baseDamage = maxHitPoint * 0.05f;
        troubleCount = 0;
        workerState = WorkCommon.WorkerState.WORKING;
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <param name="workId"></param>
    /// <returns></returns>
    public float getWorkPower(RI.PlacementState workId)
    {
        return workerStatus.getWorkPower(workId);
    }

    /// <summary>
    /// ヒットポイントを削る
    /// </summary>
    private void takeDamage()
    {
        float damage = baseDamage - (workerStatus.Physical * baseDamage);
        float rate = UnityEngine.Random.Range(damageVariationRate * (-1), damageVariationRate);
        damage = damage + (damage * rate);

        hitPoint = hitPoint - damage;

        if (hitPoint < 0.0f) hitPoint = 0.0f;
    }

    /// <summary>
    /// 体力を回復する
    /// </summary>
    private void recoveryHitPoint()
    {
        hitPoint += baseRecovery;

        if (hitPoint > maxHitPoint) hitPoint = maxHitPoint;
    }

    /// <summary>
    /// 邪魔イベント
    /// </summary>
    private bool obstacleEvent()
    {
        if (workerStatus.ObstaclePower <= 0) return false;

        float obstacleValue = workerStatus.ObstaclePower * 100;

        if (UnityEngine.Random.Range(0, 100) < obstacleValue)
        {
            // 勝手に作業場を移動する
            moveWorkSection();
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 嘘つきイベント
    /// </summary>
    private bool lyingEvent()
    {
        if (workerStatus.LyingPower <= 0) return false;

        float lyingValue = workerStatus.LyingPower * 100;

        if (UnityEngine.Random.Range(0, 100) < lyingValue)
        {
            // 強制で状態を休憩にする
            this.workerState = WorkCommon.WorkerState.BREAK;
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 作業場を勝手に移動する
    /// </summary>
    private void moveWorkSection()
    {
        assignWorkId = getRondomWorkSection(assignWorkId);
        unitContainer.MoveSectionByWorker(originId, assignWorkId);
    }

    /// <summary>
    /// 指定した作業場以外の作業場を取得する
    /// </summary>
    /// <param name="placementState"></param>
    /// <returns></returns>
    private RI.PlacementState getRondomWorkSection(RI.PlacementState placementState)
    {
        RI.PlacementState movePlace = RI.PlacementState.NONE;
        int count = Enum.GetNames(typeof(RI.PlacementState)).Length;

        do
        {
            int num = UnityEngine.Random.Range(1, count-1);
            movePlace = (RI.PlacementState)Enum.ToObject(typeof(RI.PlacementState), num);
        }
        while (placementState == movePlace
        || placementState == RI.PlacementState.NONE
        || placementState == RI.PlacementState.END);

        return movePlace;
    }
}
