using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 作業場の基底クラス
/// </summary>
public abstract class WorkBase : StageObjBase
{
    /// <summary>
    /// 作業場のID
    /// </summary>
    [SerializeField]
    private RI.PlacementState workId;

    /// <summary>
    /// 作業状況
    /// </summary>
    private WorkCommon.WorkState workState;

    /// <summary>
    /// 作業員リスト
    /// </summary>
    protected List<Worker> workerList;

    /// <summary>
    /// 現在の作業物
    /// </summary>
    private Product workingProduct;

    /// <summary>
    /// 実作業ポイント
    /// </summary>
    private float workingPoint;

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

    /// <summary>
    /// 作業場のID
    /// </summary>
    public RI.PlacementState WorkId
    {
        get { return workId; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        workState = WorkCommon.WorkState.EMPTY;
        workerList = new List<Worker>();
        workingPoint = 0.0f;
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
    public void addWorker(Worker worker)
    {
        if (workerList == null)
        {
            workerList = new List<Worker>();
        }
        workerList.Add(worker);
    }

    /// <summary>
    /// 作業員を解雇する
    /// </summary>
    /// <param name="originId"></param>
    public void removeWorker(int originId)
    {
        if (workerList != null)
        {
            List<Worker> removeList = workerList.Where(w => w.OriginId == originId).ToList();

            foreach (Worker worker in removeList)
            {
                if (worker != null)
                {
                    destroyGameObj(worker.gameObject);
                }
                workerList.Remove(worker);
            }
        }
    }

    /// <summary>
    /// 作業員の数を返す
    /// </summary>
    /// <returns></returns>
    public int workerCount()
    {
        return workerList.Count;
    }

    /// <summary>
    /// 指定の作業員が存在するかどうか
    /// </summary>
    /// <param name="originId"></param>
    /// <returns></returns>
    public bool ExistsWorker(int originId)
    {
        bool exists = false;

        if (workerList != null)
        {
            exists = workerList.Any(w => w.OriginId == originId);
        }

        return exists;
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
            workingPoint = 0.0f;
        }
    }

    /// <summary>
    /// 作業状態を更新する
    /// </summary>
    private void updateWorkState()
    {
        if (productList != null)
        {
            if (productList.Count == 0 && workingProduct == null)
            {
                workState = WorkCommon.WorkState.EMPTY;
            }
            else if (productList.Count > burningCount)
            {
                workState = WorkCommon.WorkState.BURNING;
            }
            else if (workingProduct != null || productList.Count != 0)
            {
                workState = WorkCommon.WorkState.WORKING;
            }
            else
            {
                // 入ることはないはず
                workState = WorkCommon.WorkState.EMPTY;
            }

            workColorManager.spriteColorChange(sprite, workState);
        }
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
    /// 指定のオブジェクトを削除する
    /// </summary>
    private void destroyGameObj(GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <returns></returns>
    abstract protected float getWorkPower();
}
