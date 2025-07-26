using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterObjectBehavior : MonoBehaviour
{

    [DisplayName("開始シーン名")]
    public SceneNames StartSceneName
    {
        get => _StartSceneName;
        set => _StartSceneName = value;
    }
    [SerializeField]
    private SceneNames _StartSceneName = SceneNames.Title;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManagementManager.Instance.loadScene(StartSceneName);
    }
}
