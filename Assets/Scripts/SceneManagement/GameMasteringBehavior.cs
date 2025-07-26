using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMasteringBehavior : MonoBehaviour
{
    private const string GameMasteringSceneName = "GameMastering";


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void main()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(GameMasteringSceneName, LoadSceneMode.Additive);
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
