using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// BuildProfilies��SceneList�ƈ�v������ (����)
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
