using UnityEngine;

public class BaseResume : MonoBehaviour
{
    [SerializeField] string _name = "***NoData***";

    [SerializeField] string _productionEfficiency1 = "---";
    [SerializeField] string _productionEfficiency2 = "---";
    [SerializeField] string _productionEfficiency3 = "---";

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // ÉåÉWÉÖÉÅçÏê¨
    public void SetResume(BaseUnit baseUnit)
    {
        _name = baseUnit.Name;

        _productionEfficiency1 = baseUnit.GetProductionEfficiencyRank(ProductionEfficiency.SECTION_1);
        _productionEfficiency2 = baseUnit.GetProductionEfficiencyRank(ProductionEfficiency.SECTION_2);
        _productionEfficiency3 = baseUnit.GetProductionEfficiencyRank(ProductionEfficiency.SECTION_3);
    }
}
