
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkScore", menuName = "Scriptable Objects/UnitWalkRoute")]
public class UnitWalkRoute :ScriptableObject {
    [SerializeField]
    private List<Vector3> m_pointList;

    public List<Vector3> pointList => m_pointList;
}
