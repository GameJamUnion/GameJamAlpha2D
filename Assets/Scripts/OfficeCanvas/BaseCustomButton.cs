using UnityEngine;
using UnityEngine.UI;

public class BaseCustomButton : MonoBehaviour
{
    [SerializeField] Color _enterColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] Color _exitColor = new Color(0.8f, 0.8f, 0.8f, 1f);
    [SerializeField] Image _changeColorImage = null;

    virtual protected void Start()
    {
        PointExit();
    }

    virtual public void PointEnter()
    {
        _changeColorImage.color = _enterColor;
    }
    virtual public void PointExit()
    {
        _changeColorImage.color = _exitColor;
    }
    virtual public void PointDown()
    {

    }
}
