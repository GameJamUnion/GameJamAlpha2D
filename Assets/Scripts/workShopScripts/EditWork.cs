using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using WorkCommon;

/// <summary>
/// 加工場クラス
/// </summary>
public class EditWork : WorkBase
{
    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void WorkPerSeconds()
    {
        base.WorkPerSeconds();
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void WorkPerFlame()
    {
        base.WorkPerFlame();
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <returns></returns>
    protected override float GetWorkPower()
    {
        return workManager.GetWorkPower(workId);
    }
}