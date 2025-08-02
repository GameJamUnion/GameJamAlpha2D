using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎l���𓮂����N���X
/// </summary>
public class Ch1LimbAnimator
{
    public Ch1LimbAnimator(Ch1LimbController controller)
    {
        _Controller = controller;
    }

    #region Definition
    public enum AnimationTypes
    {
        /// <summary>
        /// �ʏ�ҋ@
        /// </summary>
        Waiting,

        /// <summary>
        /// �x�e��
        /// </summary>
        Rest,
    }
    #endregion
    #region Property
    public AnimationTypes CurrentType
    {
        get => _CurrentPhase;
        set => _CurrentPhase = value;
    }      
    private AnimationTypes _CurrentPhase = AnimationTypes.Waiting;
    #endregion Property
    #region Field
    private GameObject _TargetObj = null;
    private Ch1LimbAnimationScriptableObject _Data = null;
    private Ch1LimbController _Controller = null;
    private Ch1CharacterBehavior _CharacterBehavior = null;

    // ��{�̈ʒu
    private Vector3 _BasePosition = Vector3.zero;
    private Quaternion _BaseRotation = Quaternion.identity;

    // �O��̃��[�v����̌o�ߎ���
    private float _ElapsedTime = 0f;
    #endregion Field
    #region Method
    /// <summary>
    /// ������
    /// </summary>
    /// <param name="target"></param>
    public void setup(Ch1LimbController.LimbAnimatorParam targetParam)
    {
        _TargetObj = targetParam.TargetObj;
        if (_TargetObj == null)
        {
            return;
        }

        _BasePosition = _TargetObj.transform.localPosition;
        _BaseRotation = _TargetObj.transform.localRotation;
        _Data = targetParam.ScriptableObject;
    }

    public void lateUpdate()
    {
        if (_TargetObj == null || _Data == null)
        {
            return;
        }

        var time = Time.deltaTime;

        _ElapsedTime += time;
        switch (CurrentType)
        {
            case AnimationTypes.Waiting:
                updateWaiting();
                break;
            case AnimationTypes.Rest:
                updateRest();
                break;
            default:
                break;
        }
    }

    private void changePhase(AnimationTypes type)
    {
        if (type == CurrentType)
        {
            return;
        }

        reset();

        CurrentType = type;
    }

    /// <summary>
    /// ������Ԃɖ߂�
    /// </summary>
    private void reset()
    {
        setLocalPosition(_BasePosition);
        setLocalRotation(_BaseRotation);
        _ElapsedTime = 0f;
    }

    /// <summary>
    /// �O������AnimationType��ύX����
    /// </summary>
    /// <param name="type"></param>
    public void setAnimationType(AnimationTypes type)
    {
        changePhase(type);
    }
    #region update
    /// <summary>
    /// �ʏ�ҋ@�X�V
    /// </summary>
    private void updateWaiting()
    {
        var data = _Data.Waiting;

        // �c�ړ�
        if (data.VerticalMove.Enable == true)
        {
            moveVertical(data.VerticalMove, data.LoopTime);
        }



        if (_ElapsedTime > data.LoopTime)
        {
            _ElapsedTime = 0f;
        }
    }

    /// <summary>
    /// �x�e���X�V
    /// </summary>
    private void updateRest()
    {
        var data = _Data.Rest;

        if (data.VerticalMove.Enable == true)
        {
            moveVertical(data.VerticalMove, data.LoopTime);
        }

        if (data.RotationMove.Enable == true)
        {
            moveRotation(data.RotationMove, data.LoopTime);
        }


        if (_ElapsedTime > data.LoopTime)
        {
            _ElapsedTime = 0f;
        }
    }
    #endregion update
    #region Move
    /// <summary>
    /// �c�����̈ړ�
    /// </summary>
    /// <param name="param"></param>
    /// <param name="loopTime"></param>
    private void moveVertical(Ch1LimbAnimationScriptableObject.VerticalMoveParam param, float loopTime)
    {
        var timeRate = _ElapsedTime / loopTime;
        var interpolateValue = param.MoveCurve.Evaluate(timeRate);
        var addPosY = interpolateValue * param.MoveValue;

        // �ړ�
        setLocalPosition(_BasePosition + new Vector3(0f, addPosY, 0f));
    }

    /// <summary>
    /// ��]
    /// </summary>
    /// <param name="param"></param>
    /// <param name="loopTime"></param>
    private void moveRotation(Ch1LimbAnimationScriptableObject.RotationMoveParam param, float loopTime)
    {
        var timeRate = _ElapsedTime / loopTime;
        var interpolateValue = param.MoveCurve.Evaluate(timeRate);
        var rot = interpolateValue * param.MoveValue;

        // ��]
        var rad = rot;
        setLocalRotation(rad);
    }
    #endregion Move


    /// <summary>
    /// �Ώۂ�LocalPosition��ύX����
    /// ���s������ǂ����߂Ƀ��b�s���O
    /// </summary>
    /// <param name="localPos"></param>
    private void setLocalPosition(Vector3 localPos)
    {
        _TargetObj.transform.localPosition = localPos;
    }

    /// <summary>
    /// �Ώۂ�LocalRotation��ύX����
    /// </summary>
    /// <param name="localRot"></param>
    private void setLocalRotation(Vector3 localRot)
    {
        _TargetObj.transform.localRotation = Quaternion.Euler(localRot);
    }
    private void setLocalRotation(Quaternion localRot)
    {
        _TargetObj.transform.localRotation = localRot;
    }
    #endregion
}
