using UnityEngine;
using UnityEngine.UI;

public class BaseCustomButton : MonoBehaviour
{
    [SerializeField] Color _enterColor = new Color();
    [SerializeField] Color _exitColor = new Color();
    [SerializeField] Image _changeColorImage = null;

    void Start()
    {
        PointExit();
    }

    void Update()
    {
        
    }

    public void PointEnter()
    {
        _changeColorImage.color = _enterColor;
    }
    public void PointExit()
    {
        _changeColorImage.color = _exitColor;
    }
    public void PointDown()
    {

    }
}
