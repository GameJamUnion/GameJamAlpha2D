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
    [Header("�S�g")]
    public LimbAnimatorParam FullBodyParam = new LimbAnimatorParam();

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
    private Ch1LimbAnimator _FullBodyAnimator = null;
    private Ch1LimbAnimator _HeadAnimator = null;
    private Ch1LimbAnimator _BodyAnimator = null;
    private Ch1LimbAnimator _LegAnimator = null;
    private Ch1LimbAnimator _RightHandAnimator = null;
    private Ch1LimbAnimator _LeftHandAnimator = null;

    private Ch1CharacterBehavior _Ch1CharacterBehavior = null;
    #endregion Field

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _FullBodyAnimator = new Ch1LimbAnimator(this);
        _HeadAnimator = new Ch1LimbAnimator(this);
        _BodyAnimator = new Ch1LimbAnimator(this);
        _LegAnimator = new Ch1LimbAnimator(this);
        _RightHandAnimator = new Ch1LimbAnimator(this);
        _LeftHandAnimator = new Ch1LimbAnimator(this);

        _FullBodyAnimator.setup(FullBodyParam);
        _HeadAnimator.setup(HeadParam);
        _BodyAnimator.setup(BodyParam);
        _LegAnimator.setup(LegParam);
        _RightHandAnimator.setup(RightHandParam);
        _LeftHandAnimator.setup(LimbLeftHandParam);

        TryGetComponent<Ch1CharacterBehavior>(out _Ch1CharacterBehavior);
    }

    // Update is called once per frame
    void Update()
    {
        if (_Ch1CharacterBehavior == null)
        {
            return;
        }

        if (_Ch1CharacterBehavior.checkStatus(CharacterUnitStatus.Rest) == true)
        {
            changeAnimationType(Ch1LimbAnimator.AnimationTypes.Rest);
        }
        else if (_Ch1CharacterBehavior.checkStatus(CharacterUnitStatus.Working) == true)
        {
            changeAnimationType(Ch1LimbAnimator.AnimationTypes.Waiting);
        }
    }

    private void LateUpdate()
    {
        _FullBodyAnimator.lateUpdate();
        _HeadAnimator.lateUpdate();
        _BodyAnimator.lateUpdate();
        _LegAnimator.lateUpdate();
        _RightHandAnimator.lateUpdate();
        _LeftHandAnimator.lateUpdate();
    }

    /// <summary>
    /// �e�l���̓����Ԃ�ݒ�
    /// </summary>
    /// <param name="type"></param>
    private void changeAnimationType(Ch1LimbAnimator.AnimationTypes type)
    {
        _FullBodyAnimator.setAnimationType(type);
        _HeadAnimator.setAnimationType(type);
        _BodyAnimator.setAnimationType(type);
        _LegAnimator.setAnimationType(type);
        _RightHandAnimator.setAnimationType(type);
        _LeftHandAnimator.setAnimationType(type);
    }
}
