using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class WorkBase : ObjBase
{
    /// <summary>
    /// 作業状況
    /// </summary>
    private WorkState workState;

    /// <summary>
    /// 作業員リスト
    /// </summary>
    protected List<WorkerBase> workerList;

    /// <summary>
    /// 現在の作業物
    /// </summary>
    private Product workingProduct;

    /// <summary>
    /// 実作業ポイント
    /// </summary>
    private int workingPoint;

    /// <summary>
    /// 炎上するカウント数
    /// </summary>
    [SerializeField]
    private int burningCount;

    /// <summary>
    /// スプライトれんだらー
    /// </summary>
    [SerializeField]
    private SpriteRenderer sprite;

    /// <summary>
    /// カラーマネージャー
    /// </summary>
    [SerializeField]
    private WorkColorManager workColorManager;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        workState = WorkState.EMPTY;
        workerList = new List<WorkerBase>();
        workingPoint = 0;
        workingProduct = null;
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void workPerSeconds()
    {
        execWork();
    }


    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void workPerFlame()
    {
        updateWorkState();
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
    }

    /// <summary>
    /// 作業員を配置する
    /// </summary>
    /// <param name="worker"></param>
    public void addWorker(WorkerBase worker)
    {
        if (workerList == null)
        {
            workerList = new List<WorkerBase>();
        }
        workerList.Add(worker);
    }

    /// <summary>
    /// 作業員を解雇する
    /// </summary>
    /// <param name="worker"></param>
    public void removeWorker(WorkerBase worker)
    {
        if (workerList != null && workerList.Contains(worker))
        {
            workerList.Remove(worker);
        }
    }

    /// <summary>
    /// 作業物を次のオブジェクトに出力して現在の作業物を空にする
    /// </summary>
    protected void outputProduct()
    {
        if (outputObj != null && workingProduct != null)
        {
            outputObj.addProduct(workingProduct);
            workingProduct = null;
            workingPoint = 0;
        }
    }

    /// <summary>
    /// 作業状態を更新する
    /// </summary>
    private void updateWorkState()
    {
        if (productList.Count <= 0)
        {
            workState = WorkState.EMPTY;
        }
        else if (productList.Count > burningCount)
        {
            workState = WorkState.BURNING;
        }
        else
        {
            workState = WorkState.WORKING;
        }

        workColorManager.spriteColorChange(sprite, workState);
    }

    /// <summary>
    /// 作業力にあわせて作業を進める
    /// </summary>
    protected void execWork()
    {
        sliceProductToWorkingProduct();

        if (workingProduct != null)
        {
            workingPoint += getWorkPower();

            if (workingPoint >= workingProduct.WorkAmount)
            {
                outputProduct();
            }
        }
    }

    /// <summary>
    /// 作業中の物がない場合に作業物をセットする
    /// </summary>
    private void sliceProductToWorkingProduct()
    {
        if (workingProduct == null)
        {
            if (productList.Count > 0)
            {
                workingProduct = productList[0];
                productList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <returns></returns>
    abstract protected int getWorkPower();
}
