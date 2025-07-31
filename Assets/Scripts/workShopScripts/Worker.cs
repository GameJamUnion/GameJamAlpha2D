using UnityEngine;

/// <summary>
/// 作業員クラス
/// </summary>
public class Worker : ObjBase
{
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
    /// 作業員ID
    /// </summary>
    private int originId;

    /// <summary>
    /// 作業員のステータス
    /// </summary>
    private WokerStatus wokerStatus;

    /// <summary>
    /// 体力
    /// </summary>
    private float hitPoint = 100.0f; // TODO とりあえず100

    /// <summary>
    /// 配属している作業場のID
    /// </summary>
    private RI.PlacementState assignWorkId;

    /// <summary>
    /// 作業員の状態
    /// </summary>
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
        get { return wokerStatus; }
        set { wokerStatus = value; }
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

            
        }
        else if (workerState == WorkCommon.WorkerState.BREAK)
        {
            recoveryHitPoint();

            if (hitPoint >= 100.0f)
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
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <param name="workId"></param>
    /// <returns></returns>
    public float getWorkPower(RI.PlacementState workId)
    {
        return wokerStatus.getWorkPower(workId);
    }

    /// <summary>
    /// ヒットポイントを削る
    /// </summary>
    private void takeDamage()
    {
        float damage = baseDamage * wokerStatus.Physical;
        float rate = Random.Range(damageVariationRate * (-1), damageVariationRate);
        damage = damage * rate;

        hitPoint -= damage;

        if (hitPoint < 0.0f) hitPoint = 0.0f;
    }

    /// <summary>
    /// 体力を回復する
    /// </summary>
    private void recoveryHitPoint()
    {
        hitPoint += 15.0f;

        if (hitPoint > 100.0f) hitPoint = 100.0f;
    }
}
