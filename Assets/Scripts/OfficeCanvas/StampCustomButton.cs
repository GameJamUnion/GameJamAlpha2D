using UnityEngine;

public class StampCustomButton : BaseCustomButton
{
    [SerializeField] ResumeInterface _resumeInterface;
    [SerializeField] StampCustomAnimation _stampAnimation;

    protected override void Start()
    {
        base.Start();
    }
    public override void PointEnter()
    {
        if (_resumeInterface.BaseResume == null)
            return;

        if (!_resumeInterface.BaseResume.OnStamp)
            base.PointEnter();
    }
    public override void PointExit()
    {
        if (_resumeInterface.BaseResume == null)
            return;

        if (!_resumeInterface.BaseResume.OnStamp)
            base.PointExit();
    }
    public override void PointDown()
    {
        if (!_resumeInterface.BaseResume.OnStamp)
        {
            _resumeInterface.OnStamp();
            _stampAnimation.AnimationActive();
            base.PointExit();
        }
    }
}
