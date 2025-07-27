using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 作業員クラス
/// </summary>
public class Worker : ObjBase
{
    /// <summary>
    /// 作業員ID
    /// </summary>
    private int originId;

    /// <summary>
    /// 作業力
    /// </summary>
    private float workPower;


    private WorkCommon.WorkerState workerState;

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
        
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void workPerFlame()
    {
        
    }

    /// <summary>
    /// 作業員ID
    /// </summary>
    public int OriginId
    {
        get { return originId; }
        set { originId = value; }
    }

    /// <summary>
    /// 作業力
    /// </summary>
    public float WorkPower
    { 
        get { return workPower; } 
        set { workPower = value; } 
    }

    /// <summary>
    /// 自身のゲームオブジェクトを削除する
    /// </summary>
    public void destroyThis()
    {
        Destroy(gameObject);
    }
}
