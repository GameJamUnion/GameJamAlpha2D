using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CAPointTable", menuName = "Scriptable Objects/CAPointTable")]
public class CAPointTable : ScriptableObject
{
    [SerializeField] private List<PointTableData> _datas = new List<PointTableData>();

    public List<PointTableData> Datas
    {
        get { return _datas; }
    }
}

[System.Serializable]
public class PointTableData
{
    // maxPlus > minPlus > minMinus > maxMinus
    [Header("区間の最大値と最小値(最大値は以下、最小値は以上)")]
    [SerializeField] float _max;
    [SerializeField] float _min;

    [Header("ウェイト")]
    [SerializeField] int _weight = 1;

    #region Property
    public float Max
    {
        get { return _max; }
    }
    public float Min
    {
        get { return _min; }
    }
    public int Weight
    {
        get { return _weight; }
    }
    #endregion
}
