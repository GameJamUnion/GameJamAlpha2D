using UnityEngine;

/// <summary>
/// 納品場クラス
/// </summary>
public class DeliveryWork : WorkBase
{
    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected override void workPerSeconds()
    {
        base.workPerSeconds();
    }

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected override void workPerFlame()
    {
        base.workPerFlame();
    }

    /// <summary>
    /// 作業力を取得する
    /// </summary>
    /// <returns></returns>
    protected override int getWorkPower()
    {
        // ここは処理されない想定
        return 0;
    }

    /// <summary>
    /// 納品する
    /// summary>
    public void DeliverProduct()
    {
        if (this.productList != null && this.productList.Count > 0)
        {
            // 納品処理の実装
            foreach (var product in this.productList)
            {
                // ここで納品処理を行う

            }
            // 納品後はリストをクリアする
            this.productList.Clear();
        }
    }
}
