using UnityEngine;

[System.Serializable]
public class ProductionEfficiencyRankClass
{
    [SerializeField] private string _scoreStr;
    [SerializeField] private float _maxProductionEfficiency;
    [SerializeField] private float _minProductionEfficiency;

    #region �v���p�e�B
    public string ScoreStr
    {
        get { return _scoreStr; }
    }
    public float MaxProductionEfficiency
    {
        get { return _maxProductionEfficiency; }
    }
    public float MinProductionEfficiency
    {
        get { return _minProductionEfficiency; }
    }
    #endregion
}
