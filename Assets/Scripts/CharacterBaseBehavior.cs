using UnityEngine;

public class CharacterBaseBehavior : MonoBehaviour
{
    #region Property
    public CharacterUnitStatus UnitStatus { get ; private set; } 
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ステータス設定
    /// </summary>
    /// <param name="status"></param>
    public void setStatus(CharacterUnitStatus status)
    {
        UnitStatus |= status;
    }

    /// <summary>
    /// ステータス解除
    /// </summary>
    /// <param name="status"></param>
    public void removeStatus(CharacterUnitStatus status)
    {
        UnitStatus &= ~status;
    }

    /// <summary>
    /// ステータス判定
    /// </summary>
    /// <param name="status"></param>
    public bool checkStatus(CharacterUnitStatus status)
    {
        return (UnitStatus & status) != 0;
    }
}
