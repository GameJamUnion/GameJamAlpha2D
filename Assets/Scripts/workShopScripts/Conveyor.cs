using UnityEngine;
using System.Collections.Generic;

public class Conveyor
{
    // 作業物リスト
    private List<Product> productList;

    // 排出先作業場
    private WorkBase outputWork;

    // 運搬力
    private int carryPower;

    // コンストラクタ
    public Conveyor()
    {
        productList = new List<Product>();
        carryPower = 1;                         // とりあえず1固定値
    }

    // 作業物を追加する
    public void addProduct(Product p)
    {
        productList.Add(p);
    }

    // 作業物を運搬する(1処理分)
    public void carryProduct()
    {

    }

    public void outputProduct()
    {
        
    }

}
