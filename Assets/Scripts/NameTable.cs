using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NameTable", menuName = "Scriptable Objects/NameTable")]
public class NameTable : ScriptableObject
{
    [SerializeField] List<string> _names;

    public List<string> Names
    {
        get { return _names; }
    }
}
