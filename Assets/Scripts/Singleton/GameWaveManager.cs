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

    // �ʒm�p
    private GameWaveChangeReceiveParam _ReceiveParam = null;
    #endregion Field

    #region Method
    /// <summary>
    /// ������
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


        // �ݒ肪����Αҋ@�����Ă����ʒm����Ēʒm
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
    /// Receiver�̓o�^
    /// </summary>
    /// <param name="receiver"></param>
    public void registerReceiver(IGameWaveChangeReceiver receiver)
    {
        lock (_ReceiverList)
        {
            _ReceiverList.Add(receiver);
        }

        // �ʒm�ł���ݒ肪����΂��̂܂ܒʒm
        // �ݒ肪���o�^�Ȃ��U�ҋ@
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
    /// Wave�؂�ւ��ʒm
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
    /// �X�R�A�ő厞�C�x���g
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
            // ����WorkScore������ΑJ��
            _ActiveWorkScoreIndex += 1;
            _ActiveWorkScore = _WaveSettings.Configuration.WorkScores[_ActiveWorkScoreIndex];

            // �ʒm
            _ReceiveParam.WorkScore = _ActiveWorkScore;
            notifyWaveChange();
        }
        else
        {
            // �I��

            // �Q�[���N���A�J�n
            startGameClear();
        }
    }

    /// <summary>
    /// �^�C���I�[�o�[���C�x���g
    /// </summary>
    private void onTimeUpEvent()
    {
        startGameOver();
    }

    /// <summary>
    /// �Q�[���N���A�J�n
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
    /// �Q�[���I�[�o�[�J�n
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

        // �Q�[���I�[�o�[�J�n
        GameOverManager.Instance.requestStartGameOver(new GameOverManager.GameOverStartArgs()
        {
            Score = score,
            RemainingTime = time,
        });
    }
    #endregion
}
