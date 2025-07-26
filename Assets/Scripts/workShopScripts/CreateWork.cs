using System.Linq;
using UnityEngine;

/// <summary>
/// 生産場クラス
/// </summary>
public class CreateWork : WorkBase
{
    /// <summary>
    /// 作成上限
    /// </summary>
    [SerializeField]
    private int maxCreateProductsNum;

    /// <summary>
    /// 生産力
    /// </summary>
    [SerializeField]
    private int createPower;

    /// <summary>
    /// スタート
    /// </summary>
    override protected void Start()
    {
        base.Start();
        productList = Enumerable.Range(0, maxCreateProductsNum).Select(i => new Product()).ToList();
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void workPerSeconds()
    {
        base.workPerSeconds();
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void workPerFlame()
    {
        base.workPerFlame();
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <returns></returns>
    protected override int getWorkPower()
    {
        return createPower;
    }
}
