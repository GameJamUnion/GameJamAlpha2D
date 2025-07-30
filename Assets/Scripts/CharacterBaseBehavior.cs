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
    /// �X�e�[�^�X�ݒ�
    /// </summary>
    /// <param name="status"></param>
    public void setStatus(CharacterUnitStatus status)
    {
        UnitStatus |= status;
    }

    /// <summary>
    /// �X�e�[�^�X����
    /// </summary>
    /// <param name="status"></param>
    public void removeStatus(CharacterUnitStatus status)
    {
        UnitStatus &= ~status;
    }

    /// <summary>
    /// �X�e�[�^�X����
    /// </summary>
    /// <param name="status"></param>
    public bool checkStatus(CharacterUnitStatus status)
    {
        return (UnitStatus & status) != 0;
    }
}
