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
    protected override void WorkPerSeconds()
    {

    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void WorkPerFlame()
    {

    }

    /// <summary>
    /// 作業物を追加する
    /// </summary>
    /// <param name="product"></param>
    public override void AddProduct(Product product)
    {
        score.AddScore(product.ScorePoint);
    }
}
