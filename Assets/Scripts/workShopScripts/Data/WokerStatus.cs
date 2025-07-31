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
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="_workPower"></param>
    /// <param name="_physical"></param>
    /// <param name="_listeningPower"></param>
    /// <param name="_lyingPower"></param>
    /// <param name="_obstaclePower"></param>
    /// <param name="_speed"></param>
    public WokerStatus(Dictionary<RI.PlacementState, float> _workPower,
        float _physical,
        float _listeningPower,
        float _lyingPower,
        float _obstaclePower,
        float _speed
        )
    {
        workPower = _workPower;
        physical = _physical;
        listeningPower = _listeningPower;
        lyingPower = _lyingPower;
        obstaclePower = _obstaclePower;
        speed = _speed;
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

    #region Property

    /// <summary>
    /// ���v�́E�W����
    /// </summary>
    public float Physical
    {
        get { return physical; }
        set { physical = value; }
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

    #endregion
}
