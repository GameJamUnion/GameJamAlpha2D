using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// コンベアクラス
/// </summary>
public class Conveyor : StageObjBase
{
    /// <summary>
    /// 作成物毎の経過運搬時間
    /// </summary>
    //private List<int> productsCarryTimeList;

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
    /// 運搬距離
    /// </summary>
    [SerializeField]
    private int carryTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        productList = new List<Product>();
        //productsCarryTimeList = new List<int>();
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void WorkPerSeconds()
    {
        //CarryProduct();
        //OutputProduct();
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void WorkPerFlame()
    {

    }

    /// <summary>
    /// 作業物を運搬する(1処理分)
    /// </summary>
    //public void CarryProduct()
    //{
    //    if (productList != null && productList.Count > 0
    //        && productsCarryTimeList != null && productsCarryTimeList.Count > 0)
    //    {
    //        for (int i = 0; i < productsCarryTimeList.Count; i++)
    //        {
    //            productsCarryTimeList[i] -= 1;
    //        }

    //    }
    //}

    /// <summary>
    /// 最初の作業物を出力する
    /// </summary>
    //public void OutputProduct()
    //{
    //    if (productList != null && productList.Count > 0
    //        && productsCarryTimeList != null && productsCarryTimeList.Count > 0)
    //    {
    //        if (productsCarryTimeList[0] <= 0)
    //        {
    //            outputObj.AddProduct(productList[0]);
    //            productList.RemoveAt(0);
    //            productsCarryTimeList.RemoveAt(0);
    //        }
    //    }
    //}

    public void OutputProduct()
    {
        if (productList != null && productList.Count > 0)
        {
            outputObj.AddProduct(productList[0]);

            DstroyGameObj(productList[0].gameObject);
            productList.RemoveAt(0);
        }
    }

    /// <summary>
    /// 作業物を追加する
    /// </summary>
    /// <param name="product"></param>
    public override void AddProduct(Product product)
    {
        if (productList == null)
        {
            productList = new List<Product>();
        }

        Vector3 pos = new Vector3(100, 0, 0);
        Quaternion rot = Quaternion.identity;

        Product productMove = Instantiate<Product>(productPrefab, pos, rot, productsRootTrans);

        productList.Add(productMove);

        //if (productsCarryTimeList == null)
        //{
        //    productsCarryTimeList = new List<int>();
        //}
        //productsCarryTimeList.Add(carryTime);
    }

    /// <summary>
    /// 指定のオブジェクトを削除する
    /// </summary>
    private void DstroyGameObj(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }
}
