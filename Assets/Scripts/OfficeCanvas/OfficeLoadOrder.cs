using UnityEngine;

public class OfficeLoadOrder : MonoBehaviour
{
    [SerializeField] ResumeInterfaceManager _resumeInterfaceManager;
    [SerializeField] OfficeGameMaster _officeGameMaster;

    void Start()
    {
        _resumeInterfaceManager.Initialize();
        _officeGameMaster.Initialize();
    }

    void Update()
    {
        
    }
}
