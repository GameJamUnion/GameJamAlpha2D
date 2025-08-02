using System;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// キャラクターの四肢の移動の調整パラメータ
/// </summary>
[CreateAssetMenu(fileName = "Ch1LimbAnimationScriptableObject", menuName = "Scriptable Objects/Ch1LimbAnimationScriptableObject")]
public class Ch1LimbAnimationScriptableObject : ScriptableObject
{
    #region Definition
    [Serializable]
    public class MoveParams<T>
    {
        [Header("有効")]
        public bool Enable = true;

        [Header("移動量 (m or 度)"), SerializeField]
        public T MoveValue = default;

        [Header("移動カーブ"), SerializeField]
        public AnimationCurve MoveCurve = new AnimationCurve();
    }

    [Serializable]
    public class RotationMoveParam : MoveParams<Vector3> { }

    [Serializable]
    public class VerticalMoveParam : MoveParams<float> { }

    /// <summary>
    /// 待機中
    /// </summary>
    [Serializable]
    public class WaitingParam
    {
        [Header("ループ時間 (秒)"), SerializeField]
        public float LoopTime = 2f;

        [Header("縦移動")]
        public VerticalMoveParam VerticalMove = new VerticalMoveParam();
    }

    /// <summary>
    /// 休憩中
    /// </summary>
    [Serializable]
    public class RestParam
    {
        [Header("ループ時間")]
        public float LoopTime = 2f;

        [Header("縦移動")]
        public VerticalMoveParam VerticalMove = new VerticalMoveParam();

        [Header("回転")]
        public RotationMoveParam RotationMove = new RotationMoveParam();
    }
    #endregion Definition

    #region Property
    public WaitingParam Waiting
    {
        get => _Waiting;
        set => _Waiting = value;
    }
    [SerializeField, Header("通常待機")]
    private WaitingParam _Waiting = new WaitingParam();

    
    public RestParam Rest
    {
        get => _Rest;
        set => _Rest = value;
    }
    [SerializeField, Header("休憩状態")]
    private RestParam _Rest = new RestParam();
    #endregion


}
