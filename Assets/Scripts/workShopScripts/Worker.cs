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

        WorkDataManager.Instance.registerWorker(this);
    }

    private void OnDestroy()
    {
        WorkDataManager.Instance.unregisterWorker(this);
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
        //Destroy(gameObject);
        WorkDataManager.Instance.destroy(this);
    }
}

public class WorkDataManager : SingletonBase<WorkDataManager>
{
    List<Worker> _Workers = new List<Worker>(16);

    public void registerWorker(Worker obj)
    {
        _Workers.Add(obj);
    }

    public void unregisterWorker(Worker obj)
    {
        _Workers.Remove(obj);
    }

    public void destroy(Worker obj)
    {
        if (_Workers.Contains(obj))
        {
            if (obj != null)
            {
                GameObject.Destroy(obj.gameObject);
                unregisterWorker(obj);
            }
        }        
    }
}
