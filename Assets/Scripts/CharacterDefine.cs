using UnityEngine;

public class CharacterDefine
{
    
}

/// <summary>
/// キャラクターの状態
/// </summary>
public enum CharacterUnitStatus: uint
{
    Working = 1 << 0,
    Rest = 1 << 1,
}
