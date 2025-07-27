using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ��Ə�̊Ǘ��N���X
/// </summary>
public class WorkManager : MonoBehaviour
{
    /// <summary>
    /// ��Əꃊ�X�g
    /// </summary>
    [SerializeField]
    private List<WorkBase> workList;

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
    /// ��ƈ������ق���
    /// </summary>
    public void removeWorker(int originId)
    {
        foreach (WorkBase work in workList)
        {
            work.removeWorker(originId);
        }
    }
}
