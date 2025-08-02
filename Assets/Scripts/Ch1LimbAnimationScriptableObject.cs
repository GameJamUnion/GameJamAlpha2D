using System;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎l���̈ړ��̒����p�����[�^
/// </summary>
[CreateAssetMenu(fileName = "Ch1LimbAnimationScriptableObject", menuName = "Scriptable Objects/Ch1LimbAnimationScriptableObject")]
public class Ch1LimbAnimationScriptableObject : ScriptableObject
{
    #region Definition
    [Serializable]
    public class MoveParams<T>
    {
        [Header("�L��")]
        public bool Enable = true;

        [Header("�ړ��� (m or �x)"), SerializeField]
        public T MoveValue = default;

        [Header("�ړ��J�[�u"), SerializeField]
        public AnimationCurve MoveCurve = new AnimationCurve();
    }

    [Serializable]
    public class RotationMoveParam : MoveParams<Vector3> { }

    [Serializable]
    public class VerticalMoveParam : MoveParams<float> { }

    /// <summary>
    /// �ҋ@��
    /// </summary>
    [Serializable]
    public class WaitingParam
    {
        [Header("���[�v���� (�b)"), SerializeField]
        public float LoopTime = 2f;

        [Header("�c�ړ�")]
        public VerticalMoveParam VerticalMove = new VerticalMoveParam();
    }

    /// <summary>
    /// �x�e��
    /// </summary>
    [Serializable]
    public class RestParam
    {
        [Header("���[�v����")]
        public float LoopTime = 2f;

        [Header("�c�ړ�")]
        public VerticalMoveParam VerticalMove = new VerticalMoveParam();

        [Header("��]")]
        public RotationMoveParam RotationMove = new RotationMoveParam();
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

    
    public RestParam Rest
    {
        get => _Rest;
        set => _Rest = value;
    }
    [SerializeField, Header("�x�e���")]
    private RestParam _Rest = new RestParam();
    #endregion


}
