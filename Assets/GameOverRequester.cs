using UnityEngine;

public class GameOverRequester : MonoBehaviour
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

    #endregion Property
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (WorkTime != null)
        {
            WorkTime.RegisterTimeupEvent(startGameOver);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Method
    [ContextMenu("ゲームオーバー開始")]
    /// <summary>
    /// ゲームオーバー開始イベント
    /// </summary>
    private void startGameOver()
    {
        GameOverManager.Instance.requestStartGameOver(new GameOverManager.GameOverStartArgs()
        {
            
        });
    }
    #endregion Method
}
