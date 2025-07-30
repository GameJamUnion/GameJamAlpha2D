using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CustomSimpleAutoRandomStatus", menuName = "Scriptable Objects/CustomSimpleAutoRandomStatus")]
public class CustomSimpleAutoRandomStatus : ScriptableObject
{
    [SerializeField] List<SimpleAutoRandomStatusSectionData> _datas;

    public List<SimpleAutoRandomStatusSectionData> Datas
    {
        get { return _datas; }
    }
}

[System.Serializable]
public class SimpleAutoRandomStatusSectionData
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


