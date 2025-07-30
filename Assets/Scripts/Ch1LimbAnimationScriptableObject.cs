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
        [Header("�ړ��� (m)"), SerializeField]
        public float MoveValue = 1f;

        [Header("�ړ��J�[�u"), SerializeField]
        public AnimationCurve MoveCurve = new AnimationCurve();
    }

    [Serializable]
    public class VerticalMoveParam : MoveParams { }

    [Serializable]
    public class WaitingParam
    {
        [Header("���[�v���� (�b)"), SerializeField]
        public float LoopTime = 2f;

        [Header("�c�ړ�")]
        public VerticalMoveParam VerticalMove = new VerticalMoveParam();
    }
    #endregion Definition

    #region Property
    public WaitingParam Waiting
    {
        get => _Waiting;
        set => _Waiting = value;
    }
    [SerializeField, Header("�ʏ�ҋ@")]
    private WaitingParam _Waiting = new WaitingParam();
    #endregion


}
