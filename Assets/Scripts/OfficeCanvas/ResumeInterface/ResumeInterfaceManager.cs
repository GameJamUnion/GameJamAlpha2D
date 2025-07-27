using UnityEngine;
using System.Collections.Generic;

public class ResumeInterfaceManager : MonoBehaviour
{
    [SerializeField] private List<ResumeInterface> _resumeInterfaces;

    #region プロパティ
    public List<ResumeInterface> ResumeInterfaces
    {
        get { return _resumeInterfaces; }
    }
    public bool AssignInactiveResumeInterface
    {
        get
        {
            foreach (var item in _resumeInterfaces)
            {
                if (!item.Active)
                {
                    return true;
                }
            }
            return false;
        }
    }
    #endregion

    public void Initialize()
    {

    }

    public ResumeInterface GetInactiveResumeInterface()
    {
        for (int i = 0; i < _resumeInterfaces.Count; i++)
        {
            if (!_resumeInterfaces[i].Active)
            {
                return _resumeInterfaces[i];
            }
        }

        Debug.Log("ResumeInterfaceの数が足りないYO");
        return null;
    }
}
