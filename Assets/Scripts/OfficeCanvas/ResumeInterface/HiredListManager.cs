using UnityEngine;

public class HiredListManager : MonoBehaviour
{
    [SerializeField] OfficeGameMaster _officeGameMaster;
    [SerializeField] ResumeInterface _hiredList;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void SetDefault()
    {
        _hiredList.SetDefault();
    }
    // Update
    public void HiredListUpdate()
    {
        if (_officeGameMaster.SelectUnit != null)
            _hiredList.Initialize(_officeGameMaster.SelectUnit.MyResume);
    }
}
