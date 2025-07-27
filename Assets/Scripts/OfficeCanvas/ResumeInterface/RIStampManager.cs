using UnityEngine;

public class RIStampManager : MonoBehaviour
{
    [SerializeField] RI.StampState _stampState;

    [SerializeField] GameObject _tAccepted1;
    [SerializeField] GameObject _tRejected2;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ChnageStampState(RI.StampState newState)
    {
        _stampState = newState;

        switch (_stampState)
        {
            case RI.StampState.NONE:
                _tAccepted1.SetActive(false);
                _tRejected2.SetActive(false);
                break;

            case RI.StampState.Accepted:
                _tAccepted1.SetActive(true);
                _tRejected2.SetActive(false);
                break;

            case RI.StampState.Rejected:
                _tAccepted1.SetActive(false);
                _tRejected2.SetActive(true);
                break;
        }
    }
}

namespace RI
{
    public enum StampState
    {
        NONE,
        Accepted,
        Rejected
    }
}
