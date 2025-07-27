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
    private List<int> productsCarryTimeList;

    /// <summary>
    /// 運搬力
    /// </summary>
    [SerializeField]
    private int carryPower;

    /// <summary>
    /// 運搬距離
    /// </summary>
    [SerializeField]
    private int carryDistance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        productList = new List<Product>();
        productsCarryTimeList = new List<int>();
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void workPerSeconds()
    {
        carryProduct();
        outputProduct();
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void workPerFlame()
    {
        
    }

    /// <summary>
    /// 作業物を運搬する(1処理分)
    /// </summary>
    public void carryProduct()
    {
        if (productList != null && productList.Count > 0
            && productsCarryTimeList != null && productsCarryTimeList.Count > 0)
        {
            for (int i = 0; i < productsCarryTimeList.Count; i++)
            {
                productsCarryTimeList[i] += carryPower;
            }

        }
    }

    /// <summary>
    /// 最初の作業物を出力する
    /// </summary>
    public void outputProduct()
    {
        if (productList != null && productList.Count > 0 
            && productsCarryTimeList != null && productsCarryTimeList.Count > 0)
        {
            if (productsCarryTimeList[0] > carryDistance)
            {
                outputObj.addProduct(productList[0]);
                productList.RemoveAt(0);
                productsCarryTimeList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// 作業物を追加する
    /// </summary>
    /// <param name="product"></param>
    public override void addProduct(Product product)
    {
        if (productList == null)
        {
            productList = new List<Product>();
        }
        productList.Add(product);

        if (productsCarryTimeList == null)
        {
            productsCarryTimeList = new List<int>();
        }
        productsCarryTimeList.Add(0);
    }
}
