using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class SelectPanelController : MonoBehaviour
{

    [Browsable(true)]
    public TextMeshProUGUI MainText
    {
        get => _MainText;
        set => _MainText = value;
    }
    [SerializeField, Browsable(false)]
    private TextMeshProUGUI _MainText = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMainText(string text)
    {
        if (MainText != null)
        {
            MainText.text = text;
        }
    }
}
