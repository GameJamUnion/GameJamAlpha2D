using System;
using UnityEngine;

/// <summary>
/// 作業員配置場所
/// </summary>
[Serializable]
public class UnitPlacement
{
    /// <summary>
    /// 座標
    /// </summary>
    [SerializeField]
    private Transform transform = null;

    /// <summary>
    /// 配置されている作業員ID
    /// </summary>
    private int placementOriginId = 0;

    /// <summary>
    /// 使用可能か
    /// </summary>
    private bool available = true;

    #region Property

    /// <summary>
    /// 座標
    /// </summary>
    public Transform Transform
    {
        get { return transform; } 
    }

    /// <summary>
    /// 配置されている作業員ID
    /// </summary>
    public int PlacementOriginId
    {
        get { return placementOriginId; }
        set { placementOriginId = value; }
    }

    /// <summary>
    /// 使用可能か
    /// </summary>
    public bool Available
    {
        get { return available; }
        set { available = value; }
    }

    #endregion
}