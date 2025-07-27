using NUnit.Framework.Interfaces;
using UnityEngine;

public class DustBox : MonoBehaviour
{
    [SerializeField] ResumeInterface _chooseResume;
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;

    public void PointEnter()
    {
        foreach (var item in _resumeInterfaceManager.ResumeInterfaces)
        {
            if (item.HoldFlg)
            {
                _chooseResume = item;
            }
        }
    }
    public void PointExit()
    {
        _chooseResume = null;
    }
    public void Drop()
    {
        if (_chooseResume != null)
            _chooseResume.Rejected();
    }
}
