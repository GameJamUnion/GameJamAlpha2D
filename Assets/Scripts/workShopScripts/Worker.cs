using System;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// 作業員クラス
/// </summary>
public class Worker : ObjBase
{
    /// <summary>
    /// アニメーションベース
    /// </summary>
    private CharacterBaseBehavior charaAnimeBase;

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

    /// <summary>
    /// 自分が妨害している作業員のID
    /// </summary>
    private int interfereOriginId;

    /// <summary>
    /// 妨害する時間
    /// </summary>
    [SerializeField]
    private int interfereTime;

    /// <summary>
    /// 妨害カウント
    /// </summary>
    private int interfereCount;

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

    /// <summary>
    /// 作業員の状態
    /// </summary>
    public WorkCommon.WorkerState WorkerState
    {
        get { return workerState; }
        set { workerState = value; }
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
    protected override void WorkPerSeconds()
    {
        if (workerState == WorkCommon.WorkerState.WORKING)
        {
            TakeDamage();

            if (hitPoint <= 0.0f)
            {
                SetState(WorkCommon.WorkerState.BREAK);
            }

            troubleCount++;
            if (troubleCount > troubleInterval)
            {
                troubleCount = 0;
                // 嘘つきイベントが発生しなかった場合のみ邪魔イベント判定を実行
                if (!LyingEvent())
                {
                    ObstacleEvent();
                }
            }
        }
        else if (workerState == WorkCommon.WorkerState.BREAK)
        {
            RecoveryHitPoint();

            if (hitPoint >= maxHitPoint)
            {
                SetState(WorkCommon.WorkerState.WORKING);
            }
        }
        else if (workerState == WorkCommon.WorkerState.INTERFERE)
        {
            interfereCount++;

            if (interfereCount >= interfereTime)
            {
                InterfereTriggerOff();
            }
        }
        else if (workerState == WorkCommon.WorkerState.BE_INTERFERED)
        {
            // 妨害されているときは何もしない
        }
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void WorkPerFlame()
    {
        
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="originId"></param>
    /// <param name="assignWorkId"></param>
    /// <param name="wokerStatus"></param>
    public void InitializeWoker(int originId, RI.PlacementState assignWorkId, WokerStatus wokerStatus)
    {
        OriginId = originId;
        AssingWorkId = assignWorkId;
        WokerStatus = wokerStatus;
        hitPoint = maxHitPoint;
        baseDamage = maxHitPoint * 0.05f;
        troubleCount = 0;
        charaAnimeBase = this.GetComponentInChildren<CharacterBaseBehavior>();
        SetState(WorkCommon.WorkerState.WORKING);
        interfereOriginId = 0;
        interfereCount = 0;
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <param name="workId"></param>
    /// <returns></returns>
    public float GetWorkPower(RI.PlacementState workId)
    {
        return workerStatus.GetWorkPower(workId);
    }

    /// <summary>
    /// ヒットポイントを削る
    /// </summary>
    private void TakeDamage()
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
    private void RecoveryHitPoint()
    {
        hitPoint += baseRecovery;

        if (hitPoint > maxHitPoint) hitPoint = maxHitPoint;
    }

    /// <summary>
    /// 邪魔イベント
    /// </summary>
    private bool ObstacleEvent()
    {
        if (workerStatus.ObstaclePower <= 0) return false;

        float obstacleValue = workerStatus.ObstaclePower * 100;

        if (UnityEngine.Random.Range(0, 100) < obstacleValue)
        {
            int rand = UnityEngine.Random.Range(0, 2);

            if (rand == 0)
            {
                // 勝手に作業場を移動する
                MoveWorkSection();
            }
            else
            {
                // 妨害をする
                InterfereTriggerOn();
            }
            
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
    private bool LyingEvent()
    {
        if (workerStatus.LyingPower <= 0) return false;

        float lyingValue = workerStatus.LyingPower * 100;

        if (UnityEngine.Random.Range(0, 100) < lyingValue)
        {
            // 強制で状態を休憩にする
            SetState(WorkCommon.WorkerState.BREAK);
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
    private void MoveWorkSection()
    {
        assignWorkId = GetRondomWorkSection(assignWorkId);

        // 左画面には知らせない
        //unitContainer.MoveSectionByWorker(originId, assignWorkId);
    }

    /// <summary>
    /// 指定した作業場以外の作業場を取得する
    /// </summary>
    /// <param name="placementState"></param>
    /// <returns></returns>
    private RI.PlacementState GetRondomWorkSection(RI.PlacementState placementState)
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

    /// <summary>
    /// 作業員の状態を設定する
    /// </summary>
    /// <param name="state"></param>
    private void SetState(WorkCommon.WorkerState state)
    {
        switch (state)
        {
            case WorkCommon.WorkerState.WORKING:
                workerState = WorkCommon.WorkerState.WORKING;
                charaAnimeBase.removeStatus(CharacterUnitStatus.Rest);
                charaAnimeBase.setStatus(CharacterUnitStatus.Working);
                break;

            case WorkCommon.WorkerState.BREAK:
                workerState = WorkCommon.WorkerState.BREAK;
                charaAnimeBase.removeStatus(CharacterUnitStatus.Working);
                charaAnimeBase.setStatus(CharacterUnitStatus.Rest);
                break;

            case WorkCommon.WorkerState.INTERFERE:
                workerState = WorkCommon.WorkerState.INTERFERE;
                break;

            case WorkCommon.WorkerState.BE_INTERFERED:
                workerState = WorkCommon.WorkerState.BE_INTERFERED;
                break;
        }
    }

    /// <summary>
    /// 妨害イベント発火
    /// </summary>
    private void InterfereTriggerOn()
    {
        unitContainer.Interfere(originId);
    }

    /// <summary>
    /// 妨害を終了する
    /// </summary>
    private void InterfereTriggerOff()
    {
        unitContainer.StopInterfere(originId, interfereOriginId);
    }

    /// <summary>
    /// 妨害する
    /// </summary>
    /// <param name="interfereOriginId"></param>
    public void Interfere(int interfereOriginId)
    {
        SetState(WorkCommon.WorkerState.INTERFERE);

        this.interfereOriginId = interfereOriginId;

        // TODO 妨害アクションを実装
    }

    /// <summary>
    /// 妨害される
    /// </summary>
    public void BeInterfered()
    {
        SetState(WorkCommon.WorkerState.BE_INTERFERED);
    }

    /// <summary>
    /// 作業をする
    /// </summary>
    public void Working()
    {
        SetState(WorkCommon.WorkerState.WORKING);

        // 各カウントを初期化
        troubleCount = 0;
        interfereCount = 0;

        // 妨害相手を初期化
        interfereOriginId = 0;
    }
}
