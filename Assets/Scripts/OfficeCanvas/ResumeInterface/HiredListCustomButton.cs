using UnityEngine;

public class HiredListCustomButton : MonoBehaviour
{
    [SerializeField] ResumeInterface _chooseResume;
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;
    [SerializeField] OfficeGameMaster officeGameMaster;

    [SerializeField] GameObject _hiredListObject;
    [SerializeField] GameObject _hiredListHitBoxObject;

    protected void Start()
    {

    }

    private void Update()
    {
        _hiredListHitBoxObject.GetComponent<RectTransform>().transform.position = _hiredListObject.GetComponent<RectTransform>().transform.position;
    }
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
            if (_chooseResume.BaseResume.OnStamp)
                officeGameMaster.Employment(_chooseResume.BaseResume.BaseUnit);
    }
}
