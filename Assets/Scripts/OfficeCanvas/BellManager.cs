using UnityEngine;

public class BellManager : MonoBehaviour
{
    [Header("アタッチ")]
    [SerializeField] private GameObject _bell;
    [SerializeField] private GameObject _bellButton;
    [SerializeField] private RectTransform _bellButtonRect;

    [Header("Config")]
    [SerializeField] private float _bellButtonDownSpeed;
    [SerializeField] private float _bellButtonUpSpeed;
    [SerializeField] private float _stopBottomWaitTimer;

    [SerializeField] Vector3 _localBellButtonDownPos;
    [SerializeField] Vector3 _localBellButtonUpPos;

    Bell.ButtonAnimState _buttonAnimState;

    private float _currentTimer = 0f;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 newVec = _bellButtonRect.localPosition;

        switch (_buttonAnimState)
        {
            case Bell.ButtonAnimState.NONE:
                break;

            case Bell.ButtonAnimState.Down:
                newVec.y = newVec.y - _bellButtonDownSpeed;
                if(newVec.y <= _localBellButtonDownPos.y)
                {
                    newVec.y = _localBellButtonDownPos.y;
                    _buttonAnimState = Bell.ButtonAnimState.Wait;
                }
                _bellButtonRect.localPosition = newVec;
                break;

            case Bell.ButtonAnimState.Wait:
                _currentTimer += Time.deltaTime;
                if( _currentTimer >= _stopBottomWaitTimer)
                {
                    _currentTimer = 0f;
                    _buttonAnimState = Bell.ButtonAnimState.Up;
                }
                break;

            case Bell.ButtonAnimState.Up:
                newVec.y = newVec.y + _bellButtonUpSpeed;
                if (newVec.y >= _localBellButtonUpPos.y)
                {
                    newVec.y = _localBellButtonUpPos.y;
                    _buttonAnimState = Bell.ButtonAnimState.NONE;
                }
                _bellButtonRect.localPosition = newVec;
                break;
        }
    }

    // ボタンが押されたときに呼び出し
    public void PushButton()
    {
        if (_buttonAnimState == Bell.ButtonAnimState.Up || _buttonAnimState == Bell.ButtonAnimState.NONE)
            _buttonAnimState = Bell.ButtonAnimState.Down;
    }
}

namespace Bell
{
    public enum ButtonAnimState
    {
        NONE,
        Down,
        Wait,
        Up
    }
}