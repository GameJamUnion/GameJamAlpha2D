using UnityEngine;

/// <summary>
/// 作業物クラス
/// </summary>
public class Product
{
    /// <summary>
    /// 必要な作業量
    /// </summary>
    private int workAmount;

    /// <summary>
    /// 点数
    /// </summary>
    private int scorePoint;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Product()
    {
        workAmount = 100;   // TODO とりあえず100
        scorePoint = 1;     // TODO とりあえず1
    }

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

}
