using System.Runtime.Serialization.Json;
using UnityEngine;

/// <summary>
/// ポーズ画面開くリクエストを送るオブジェクト
/// </summary>
public class PauseRequester : MonoBehaviour
{
    private bool _Paused = false;
    private bool _RequestStart = false;
    private bool _RequestEnd = false;
    // Update is called once per frame
    void Update()
    {
        if (checkEnablePause() == false)
        {
            // ポーズ不可
            return;
        }

        if (checkPauseInput() == true)
        {
            if (PauseManager.Instance.checkPauseAny() == true)
            {
                requestEndPause();
            }
            else
            {
                requestStartPause();
            }
        }

    }

    private void LateUpdate()
    {
        // リクエスト処理
        if (_RequestEnd == true)
        {
            endPause();
        }

        if (_RequestStart == true)
        {
            startPause();
        }

        _RequestStart = false;
        _RequestEnd = false;
    }

    private void OnDestroy()
    {
        endPause();
    }

    /// <summary>
    /// ポーズ開始処理
    /// </summary>
    public void startPause()
    {
        if (_Paused == true)
        {
            // 既にポーズリクエスト済み
            return;
        }
        _Paused = true;

        // ポーズ開始
        PauseManager.Instance.requestStartPause(new PauseManager.PauseRequestArgs()
        {
            Owner = this.GetType(),
            Type = PauseType.InGamePause,
        });

        // GUI開く
        GuiManager.Instance.requestOpenGui(GuiManager.GuiType.Pause, new OpenParamBase() { });
    }

    /// <summary>
    /// ポーズ終了処理
    /// </summary>
    public void endPause()
    {
        if (_Paused == false)
        {
            // ポーズリクエストしていない
            return;
        }
        _Paused = false;

        // GUI閉じる
        GuiManager.Instance.requestCloseGui(GuiManager.GuiType.Pause);

        // ポーズ終了
        PauseManager.Instance.requestEndPause(this.GetType());
    }

    /// <summary>
    /// ポーズ可能状態か
    /// </summary>
    /// <returns></returns>
    private bool checkEnablePause()
    {
        if (SceneManager.Instance.checkInGameScene() == false)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// ポーズキー入力をチェック
    /// </summary>
    /// <returns></returns>
    private bool checkPauseInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// ポーズ開始リクエスト
    /// </summary>
    public void requestStartPause()
    {
        _RequestStart = true;
    }

    /// <summary>
    /// ポーズ終了リクエスト
    /// </summary>
    public void requestEndPause()
    {
        _RequestEnd = true;
    }
}
