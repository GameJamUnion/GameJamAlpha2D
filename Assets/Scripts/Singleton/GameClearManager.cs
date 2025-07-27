using UnityEngine;
using System;

public class GameClearManager : SingletonBase<GameClearManager>
{
	#region Definition
	public class GameClearStartArgs
	{
        // 表示するスコア
        public int Score = 0;

        // 残り時間
        public float RemainingTime = 0.0f;

		// 成立時呼ばれる処理
		public Action onStartProcess;
        // 終了時呼ばれる処理
        //public Action onEndProcess; 必要になったら解放
    }
    #endregion Definition
    #region Field
    private GameClearStartArgs _GameClearStartRequest = null;
    private bool _GameClearEndRequest = false;
    #endregion Field

    #region Method
    #region 更新処理
    public override void LateUpdate()
    {
        base.LateUpdate();

        updateRequest();
    }

    /// <summary>
    /// リクエスト処理
    /// </summary>
    private void updateRequest()
    {
        // ゲームクリア終了
        if (_GameClearEndRequest == true)
        {
            endGameClear();
            _GameClearEndRequest = false;

            // 終了リクエストを優先させるため開始リクエストは終了させる
            _GameClearStartRequest = null;
            return;
        }

        // ゲームクリア開始
        if (_GameClearStartRequest != null)
        {
            startGameClear(_GameClearStartRequest);
            _GameClearStartRequest = null;
        }
    }
    #endregion 更新処理
    /// <summary>
    /// ゲームクリア開始処理
    /// </summary>
    /// <param name="startArgs"></param>
    private void startGameClear(GameClearStartArgs startArgs)
    {
        // ポーズかける
        PauseManager.Instance.requestStartPause(new PauseManager.PauseRequestArgs()
        {
            Owner = this.GetType(),
            Type = PauseType.InGamePause,
        });

        // Gui開く
        GuiManager.Instance.requestOpenGui(GuiManager.GuiType.GameClear, new GameClearGuiController.OpenParam()
        {
            // 開く際に渡すパラメータ
            // startArgsから渡す
            Score = startArgs.Score,
            RemainingTime = startArgs.RemainingTime,
        });

        // サウンド再生
        SoundManager.Instance.requestPlaySound(BGMKind.Result);

        startArgs.onStartProcess?.Invoke();
    }

    /// <summary>
    /// ゲームクリア終了処理
    /// </summary>
    private void endGameClear()
    {
        // Gui閉じる
        GuiManager.Instance.requestCloseGui(GuiManager.GuiType.GameClear);

        // ポーズ終了
        PauseManager.Instance.requestEndPause(this.GetType());
    }
    #region Request
    /// <summary>
    /// ゲームクリア開始リクエスト
    /// </summary>
    /// <param name="args"></param>
    public void requestStartGameClear(GameClearStartArgs args)
    {
        _GameClearStartRequest = args;
    }

    /// <summary>
    /// ゲームクリア終了リクエスト
    /// </summary>
    public void requestEndGameClear()
    {
        _GameClearEndRequest = true;
    }
    #endregion Request
    #endregion Method
}
