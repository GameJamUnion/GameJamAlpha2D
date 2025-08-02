using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameWaveSettings", menuName = "Scriptable Objects/GameWaveSettings")]
public class GameWaveSettings : ScriptableObject
{
    [Serializable]
    public class GameWaveSettingsConfiguration
    {
        [SerializeField]
        public WorkScore[] WorkScores;

        [SerializeField]
        public WorkTime WorkTime;
    }

    [SerializeField]
    public GameWaveSettingsConfiguration Configuration;
}
