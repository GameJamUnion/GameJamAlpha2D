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
    protected RI.PlacementState workId;

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
    /// 作業場の管理クラス
    /// </summary>
    [SerializeField]
    protected WorkManager workManager;

    /// <summary>
    /// 作業員の配置場所リスト
    /// </summary>
    [SerializeField]
    private List<UnitPlacement> unitPlacementList;

    /// <summary>
    /// 作業状況
    /// </summary>
    private WorkCommon.WorkState workState;

    /// <summary>
    /// 現在の作業物
    /// </summary>
    private Product workingProduct;

    /// <summary>
    /// 実作業ポイント
    /// </summary>
    private float workingPoint;

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
        workingPoint = 0.0f;
        workingProduct = null;
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void WorkPerSeconds()
    {
        ExecWork();
    }


    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void WorkPerFlame()
    {
        UpdateWorkState();
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
        productList.Add(product);
    }

    /// <summary>
    /// 作業物を次のオブジェクトに出力して現在の作業物を空にする
    /// </summary>
    protected void OutputProduct()
    {
        if (outputObj != null && workingProduct != null)
        {
            outputObj.AddProduct(workingProduct);
            workingProduct = null;
            workingPoint = 0.0f;
        }
    }

    /// <summary>
    /// 作業状態を更新する
    /// </summary>
    private void UpdateWorkState()
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
        }
    }

    /// <summary>
    /// 作業力にあわせて作業を進める
    /// </summary>
    protected void ExecWork()
    {
        SliceProductToWorkingProduct();

        if (workingProduct != null)
        {
            workingPoint += GetWorkPower();

            if (workingPoint >= workingProduct.WorkAmount)
            {
                OutputProduct();
            }
        }
    }

    /// <summary>
    /// 作業中の物がない場合に作業物をセットする
    /// </summary>
    private void SliceProductToWorkingProduct()
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
    /// 使用可能な作業員配置場所を取得して配置作業員情報を入力する
    /// </summary>
    /// <param name="originId"></param>
    /// <returns></returns>
    public Transform GetAvailableUnitPlacement(int originId)
    {
        UnitPlacement unitPlacement = unitPlacementList.Where(u => u.Available == true)
            .OrderBy(u => Random.value)
            .FirstOrDefault();

        if (unitPlacement != null)
        {
            unitPlacement.PlacementOriginId = originId;
            unitPlacement.Available = false;
        }

        return unitPlacement.Transform;
    } 

    /// <summary>
    /// 配置していた作業員を削除する
    /// </summary>
    /// <param name="originId"></param>
    public void RemoveUnit(int originId)
    {
        List<UnitPlacement> removeUnitPlacementList = unitPlacementList.Where(u => u.PlacementOriginId == originId).ToList();
        
        foreach (UnitPlacement unitPlacement in removeUnitPlacementList)
        {
            unitPlacement.PlacementOriginId = 0;
            unitPlacement.Available = true;
        }
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <returns></returns>
    abstract protected float GetWorkPower();
}
