using UnityEngine;

public class GameWaveManagerBehavior : MonoBehaviour
{
    [SerializeField]
    public GameWaveSettings WaveSettings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameWaveManager.Instance.registerWaveSettings(WaveSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
