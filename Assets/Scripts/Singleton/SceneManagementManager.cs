using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BuildProfilies‚ÌSceneList‚Æˆê’v‚³‚¹‚é (èì‹Æ)
/// </summary>
public enum SceneNames 
{
    Master = 0,
    GameMastering = 1,
    Title = 2,
}

public class SceneManagementManager : SingletonBase<SceneManagementManager>
{
    public void loadScene(SceneNames sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString(), LoadSceneMode.Additive);
    }
}
