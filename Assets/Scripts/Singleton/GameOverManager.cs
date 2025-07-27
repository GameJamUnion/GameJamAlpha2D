using System;
using UnityEngine;

public class GameOverManager : SingletonBase<GameOverManager>
{
    #region Definition
    public class GameOverStartArgs
    {
        // �\������X�R�A�Ȃ�
        public int Score = 0;

        // �c�莞��
        public float RemainingTime = 0.0f;

        // �������Ă΂�鏈��
        public Action onStartProcess;

        // �I�����Ă΂�鏈��
        //public Action onEndProcess; �K�v�ɂȂ�������
    }

    #endregion Definition

    #region Field
    private GameOverStartArgs _GameOverStartRequest = null;
    private bool _GameOverEndRequest = false;
    #endregion Field

    #region Method
    #region �X�V����

    public override void LateUpdate()
    {
        base.LateUpdate();

        updateRequest();
    }

    /// <summary>
    /// ���N�G�X�g����
    /// </summary>
    private void updateRequest()
    {
        // �Q�[���I�[�o�[�I��
        if (_GameOverEndRequest == true)
        {
            endGameOver();
            _GameOverEndRequest = false;

            // �I�����N�G�X�g��D�悳���邽�ߊJ�n���N�G�X�g�͏I��������
            _GameOverStartRequest = null;
            return;
        }


        // �Q�[���I�[�o�[�J�n
        if (_GameOverStartRequest != null)
        {
            startGameOver(_GameOverStartRequest);
            _GameOverStartRequest = null;
        }

    }
    #endregion �X�V����
    /// <summary>
    /// �Q�[���I�[�o�[�J�n����
    /// </summary>
    /// <param name="startArgs"></param>
    private void startGameOver(GameOverStartArgs startArgs)
    {
        // �|�[�Y������
        PauseManager.Instance.requestStartPause(new PauseManager.PauseRequestArgs()
        {
            Owner = this.GetType(),
            Type = PauseType.InGamePause,
        });

        // Gui�J��
        GuiManager.Instance.requestOpenGui(GuiManager.GuiType.GameOver, new GameOverGuiController.OpenParam()
        {
            // �J���ۂɓn���p�����[�^
            // startArgs����n��
            Score = startArgs.Score,
            RemainingTime = startArgs.RemainingTime,
        });

        // �T�E���h�Đ�
        SoundManager.Instance.requestPlaySound(BGMKind.Result);

        startArgs.onStartProcess?.Invoke();
    }

    /// <summary>
    /// �Q�[���I�[�o�[�I������
    /// </summary>
    private void endGameOver()
    {
        // Gui����
        GuiManager.Instance.requestCloseGui(GuiManager.GuiType.GameOver);

        // �|�[�Y�I��
        PauseManager.Instance.requestEndPause(this.GetType());

    }

    #region Request
    /// <summary>
    /// �Q�[���I�[�o�[�J�n���N�G�X�g
    /// </summary>
    /// <param name="args"></param>
    public void requestStartGameOver(GameOverStartArgs args)
    {
        _GameOverStartRequest = args;
    }

    /// <summary>
    /// �Q�[���I�[�o�[�I�����N�G�X�g
    /// </summary>
    public void requestEndGameOver()
    {
        _GameOverEndRequest = true;
    }
    #endregion Request
    #endregion Method

}
