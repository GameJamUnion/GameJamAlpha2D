using System;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "Ch1LimbAnimationScriptableObject", menuName = "Scriptable Objects/Ch1LimbAnimationScriptableObject")]
public class Ch1LimbAnimationScriptableObject : ScriptableObject
{
    #region Definition
    [Serializable]
    public class MoveParams
    {
        [Header("移動量 (m)"), SerializeField]
        public float MoveValue = 1f;

        [Header("移動カーブ"), SerializeField]
        public AnimationCurve MoveCurve = new AnimationCurve();
    }

    [Serializable]
    public class VerticalMoveParam : MoveParams { }

    [Serializable]
    public class WaitingParam
    {
        [Header("ループ時間 (秒)"), SerializeField]
        public float LoopTime = 2f;

        [Header("縦移動")]
        public VerticalMoveParam VerticalMove = new VerticalMoveParam();
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
    #endregion


}
