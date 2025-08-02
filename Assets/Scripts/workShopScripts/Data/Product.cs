using UnityEngine;

/// <summary>
/// 作業物クラス
/// </summary>
public class Product : ProductMove
{
    /// <summary>
    /// 必要な作業量
    /// </summary>
    private int workAmount = 100;

    /// <summary>
    /// 点数
    /// </summary>
    private int scorePoint = 1;

    #region Property

    /// <summary>
    /// 必要な作業量
    /// </summary>
    public int WorkAmount
    {
        get { return workAmount; }
        set { workAmount = value; }
    }

    /// <summary>
    /// 点数
    /// </summary>
    public int ScorePoint
    {
        get { return scorePoint; }
        set { scorePoint = value; }
    }

    #endregion
}
