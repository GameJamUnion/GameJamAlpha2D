using UnityEngine;
using System.Collections.Generic;

public enum PauseType
{
    Invalid,

    InGamePause,

}

public class PauseManager : SingletonBase<PauseManager>
{
    public class PauseRequestArgs
    {
        public GameObject Owner;
        public PauseType Type;
    }

    private PauseFunction_InGamePause _InGamePauseFunc = new PauseFunction_InGamePause();

    private List<PauseRequestArgs> _PauseRequest = new List<PauseRequestArgs>(4);

    public override void LateUpdate()
    {
        base.LateUpdate();

        updatePause();
    }

    /// <summary>
    /// ポーズ処理更新
    /// </summary>
    private void updatePause()
    {
        var inGamePause = false;
        lock (_PauseRequest)
        {
            var pauseRequestCount = _PauseRequest.Count;
            if (_PauseRequest.Count != 0)
            {
                for (int i = 0; i < pauseRequestCount; i++)
                {
                    var request = _PauseRequest[i];
                    switch (request.Type)
                    {
                        case PauseType.InGamePause:
                            if (_InGamePauseFunc.Active == false)
                            {
                                _InGamePauseFunc.onStartPause();
                            }
                            inGamePause = true;
                            break;
                        default:
                            Debug.LogError($"Unknown PauseType: {request.Type}");
                            break;
                    }
                }
            }
        }

        if (inGamePause == false)
        {
            _InGamePauseFunc.onEndPause();
        }
    }

    /// <summary>
    /// ポーズがかかっているか
    /// </summary>
    /// <returns></returns>
    public bool checkPauseAny()
    {
        lock (_PauseRequest)
        {
            return _PauseRequest.Count > 0;
        }
    }
    #region Request
    /// <summary>
    /// ポーズ開始リクエスト
    /// </summary>
    /// <param name="args"></param>
    public void requestStartPause(PauseRequestArgs args)
    {
        lock (_PauseRequest)
        {
            _PauseRequest.Add(args);
        }
    }

    /// <summary>
    /// ポーズ終了リクエスト
    /// </summary>
    /// <param name="owner"></param>
    public void requestEndPause(GameObject owner)
    {
        lock(_PauseRequest)
        {
            _PauseRequest.RemoveAll(request => request.Owner == owner);
        }
    }
    #endregion Request
}

#region Pause処理
public abstract class PauseFunctionBase
{
    public bool Active { get; protected set; } = false;

    /// <summary>
    /// ポーズ開始処理
    /// </summary>
    public virtual void onStartPause()
    {
        Active = true;
    }
    /// <summary>
    /// ポーズ終了処理
    /// </summary>
    public virtual void onEndPause()
    {
        Active = false;
    }
}

public class PauseFunction_InGamePause : PauseFunctionBase
{
    public override void onStartPause()
    {
        base.onStartPause();

        Time.timeScale = 0f; // ゲームの時間を停止
    }

    public override void onEndPause()
    {
        base.onEndPause();

        Time.timeScale = 1f; // ゲームの時間を再開
    }
}
#endregion