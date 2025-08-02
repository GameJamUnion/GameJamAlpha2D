using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 作業場の管理クラス
/// </summary>
public class WorkManager : MonoBehaviour
{
    /// <summary>
    /// 作業力倍率 TODO　後々消す？
    /// </summary>
    [SerializeField]
    private float workPowerRate;

    /// <summary>
    /// 作業場シーンメイン処理
    /// </summary>
    [SerializeField]
    private WorkShopSceneMain sceneMain;

    /// <summary>
    /// 作業場リスト
    /// </summary>
    [SerializeField]
    private List<WorkBase> workList;

    /// <summary>
    /// 作業員リスト
    /// </summary>
    private List<Worker> workerList = new List<Worker>();


    /// <summary>
    /// 作業員を解雇する
    /// </summary>
    /// <param name="originId"></param>
    /// <param name="placementState"></param>
    public void RemoveWorker(int originId, RI.PlacementState placementState)
    {
        if (workerList != null && workList != null)
        {
            WorkBase work = workList.Where(w => w.WorkId == placementState).FirstOrDefault();

            if (work != null)
            {
                work.RemoveUnit(originId);
            }

            List<Worker> removeWorkerList = workerList.Where(w => w.OriginId == originId).ToList();

            foreach (Worker worker in removeWorkerList)
            {
                if (worker != null)
                {
                    DstroyGameObj(worker.gameObject);
                }
                workerList.Remove(worker);
            }
        }
    }

    /// <summary>
    /// 作業員を雇用する
    /// </summary>
    public void EmployWoker(Worker worker)
    {
        workerList.Add(worker);
    }

    /// <summary>
    /// 指定IDの作業員を取得する
    /// </summary>
    /// <param name="originId"></param>
    /// <returns></returns>
    public Worker GetWorker(int originId)
    {
        return workerList.FirstOrDefault(w => w.OriginId == originId);
    }

    /// <summary>
    /// 指定作業場に配属されている作業員を返す
    /// </summary>
    /// <param name="placementState"></param>
    /// <returns></returns>
    public List<Worker> GetWokerList(RI.PlacementState placementState)
    {
          return workerList.Where(w => w.AssingWorkId == placementState).ToList();
    }

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
    /// 指定の作業場に対しての作業力を返す
    /// </summary>
    /// <param name="placementState"></param>
    /// <returns></returns>
    public float GetWorkPower(RI.PlacementState placementState)
    {
        List<Worker> list = GetWokerList(placementState);
        float power = list.Sum(w => w.WokerStatus.GetWorkPower(placementState));
        return power * workPowerRate; // TODO 後々倍掛けはやめる
    }

    /// <summary>
    /// 妨害する
    /// </summary>
    /// <param name="interfereOriginId"></param>
    public void InterfereWorker(int interfereOriginId)
    {
        Worker randomWorker = workerList
            .Where(w => w.OriginId != interfereOriginId && w.WorkerState == WorkCommon.WorkerState.WORKING)
            .OrderBy(w => Random.value)
            .FirstOrDefault();

        if (randomWorker != null)
        {
            Worker interfereWorker = GetWorker(interfereOriginId);
            interfereWorker.Interfere(randomWorker.OriginId);
            randomWorker.BeInterfered();
        }
    }

    /// <summary>
    /// 妨害を終了する
    /// </summary>
    /// <param name="interfereOriginId"></param>
    /// <param name="beInterferedOriginID"></param>
    public void StopInterfereWorker(int interfereOriginId, int beInterferedOriginID)
    {
        Worker interfereWorker = GetWorker(interfereOriginId);
        if (interfereWorker != null)
        {
            interfereWorker.Working();
        }

        Worker beInterfereWorker = GetWorker(beInterferedOriginID);
        if (beInterfereWorker != null)
        {
            beInterfereWorker.Working();
        }   
    }

    /// <summary>
    /// 指定のオブジェクトを削除する
    /// </summary>
    private void DstroyGameObj(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }
}
