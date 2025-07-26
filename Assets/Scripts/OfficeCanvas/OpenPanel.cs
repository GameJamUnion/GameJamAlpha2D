using UnityEngine;
using System.Collections.Generic;

public class OpenPanel : MonoBehaviour
{
    [SerializeField] GameObject _openObject;
    [SerializeField] bool _open = true;
    [SerializeField] List<BaseCustomButton> _baseCustomButtons;

    void Start()
    {

    }

    public void Intaract()
    {
        _open = !_open;
        if (_open)
        {
            Open();
        }
        else
        {
            Close();
        }
    }

    private void Open()
    {
        _openObject.SetActive(true);
    }
    private void Close()
    {
        _openObject.SetActive(false);
    }
}
