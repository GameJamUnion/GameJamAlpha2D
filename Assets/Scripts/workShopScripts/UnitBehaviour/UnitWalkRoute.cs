
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkScore", menuName = "Scriptable Objects/UnitWalkRoute")]
public class UnitWalkRoute :ScriptableObject {
    [SerializeField]
    private List<Vector3> m_pointList;

    [SerializeField]
    GoalPointType m_goalType;
    public List<Vector3> pointList => m_pointList;

    public GoalPointType goalType => m_goalType;
}

public enum GoalPointType {
    Work_Section1,  // ì‹Æê1
	Work_Section2,  // ì‹Æê2
	Work_Section3,  // ì‹Æê‚P
	Office,         // ƒIƒtƒBƒX
    BreakRoom       // ‹xŒeŠ
}
