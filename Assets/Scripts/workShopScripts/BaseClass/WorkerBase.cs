using UnityEngine;

/// <summary>
/// 作業員の基底クラス
/// </summary>
public class WorkerBase
{
    /// <summary>
    /// 作業力
    /// </summary>
    private int workPower;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WorkerBase()
    {
        
    }

    /// <summary>
    /// プロパティ
    /// </summary>
    public int WorkPower
    { 
        get { return workPower; } 
        set { workPower = value; } 
    }
}
