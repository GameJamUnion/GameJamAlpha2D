using System.Collections.Generic;

/// <summary>
/// ��ƈ��̃X�e�[�^�X
/// </summary>
public class WokerStatus
{
    /// <summary>
    /// ��Ɨ�
    /// </summary>
    private Dictionary<RI.PlacementState, float> workPower;

    /// <summary>
    /// ���v�́E�W����
    /// </summary>
    private float physical;

    /// <summary>
    /// ����
    /// </summary>
    private float listeningPower;

    /// <summary>
    /// �R���x
    /// </summary>
    private float lyingPower;

    /// <summary>
    /// �ז���
    /// </summary>
    private float obstaclePower;

    /// <summary>
    /// ���̑���
    /// </summary>
    private float speed;

    /// <summary>
    /// ��Ɨ�
    /// </summary>
    /// <param name="wPower"></param>
    public WokerStatus(Dictionary<RI.PlacementState, float> wPower)
    {
        workPower = wPower;
    }

    /// <summary>
    /// �w���Ə�ID�̍�Ɨ͂��擾����
    /// </summary>
    /// <param name="workId"></param>
    /// <returns></returns>
    public float getWorkPower(RI.PlacementState workId)
    {
        return workPower[workId];
    }

    /// <summary>
    /// ����
    /// </summary>
    public float ListeningPower
    {
        get { return listeningPower; }
        set { listeningPower = value; }
    }

    /// <summary>
    /// ���v�́E�W����
    /// </summary>
    public float Physical
    {
        get { return physical; }
        set { physical = value; }
    }

    /// <summary>
    /// �R���x
    /// </summary>
    public float LyingPower
    {
        get { return lyingPower; }
        set { lyingPower = value; }
    }

    /// <summary>
    /// �ז���
    /// </summary>
    public float ObstaclePower
    { 
        get { return obstaclePower; }
        set { obstaclePower = value; }
    }

    /// <summary>
    /// ���̑���
    /// </summary>
    public float Speed
    { 
        get { return speed; }
        set { speed = value; }
    }
}
