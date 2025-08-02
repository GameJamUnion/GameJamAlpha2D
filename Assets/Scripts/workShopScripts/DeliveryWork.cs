using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

/// <summary>
/// 納品場クラス
/// </summary>
public class DeliveryWork : StageObjBase
    , IGameWaveChangeReceiver
{
    /// <summary>
    /// スコア
    /// </summary>
    [SerializeField]
    private WorkScore score;

    override protected void Start()
    {
        base.Start();
        GameWaveManager.Instance.registerReceiver(this);
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

    /// <summary>
    /// 作業物を追加する
    /// </summary>
    public override void AddProduct()
    {
        score.AddScore(1);
    }

    /// <summary>
    /// ウェーブが変更した際のイベント
    /// </summary>
    /// <param name="param"></param>
    public void receiveChangeWave(GameWaveChangeReceiveParam param)
    {
        score = param.WorkScore;
    }
}
