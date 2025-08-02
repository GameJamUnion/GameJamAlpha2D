using System;
using UnityEngine;

/// <summary>
/// ��ƈ��z�u�ꏊ
/// </summary>
[Serializable]
public class UnitPlacement
{
    /// <summary>
    /// ���W
    /// </summary>
    [SerializeField]
    private Transform transform = null;

    /// <summary>
    /// �z�u����Ă����ƈ�ID
    /// </summary>
    private int placementOriginId = 0;

    /// <summary>
    /// �g�p�\��
    /// </summary>
    private bool available = true;

    #region Property

    /// <summary>
    /// ���W
    /// </summary>
    public Transform Transform
    {
        get { return transform; } 
    }

    /// <summary>
    /// �z�u����Ă����ƈ�ID
    /// </summary>
    public int PlacementOriginId
    {
        get { return placementOriginId; }
        set { placementOriginId = value; }
    }

    /// <summary>
    /// �g�p�\��
    /// </summary>
    public bool Available
    {
        get { return available; }
        set { available = value; }
    }

    #endregion
}