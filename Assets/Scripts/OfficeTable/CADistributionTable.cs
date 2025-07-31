using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CADistributionTable", menuName = "Scriptable Objects/CADistributionTable")]
public class CADistributionTable : ScriptableObject
{
    [SerializeField] private List<DistributionTableData> _datas;

    public List<DistributionTableData> Datas {  get { return _datas; } }
}

[System.Serializable]
public class DistributionTableData
{
    // maxPlus > minPlus > minMinus > maxMinus
    [Header("��Ԃ̍ő�l�ƍŏ��l(�ő�l�͈ȉ��A�ŏ��l�͈ȏ�)")]
    [SerializeField] float _max;
    [SerializeField] float _min;

    [Header("�E�F�C�g")]
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
