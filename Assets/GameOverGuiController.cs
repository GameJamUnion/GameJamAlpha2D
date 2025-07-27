using GuiUtil;
using System;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverGuiController : GuiControllerBase
{


    public class OpenParam : OpenParamBase
    {
        public int Score { get; set; } = 0;
        public float RemainingTime { get; set; } = 0.0f;
    }

    public override GuiManager.GuiType GuiType => GuiManager.GuiType.GameOver;

    #region Property
    [DisplayName("はい/いいえ選択パネル")]
    public GameObject SelectPanel
    {
        get => _SelectPanel;
        set => _SelectPanel = value;
    }
    [SerializeField]
    private GameObject _SelectPanel = null;

    [DisplayName("スコア表示オブジェクト")]
    public TextMeshProUGUI ScoreObject
    {
        get => _ScoreObject;
        set => _ScoreObject = value;
    }
    [SerializeField]
    private TextMeshProUGUI _ScoreObject = null;

    [DisplayName("残り時間表示オブジェクト")]
    public TextMeshProUGUI RemainingTimeObject
    {
        get => _RemainingTimeObject;
        set => _RemainingTimeObject = value;
    }
    [SerializeField]
    private TextMeshProUGUI _RemainingTimeObject = null;
    #endregion

    #region Field
    private SelectWaitParam _SelectWaitParam = null;
    #endregion
    public override void onOpen(OpenParamBase openParam = null)
    {
        base.onOpen(openParam);
        var param = openParam as OpenParam;
        setParam(param);
    }

    /// <summary>
    /// 結果報告表示設定
    /// </summary>
    /// <param name="param"></param>
    private void setParam(OpenParam param)
    {
        if (ScoreObject != null)
        {
            ScoreObject.text = param.Score.ToString();
        }

        if (RemainingTimeObject != null)
        {
            RemainingTimeObject.text = param.RemainingTime.ToString("F0") + "秒";
        }
    }

    public override void onClose()
    {
        base.onClose();

        _SelectPanel.SetActive(false);
        _SelectWaitParam = null;
    }

    /// <summary>
    /// リスタートボタンクリック
    /// </summary>
    public void onRestartClick()
    {
        // リスタートリクエスト
        SceneManager.Instance.requestRestartInGame();

        // ゲームオーバー画面終了
        GameOverManager.Instance.requestEndGameOver();
    }

    /// <summary>
    /// タイトルへ戻るボタンクリック
    /// </summary>
    public void onToTitleClick()
    {
        if (_SelectPanel == null)
        {
            Debug.LogError("SelectPanel が null");
            return;
        }

        void onDecide()
        {
            // タイトルへ戻るリクエスト
            SceneManager.Instance.requestToTitle();

            // ゲームオーバー画面終了
            GameOverManager.Instance.requestEndGameOver();
        }

        void onCancel()
        {
            // 前の画面に戻す
            _SelectPanel.SetActive(false);
            setActive(true);
            _SelectWaitParam = null;
        }

        _SelectWaitParam = new SelectWaitParam
        {
            onDecide = onDecide,
            onCancel = onCancel
        };

        _SelectPanel.SetActive(true);
    }

    /// <summary>
    /// 「はい」ボタンクリック
    /// </summary>
    public void onDecideClick()
    {
        if (_SelectWaitParam != null)
        {
            _SelectWaitParam.onDecide?.Invoke();
        }
    }

    /// <summary>
    /// 「いいえ」ボタンクリック
    /// </summary>
    public void onCancelClick()
    {
        if (_SelectWaitParam != null)
        {
            _SelectWaitParam.onCancel?.Invoke();
        }
    }
}
