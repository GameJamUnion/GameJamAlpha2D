using UnityEngine;

public class StampCustomAnimation : MonoBehaviour
{
    StampState _stampState = StampState.NONE;

    [SerializeField] RectTransform _stamp;
    [SerializeField] float _defaultSize = 1.0f;
    [SerializeField] float _maxSize = 1.3f;
    [SerializeField] float _sizeChangeSpeed = 1f;

    private void Update()
    {
        float currentSize = 0f;

        switch (_stampState)
        {
            case StampState.NONE:
                break;

            case StampState.SIZE_UP:
                currentSize = _stamp.localScale.x + _sizeChangeSpeed * Time.deltaTime;
                currentSize = Mathf.Clamp(currentSize, _defaultSize, _maxSize);
                _stamp.localScale = new Vector3(currentSize, currentSize, currentSize);
                if(currentSize >= _maxSize)
                {
                    _stampState = StampState.SIZE_DOWN;
                }
                break;

            case StampState.SIZE_DOWN:
                currentSize = _stamp.localScale.x - _sizeChangeSpeed * Time.deltaTime;
                currentSize = Mathf.Clamp(currentSize, _defaultSize, _maxSize);
                _stamp.localScale = new Vector3(currentSize, currentSize, currentSize);
                if (currentSize <= _defaultSize)
                {
                    _stampState = StampState.NONE;
                }
                break;

            default:
                break;
        }
    }
    public void AnimationActive()
    {
        _stampState = StampState.SIZE_UP;
        _stamp.localScale = new Vector3(1f, 1f, 1f);
        _stamp.gameObject.SetActive(true);
    }
}

enum StampState
{
    NONE,
    SIZE_UP,
    SIZE_DOWN
}
