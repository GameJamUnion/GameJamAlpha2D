using System;
using UnityEngine;

public class GameOverManager : SingletonBase<GameOverManager>
{
    #region Definition
    public class GameOverStartArgs
    {
        // 表示するスコアなど
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
    private GameOverStartArgs _GameOverStartRequest = null;
    private bool _GameOverEndRequest = false;
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
        // ゲームオーバー終了
        if (_GameOverEndRequest == true)
        {
            endGameOver();
            _GameOverEndRequest = false;

            // 終了リクエストを優先させるため開始リクエストは終了させる
            _GameOverStartRequest = null;
            return;
        }


        // ゲームオーバー開始
        if (_GameOverStartRequest != null)
        {
            startGameOver(_GameOverStartRequest);
            _GameOverStartRequest = null;
        }

    }
    #endregion 更新処理
    /// <summary>
    /// ゲームオーバー開始処理
    /// </summary>
    /// <param name="startArgs"></param>
    private void startGameOver(GameOverStartArgs startArgs)
    {
        // ポーズかける
        PauseManager.Instance.requestStartPause(new PauseManager.PauseRequestArgs()
        {
            Owner = this.GetType(),
            Type = PauseType.InGamePause,
        });

        // Gui開く
        GuiManager.Instance.requestOpenGui(GuiManager.GuiType.GameOver, new GameOverGuiController.OpenParam()
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
    /// ゲームオーバー終了処理
    /// </summary>
    private void endGameOver()
    {
        // Gui閉じる
        GuiManager.Instance.requestCloseGui(GuiManager.GuiType.GameOver);

        // ポーズ終了
        PauseManager.Instance.requestEndPause(this.GetType());

    }

    #region Request
    /// <summary>
    /// ゲームオーバー開始リクエスト
    /// </summary>
    /// <param name="args"></param>
    public void requestStartGameOver(GameOverStartArgs args)
    {
        _GameOverStartRequest = args;
    }

    /// <summary>
    /// ゲームオーバー終了リクエスト
    /// </summary>
    public void requestEndGameOver()
    {
        _GameOverEndRequest = true;
    }
    #endregion Request
    #endregion Method

}
