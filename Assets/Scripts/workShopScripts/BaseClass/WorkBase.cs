using UnityEngine;
using System.Collections.Generic;

/// 作業場の基底クラス
public class WorkBase
{
    // 作業員リスト
    private List<WorkerBase> workerList;

    // 作業物リスト
    private List<Product> productList;

    // 実作業の値
    private int workPoint;

    // 作業中の物
    private Product workProduct;

    // 排出先コンベア
    private Conveyor outputConveyor;

    // コンストラクタ
    public WorkBase()
    {

    }

    // 作業
    public void work(int workAmount)
    {
        workPoint += workAmount;

        if (workPoint >= workProduct.WorkAmount)
        {
            outputProduct();
        }
    }

    // 作成物をコンベアに流す
    private void outputProduct()
    {
        outputConveyor.addProduct(workProduct);
        workProduct = null;
    }
}
