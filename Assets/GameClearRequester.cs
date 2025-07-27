using UnityEngine;

public class GameClearRequester : MonoBehaviour
{
    #region Property
    public WorkTime WorkTime
    {
        get => _WorkTime;
        set => _WorkTime = value;
    }
    [SerializeField]
    private WorkTime _WorkTime = null;

    public WorkScore WorkScore
    {
        get => _WorkScore;
        set => _WorkScore = value;
    }
    [SerializeField]
    private WorkScore _WorkScore = null;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (WorkScore != null)
        {
            //WorkScore
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Method
    [ContextMenu("ゲームクリア開始")]
    /// <summary>
    /// ゲームクリア開始イベント
    /// </summary>
    private void startGameClear()
    {
        GameClearManager.Instance.requestStartGameClear(new GameClearManager.GameClearStartArgs()
        {
            
        });
    }
    #endregion Method
}
