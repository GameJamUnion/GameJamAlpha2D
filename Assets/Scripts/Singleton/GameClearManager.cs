using UnityEngine;
using System;

public class GameClearManager : SingletonBase<GameClearManager>
{
	#region Definition
	public class GameClearStartArgs
	{
        // �\������X�R�A
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
    private GameClearStartArgs _GameClearStartRequest = null;
    private bool _GameClearEndRequest = false;
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
        // �Q�[���N���A�I��
        if (_GameClearEndRequest == true)
        {
            endGameClear();
            _GameClearEndRequest = false;

            // �I�����N�G�X�g��D�悳���邽�ߊJ�n���N�G�X�g�͏I��������
            _GameClearStartRequest = null;
            return;
        }

        // �Q�[���N���A�J�n
        if (_GameClearStartRequest != null)
        {
            startGameClear(_GameClearStartRequest);
            _GameClearStartRequest = null;
        }
    }
    #endregion �X�V����
    /// <summary>
    /// �Q�[���N���A�J�n����
    /// </summary>
    /// <param name="startArgs"></param>
    private void startGameClear(GameClearStartArgs startArgs)
    {
        // �|�[�Y������
        PauseManager.Instance.requestStartPause(new PauseManager.PauseRequestArgs()
        {
            Owner = this.GetType(),
            Type = PauseType.InGamePause,
        });

        // Gui�J��
        GuiManager.Instance.requestOpenGui(GuiManager.GuiType.GameClear, new GameClearGuiController.OpenParam()
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
    /// �Q�[���N���A�I������
    /// </summary>
    private void endGameClear()
    {
        // Gui����
        GuiManager.Instance.requestCloseGui(GuiManager.GuiType.GameClear);

        // �|�[�Y�I��
        PauseManager.Instance.requestEndPause(this.GetType());
    }
    #region Request
    /// <summary>
    /// �Q�[���N���A�J�n���N�G�X�g
    /// </summary>
    /// <param name="args"></param>
    public void requestStartGameClear(GameClearStartArgs args)
    {
        _GameClearStartRequest = args;
    }

    /// <summary>
    /// �Q�[���N���A�I�����N�G�X�g
    /// </summary>
    public void requestEndGameClear()
    {
        _GameClearEndRequest = true;
    }
    #endregion Request
    #endregion Method
}
