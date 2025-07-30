using System;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// �L�����N�^�[�̎l���𓮂����@�\Behavior
/// </summary>
public class Ch1LimbController : MonoBehaviour
{
    #region Definition
    [Serializable]
    public class LimbAnimatorParam
    {
        [Header("������Object")]
        public GameObject TargetObj;

        [Header("�p�����[�^")]
        public Ch1LimbAnimationScriptableObject ScriptableObject;
    }
    #endregion Definition

    #region Property
    [Header("��")]
    public LimbAnimatorParam HeadParam = new LimbAnimatorParam();

    [Header("��")]
    public LimbAnimatorParam BodyParam = new LimbAnimatorParam();

    [Header("��")]
    public LimbAnimatorParam LegParam = new LimbAnimatorParam();

    [Header("�E��")]
    public LimbAnimatorParam RightHandParam = new LimbAnimatorParam();

    [Header("����")]
    public LimbAnimatorParam LimbLeftHandParam = new LimbAnimatorParam();
    #endregion Property

    #region Field
    private Ch1LimbAnimator _HeadAnimator = new Ch1LimbAnimator();
    private Ch1LimbAnimator _BodyAnimator = new Ch1LimbAnimator();
    private Ch1LimbAnimator _LegAnimator = new Ch1LimbAnimator();
    private Ch1LimbAnimator _RightHandAnimator = new Ch1LimbAnimator();
    private Ch1LimbAnimator _LeftHandAnimator = new Ch1LimbAnimator();
    #endregion Field

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _HeadAnimator.setup(HeadParam);
        _BodyAnimator.setup(BodyParam);
        _LegAnimator.setup(LegParam);
        _RightHandAnimator.setup(RightHandParam);
        _LeftHandAnimator.setup(LimbLeftHandParam);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        _HeadAnimator.lateUpdate();
        _BodyAnimator.lateUpdate();
        _LegAnimator.lateUpdate();
        _RightHandAnimator.lateUpdate();
        _LeftHandAnimator.lateUpdate();
    }
}
