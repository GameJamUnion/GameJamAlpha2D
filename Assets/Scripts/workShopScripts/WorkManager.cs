using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ��Ə�̊Ǘ��N���X
/// </summary>
public class WorkManager : MonoBehaviour
{
    /// <summary>
    /// ��Ɨ͔{�� TODO�@��X�����H
    /// </summary>
    [SerializeField]
    private float workPowerRate;

    /// <summary>
    /// ��Ə�V�[�����C������
    /// </summary>
    [SerializeField]
    private WorkShopSceneMain sceneMain;

    /// <summary>
    /// ��Əꃊ�X�g
    /// </summary>
    [SerializeField]
    private List<WorkBase> workList;

    /// <summary>
    /// ��ƈ����X�g
    /// </summary>
    private List<Worker> workerList = new List<Worker>();

    /// <summary>
    /// ��ƈ������ق���
    /// </summary>
    /// <param name="originId"></param>
    public void RemoveWorker(int originId)
    {
        if (workerList != null)
        {
            List<Worker> removeList = workerList.Where(w => w.OriginId == originId).ToList();

            foreach (Worker worker in removeList)
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
    /// ��ƈ����ٗp����
    /// </summary>
    public void EmployWoker(Worker worker)
    {
        workerList.Add(worker);
    }

    /// <summary>
    /// �w��ID�̍�ƈ����擾����
    /// </summary>
    /// <param name="originId"></param>
    /// <returns></returns>
    public Worker GetWorker(int originId)
    {
        return workerList.FirstOrDefault(w => w.OriginId == originId);
    }

    /// <summary>
    /// �w���Ə�ɔz������Ă����ƈ���Ԃ�
    /// </summary>
    /// <param name="placementState"></param>
    /// <returns></returns>
    public List<Worker> GetWokerList(RI.PlacementState placementState)
    {
          return workerList.Where(w => w.AssingWorkId == placementState).ToList();
    }

    /// <summary>
    /// �w��̍�Ə���擾����
    /// </summary>
    /// <param name="placementState"></param>
    /// <returns></returns>
    public WorkBase GetWork(RI.PlacementState placementState)
    {
        return workList.FirstOrDefault(w => w.WorkId == placementState);    
    }

    /// <summary>
    /// �w��̍�Ə�ɑ΂��Ă̍�Ɨ͂�Ԃ�
    /// </summary>
    /// <param name="placementState"></param>
    /// <returns></returns>
    public float GetWorkPower(RI.PlacementState placementState)
    {
        List<Worker> list = GetWokerList(placementState);
        float power = list.Sum(w => w.WokerStatus.GetWorkPower(placementState));
        return power * workPowerRate; // TODO ��X�{�|���͂�߂�
    }

    /// <summary>
    /// �w��̃I�u�W�F�N�g���폜����
    /// </summary>
    private void DstroyGameObj(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }
}
