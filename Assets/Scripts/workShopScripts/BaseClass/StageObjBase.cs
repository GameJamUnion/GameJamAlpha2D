using System.Collections.Generic;
using UnityEngine;

public abstract class StageObjBase : ObjBase
{
    /// <summary>
    /// 作業物リスト
    /// </summary>
    protected List<Product> productList;

    /// <summary>
    /// 作業物出力先オブジェクト
    /// </summary>
    [SerializeField]
    protected StageObjBase outputObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        productList = new List<Product>();
    }

    /// <summary>
    /// 作業物を追加する
    /// </summary>
    /// <param name="product"></param>
    public abstract void addProduct(Product product);
}
