using UnityEngine;

[CreateAssetMenu(fileName = "ResumeData", menuName = "Scriptable Objects/ResumeData")]
public class ResumeData : ScriptableObject
{
    [SerializeField] ProductionEfficiencyRankClass _rankS;
    [SerializeField] ProductionEfficiencyRankClass _rankA;
    [SerializeField] ProductionEfficiencyRankClass _rankB;
    [SerializeField] ProductionEfficiencyRankClass _rankC;
    [SerializeField] ProductionEfficiencyRankClass _rankD;
    [SerializeField] ProductionEfficiencyRankClass _rankE;
    [SerializeField] ProductionEfficiencyRankClass _rankF;

    #region プロパティ
    public ProductionEfficiencyRankClass RankS
    {
        get { return _rankS; }
    }
    public ProductionEfficiencyRankClass RankA
    {
        get { return _rankA; }
    }
    public ProductionEfficiencyRankClass RankB
    {
        get { return _rankB; }
    }
    public ProductionEfficiencyRankClass RankC
    {
        get { return _rankC; }
    }
    public ProductionEfficiencyRankClass RankD
    {
        get { return _rankD; }
    }
    public ProductionEfficiencyRankClass RankE
    {
        get { return _rankE; }
    }
    public ProductionEfficiencyRankClass RankF
    {
        get { return _rankF; }
    }
    #endregion
}
