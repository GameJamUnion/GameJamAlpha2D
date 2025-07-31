using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎l���𓮂����N���X
/// </summary>
public class Ch1LimbAnimator
{
    #region Definition
    public enum AnimationTypes
    {
        /// <summary>
        /// �ʏ�ҋ@
        /// </summary>
        Waiting,
    }
    #endregion
    #region Property
    public AnimationTypes CurrentPhase
    {
        get => _CurrentPhase;
        set => _CurrentPhase = value;
    }      
    private AnimationTypes _CurrentPhase = AnimationTypes.Waiting;
    #endregion Property
    #region Field
    private GameObject _TargetObj = null;
    private Ch1LimbAnimationScriptableObject _Data = null;

    // ��{�̈ʒu
    private Vector3 _BasePosition = Vector3.zero;

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
        switch (CurrentPhase)
        {
            case AnimationTypes.Waiting:
                updateWaiting();
                break;
            default:
                break;
        }
    }

    #region update
    /// <summary>
    /// �ʏ�ҋ@�X�V
    /// </summary>
    private void updateWaiting()
    {
        var data = _Data.Waiting;

        // �c�ړ�
        {
            var timeRate = _ElapsedTime / data.LoopTime;
            var interpolateValue = data.VerticalMove.MoveCurve.Evaluate(timeRate);
            var addPosY = interpolateValue * data.VerticalMove.MoveValue;

            // �ړ�
            _TargetObj.transform.localPosition = _BasePosition + new Vector3(0f, addPosY, 0f);
        }



        if (_ElapsedTime > data.LoopTime)
        {
            _ElapsedTime = 0f;
        }
    }
    #endregion update
    #endregion
}
