using System;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// キャラクターの四肢を動かす機能Behavior
/// </summary>
public class Ch1LimbController : MonoBehaviour
{
    #region Definition
    [Serializable]
    public class LimbAnimatorParam
    {
        [Header("動かすObject")]
        public GameObject TargetObj;

        [Header("パラメータ")]
        public Ch1LimbAnimationScriptableObject ScriptableObject;
    }
    #endregion Definition

    #region Property
    [Header("頭")]
    public LimbAnimatorParam HeadParam = new LimbAnimatorParam();

    [Header("体")]
    public LimbAnimatorParam BodyParam = new LimbAnimatorParam();

    [Header("足")]
    public LimbAnimatorParam LegParam = new LimbAnimatorParam();

    [Header("右手")]
    public LimbAnimatorParam RightHandParam = new LimbAnimatorParam();

    [Header("左手")]
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
