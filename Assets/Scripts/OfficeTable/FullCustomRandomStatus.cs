using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "FullCustomRandomStatus", menuName = "Scriptable Objects/FullCustomRandomStatus")]
public class FullCustomRandomStatus : ScriptableObject
{
    [SerializeField] private List<ProductionEfficiencys> _productionEfficiencys;

    public ProductionEfficiencys GetRandomProductionEfficiencys()
    {
        return _productionEfficiencys[Random.Range(0, _productionEfficiencys.Count)];
    }
}

[System.Serializable]
public class ProductionEfficiencys
{
    [SerializeField] private float _productionEfficiency1 = 1f;
    [SerializeField] private float _productionEfficiency2 = 1f;
    [SerializeField] private float _productionEfficiency3 = 1f;

    public float ProductionEfficiency1
    {
        get { return _productionEfficiency1; }
    }
    public float ProductionEfficiency2
    {
        get { return _productionEfficiency2; }
    }
    public float ProductionEfficiency3
    {
        get { return _productionEfficiency3; }
    }
}