using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 作業場の管理クラス
/// </summary>
public class WorkManager : MonoBehaviour
{
    /// <summary>
    /// 作業場リスト
    /// </summary>
    [SerializeField]
    private List<WorkBase> workList;

    /// <summary>
    /// 作業員リスト
    /// </summary>
    private List<Worker> workerList;

    /// <summary>
    /// 指定の作業場を取得する
    /// </summary>
    /// <param name="placementState"></param>
    /// <returns></returns>
    public WorkBase GetWork(RI.PlacementState placementState)
    {
        return workList.FirstOrDefault(w => w.WorkId == placementState);    
    }

    /// <summary>
    /// 作業員を解雇する
    /// </summary>
    public void removeWorker(int originId)
    {
        foreach (WorkBase work in workList)
        {
            work.removeWorker(originId);
        }
    }
}
