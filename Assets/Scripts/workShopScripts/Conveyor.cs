using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// コンベアクラス
/// </summary>
public class Conveyor : StageObjBase
{
    /// <summary>
    /// 作成物運搬クラス
    /// </summary>
    [SerializeField]
    private Product productPrefab;

    /// <summary>
    /// プロダクト配置の親オブジェクト
    /// </summary>
    [SerializeField]
    private Transform productsRootTrans;

    /// <summary>
    /// プロダクトの運搬ルート
    /// </summary>
    [SerializeField]
    private UnitWalkRoute m_route;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        productList = new List<Product>();
    }

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

    public void OutputProduct()
    {
        if (productList != null && productList.Count > 0)
        {
            outputObj.AddProduct();

            DstroyGameObj(productList[0].gameObject);
            productList.RemoveAt(0);
        }
    }

    /// <summary>
    /// 作業物を追加する
    /// </summary>
    public override void AddProduct()
    {
        if (productList == null)
        {
            productList = new List<Product>();
        }

        // 画面に表示されない場所に生成
        Vector3 pos = new Vector3(100, 0, 0);
        Quaternion rot = Quaternion.identity;

        Product product = Instantiate<Product>(productPrefab, pos, rot, productsRootTrans);
        product.RegisterExitMoveEvent(OutputProduct);
        productList.Add(product);
        product.StartWalk(m_route);
    }

    /// <summary>
    /// 指定のオブジェクトを削除する
    /// </summary>
    private void DstroyGameObj(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }
}
