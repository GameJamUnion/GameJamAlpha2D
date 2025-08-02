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
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;

        for (int i = 0; i < maxCreateProductsNum; i++)
        {
            Product product = Instantiate<Product>(productPrefab, pos, rot, productsRootTrans);
            product.gameObject.SetActive(false);
            productList.Add(product);
        }
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void WorkPerSeconds()
    {
        base.WorkPerSeconds();
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void WorkPerFlame()
    {
        base.WorkPerFlame();
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <returns></returns>
    protected override float GetWorkPower()
    {
        return createPower;
    }
}
