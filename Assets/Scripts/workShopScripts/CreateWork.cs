using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

/// <summary>
/// 生産場クラス
/// </summary>
public class CreateWork : WorkBase
{ 
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
        CreateProduct();
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void WorkPerSeconds()
    {
        if (workingProduct == null)
        {
            CreateProduct();
        }
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

    /// <summary>
    /// ぬい作成
    /// </summary>
    private void CreateProduct()
    {
        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.identity;
        Product product = Instantiate<Product>(productPrefab, pos, rot, productsRootTrans);
        product.gameObject.SetActive(false);
        workingProduct = product;
    }
}
