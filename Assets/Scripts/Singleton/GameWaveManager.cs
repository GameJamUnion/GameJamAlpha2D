using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public interface IGameWaveChangeReceiver
{
    public void receiveChangeWave(GameWaveChangeReceiveParam param);
}

public class GameWaveChangeReceiveParam
{
    public WorkScore WorkScore;
}
public class GameWaveManager : SingletonBase<GameWaveManager>
{
    #region Field
    private GameWaveSettings _WaveSettings = null;

    private List<IGameWaveChangeReceiver> _ReceiverList = new List<IGameWaveChangeReceiver>(16);

    private List<IGameWaveChangeReceiver> _WaitingReceiverList = new List<IGameWaveChangeReceiver>(4);

    private int _ActiveWorkScoreIndex = 0;
    private WorkScore _ActiveWorkScore = null;
    private WorkTime _ActiveWorkTime = null;

    // 通知用
    private GameWaveChangeReceiveParam _ReceiveParam = null;
    #endregion Field

    #region Method
    /// <summary>
    /// 初期化
    /// </summary>
    private void setup()
    {
        var workScore = _WaveSettings.Configuration.WorkScores[0];
        if (workScore != null)
        {

            _ActiveWorkScore = workScore;


            _ReceiveParam = new GameWaveChangeReceiveParam();
            _ReceiveParam.WorkScore = workScore;
            _ActiveWorkScore = workScore;

            _ActiveWorkScore.RegisterFullScoreEvent(onFullScoreEvent);
        }

        var workTime = _WaveSettings.Configuration.WorkTime;
        if (workTime != null)
        {
            _ActiveWorkTime = workTime;
            _ActiveWorkTime.RegisterTimeupEvent(onTimeUpEvent);
        }

    }

    public override void Update()
    {
        base.Update();


        // 設定があれば待機させていた通知を一斉通知
        lock (_WaitingReceiverList)
        {
            var count = _WaitingReceiverList.Count;
            if (count == 0)
            {
                return;
            }

            if (_ReceiveParam != null)
            {
                for (int i = 0; i < count; i++)
                {
                    _WaitingReceiverList[i].receiveChangeWave(_ReceiveParam);
                }
            }
            _WaitingReceiverList.Clear();
        }
    }

    public void registerWaveSettings(GameWaveSettings settings)
    {
        _WaveSettings = settings;

        setup();
    }

    /// <summary>
    /// Receiverの登録
    /// </summary>
    /// <param name="receiver"></param>
    public void registerReceiver(IGameWaveChangeReceiver receiver)
    {
        lock (_ReceiverList)
        {
            _ReceiverList.Add(receiver);
        }

        // 通知できる設定があればそのまま通知
        // 設定が未登録なら一旦待機
        if (_ReceiveParam != null)
        {
            receiver.receiveChangeWave(_ReceiveParam);
        }
        else
        {
            lock (_WaitingReceiverList)
            {
                _WaitingReceiverList.Add(receiver);
            }
        }
    }

    /// <summary>
    /// Wave切り替わり通知
    /// </summary>
    private void notifyWaveChange()
    {
        var count = _ReceiverList.Count;
        for (int i = 0; i < count; i++)
        {
            var receiver = _ReceiverList[i];
            receiver.receiveChangeWave(_ReceiveParam);
        }
    }

    /// <summary>
    /// スコア最大時イベント
    /// </summary>
    private void onFullScoreEvent()
    {
        if (_ActiveWorkScore == null)
        {
            return;
        }

        var count = _WaveSettings.Configuration.WorkScores.Length;
        if (count - 1 > _ActiveWorkScoreIndex)
        {
            // 次のWorkScoreがあれば遷移
            _ActiveWorkScoreIndex += 1;
            _ActiveWorkScore = _WaveSettings.Configuration.WorkScores[_ActiveWorkScoreIndex];

            // 通知
            _ReceiveParam.WorkScore = _ActiveWorkScore;
            notifyWaveChange();
        }
        else
        {
            // 終了

            // ゲームクリア開始
            startGameClear();
        }
    }

    /// <summary>
    /// タイムオーバー時イベント
    /// </summary>
    private void onTimeUpEvent()
    {
        startGameOver();
    }

    /// <summary>
    /// ゲームクリア開始
    /// </summary>
    private void startGameClear()
    {
        var score = 0;
        if (_ActiveWorkScore != null)
        {
            score = _ActiveWorkScore.score;
        }

        var time = 0f;
        if (_ActiveWorkTime != null)
        {
            time = _ActiveWorkTime.timeLimit * (1f - _ActiveWorkTime.timeRate);
        }

        GameClearManager.Instance.requestStartGameClear(new GameClearManager.GameClearStartArgs()
        {
            Score = score,
            RemainingTime = time,
        });
    }

    /// <summary>
    /// ゲームオーバー開始
    /// </summary>
    private void startGameOver()
    {
        var score = 0;
        if (_ActiveWorkScore != null)
        {
            score = _ActiveWorkScore.score;
        }

        var time = 0f;
        if (_ActiveWorkTime != null)
        {
            time = _ActiveWorkTime.timeLimit * (1f - _ActiveWorkTime.timeRate);
        }

        // ゲームオーバー開始
        GameOverManager.Instance.requestStartGameOver(new GameOverManager.GameOverStartArgs()
        {
            Score = score,
            RemainingTime = time,
        });
    }
    #endregion
}
