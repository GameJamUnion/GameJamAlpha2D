using UnityEngine;

/// <summary>
/// 納品場クラス
/// </summary>
public class DeliveryWork : StageObjBase
{
    /// <summary>
    /// スコア
    /// </summary>
    [SerializeField]
    private WorkScore score;

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void workPerSeconds()
    {

    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void workPerFlame()
    {

    }

    /// <summary>
    /// 作業物を追加する
    /// </summary>
    /// <param name="product"></param>
    public override void addProduct(Product product)
    {
        score.AddScore(product.ScorePoint);
    }
}
