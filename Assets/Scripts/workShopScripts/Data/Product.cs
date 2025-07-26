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
    /// コンストラクタ
    /// </summary>
    public Product()
    {
        workAmount = 100;   // TODO とりあえず100
    }

    /// <summary>
    /// プロパティ
    /// </summary>
    public int WorkAmount
    {
        get { return workAmount; }
        set { workAmount = value; }
    }
}
