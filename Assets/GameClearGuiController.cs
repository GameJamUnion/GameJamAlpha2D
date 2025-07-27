using GuiUtil;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClearGuiController : GuiControllerBase
{
    public class  OpenParam : OpenParamBase
    {
        public int Score { get; set; } = 0;
        public float RemainingTime { get; set; } = 0.0f;
    }

    public override GuiManager.GuiType GuiType => GuiManager.GuiType.GameClear;

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
    #endregion Property

    #region Field
    private SelectWaitParam _SelectWaitParam = null;
    #endregion Field
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    /// <summary>
    /// GuiOpen時
    /// </summary>
    /// <param name="openParam"></param>
    public override void onOpen(OpenParamBase openParam = null)
    {
        base.onOpen(openParam);
        var param = openParam as OpenParam;
        setParam(param);        
    }
    
    /// <summary>
    /// スコア表示を設定
    /// </summary>
    /// <param name="param"></param>
    private void setParam(OpenParam param)
    {
        if (_ScoreObject != null)
        {
            _ScoreObject.text = param.Score.ToString();
        }

        if (_RemainingTimeObject != null)
        {
            _RemainingTimeObject.text = param.RemainingTime.ToString("F0") + "秒";
        }
    }

    /// <summary>
    /// GuiClose時
    /// </summary>
    public override void onClose()
    {
        base.onClose();

        _SelectPanel.SetActive(false);
        _SelectWaitParam = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// リスタートボタンクリック
    /// </summary>
    public void onRestartClick()
    {
        // リスタートリクエスト
        SceneManager.Instance.requestRestartInGame();

        // ゲームクリア画面終了
        GameClearManager.Instance.requestEndGameClear();
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

            //ゲームクリア画面終了
            GameClearManager.Instance.requestEndGameClear();
        }


        void onCancel()
        {
            // 前の画面に戻す
            _SelectPanel.SetActive(false);
            setActive(true);
            _SelectWaitParam = null;
        }

        _SelectWaitParam = new SelectWaitParam()
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
