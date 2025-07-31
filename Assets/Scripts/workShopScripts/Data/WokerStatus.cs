using System.Collections.Generic;

/// <summary>
/// 作業員のステータス
/// </summary>
public class WokerStatus
{
    /// <summary>
    /// 作業力
    /// </summary>
    private Dictionary<RI.PlacementState, float> workPower;

    /// <summary>
    /// 持久力・集中力
    /// </summary>
    private float physical;

    /// <summary>
    /// 聴力
    /// </summary>
    private float listeningPower;

    /// <summary>
    /// 嘘つき度
    /// </summary>
    private float lyingPower;

    /// <summary>
    /// 邪魔力
    /// </summary>
    private float obstaclePower;

    /// <summary>
    /// 足の速さ
    /// </summary>
    private float speed;

    /// <summary>
    /// コンストラクタ
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
    /// 指定作業場IDの作業力を取得する
    /// </summary>
    /// <param name="workId"></param>
    /// <returns></returns>
    public float getWorkPower(RI.PlacementState workId)
    {
        return workPower[workId];
    }

    #region Property

    /// <summary>
    /// 持久力・集中力
    /// </summary>
    public float Physical
    {
        get { return physical; }
        set { physical = value; }
    }

    /// <summary>
    /// 聴力
    /// </summary>
    public float ListeningPower
    {
        get { return listeningPower; }
        set { listeningPower = value; }
    }

    /// <summary>
    /// 嘘つき度
    /// </summary>
    public float LyingPower
    {
        get { return lyingPower; }
        set { lyingPower = value; }
    }

    /// <summary>
    /// 邪魔力
    /// </summary>
    public float ObstaclePower
    { 
        get { return obstaclePower; }
        set { obstaclePower = value; }
    }

    /// <summary>
    /// 足の速さ
    /// </summary>
    public float Speed
    { 
        get { return speed; }
        set { speed = value; }
    }

    #endregion
}
