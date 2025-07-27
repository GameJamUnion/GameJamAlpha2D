using UnityEngine;
using System.Collections.Generic;

public class ResumeInterfaceManager : MonoBehaviour
{
    [SerializeField] private List<ResumeInterface> _resumeInterfaces;

    #region �v���p�e�B
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

        Debug.Log("ResumeInterface�̐�������Ȃ�YO");
        return null;
    }
}
