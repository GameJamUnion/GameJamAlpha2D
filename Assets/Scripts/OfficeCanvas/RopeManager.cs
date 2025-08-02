using UnityEngine;

public class RopeManager : MonoBehaviour
{
    [Header("アタッチ")]
    [SerializeField] GameObject _ropeObj;
    [SerializeField] RectTransform _ropeRec;

    [Header("Config")]
    [SerializeField] private float _ropeDownSpeed;
    [SerializeField] private float _ropeUpSpeed;
    [SerializeField] private float _stopRopeWaitTimer;

    [SerializeField] Vector3 _localRopeDownPos;
    [SerializeField] Vector3 _localRopeUpPos;

    private Rope.ButtonAnimState _buttonAnimState;

    private float _currentTimer = 0f;

    public Rope.ButtonAnimState ButtonAnimState
    {
        get { return _buttonAnimState; }
    }

    void Start()
    {
        
    }
    void Update()
    {
        Vector3 newVec = _ropeRec.localPosition;

        switch (_buttonAnimState)
        {
            case Rope.ButtonAnimState.NONE:
                break;

            case Rope.ButtonAnimState.Down:
                newVec.y = newVec.y - _ropeDownSpeed;
                if (newVec.y <= _localRopeDownPos.y)
                {
                    newVec.y = _localRopeDownPos.y;
                    _buttonAnimState = Rope.ButtonAnimState.Wait;
                    SoundManager.Instance.requestPlaySound(SEKind.Call);
                }
                _ropeRec.localPosition = newVec;
                break;

            case Rope.ButtonAnimState.Wait:
                _currentTimer += Time.deltaTime;
                if (_currentTimer >= _stopRopeWaitTimer)
                {
                    _currentTimer = 0f;
                    _buttonAnimState = Rope.ButtonAnimState.Up;
                }
                break;

            case Rope.ButtonAnimState.Up:
                newVec.y = newVec.y + _ropeUpSpeed;
                if (newVec.y >= _localRopeUpPos.y)
                {
                    newVec.y = _localRopeUpPos.y;
                    _buttonAnimState = Rope.ButtonAnimState.NONE;
                }
                _ropeRec.localPosition = newVec;
                break;
        }
    }

    // ボタンが押されたときに呼び出し
    public void PushButton()
    {
        if (_buttonAnimState == Rope.ButtonAnimState.Up || _buttonAnimState == Rope.ButtonAnimState.NONE)
            _buttonAnimState = Rope.ButtonAnimState.Down;
    }
}

namespace Rope
{
    public enum ButtonAnimState
    {
        NONE,
        Down,
        Wait,
        Up
    }
}
