using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 加工場クラス
/// </summary>
public class EditWork : WorkBase
{
    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void workPerSeconds()
    {
        base.workPerSeconds();
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void workPerFlame()
    {
        base.workPerFlame();
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <returns></returns>
    protected override int getWorkPower()
    {
        int workPower = 0;
        //int workPower = 50;

        if (workerList != null && workerList.Count > 0)
        {
            foreach (Worker worker in workerList)
            {
                workPower += worker.WorkPower;
            }
        }

        return workPower;
    }
}