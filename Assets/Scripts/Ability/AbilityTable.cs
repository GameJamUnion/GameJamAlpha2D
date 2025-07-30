using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
	[SerializeField]
	private string m_name = "No Ability";

	[SerializeField]
	private AbilityType m_type = AbilityType.None;

	public string name => m_name;
	public AbilityType type => m_type;
}

/// <summary>
/// 特性の影響タイプ
/// </summary>
[System.Flags]
public enum AbilityType {
	None			= 0,
	Vitality_Up		= 1 << 0,		// 体力アップ
	Vitality_Down	= 1 << 1,		// 体力ダウン
	Hearing_Down	= 1 << 2,		// 聴力ダウン
	Disturber		= 1 << 3,		// 邪魔者
	Agility_Up		= 1 << 4,		// 速度アップ
	Agility_Down	= 1 << 5,		// 速度ダウン
}

/// <summary>
/// アビリティテーブル
/// </summary>
[CreateAssetMenu(fileName = "AbilityTable", menuName = "Scriptable Objects/AbilityTable")]
public class AbilityTable : ScriptableObject{
	[SerializeField]
	private List<Ability> m_abilityDatas;

	public List<Ability> abilityDatas => m_abilityDatas;

	/// <summary>
	/// 面接者用のアビリティデータの作成
	/// </summary>
	/// <returns></returns>
	public IntervieweeUnitAbility CreateIntervieweeUnitAbility() {
		var data = new IntervieweeUnitAbility();

		for(int i = 0; i < data.abilities.Length; ++i) {
			int index = Random.Range(0, m_abilityDatas.Count - 1);
			data.abilities[i] = abilityDatas[index];
		}

		return data;
	}
}

/// <summary>
/// 面接者が持つ特性
/// </summary>
public class IntervieweeUnitAbility {
	public const int AbilityCapacityMax = 2;

	private Ability[] m_abilities;

	public Ability[] abilities => m_abilities;

	public IntervieweeUnitAbility() {
		m_abilities = new Ability[AbilityCapacityMax];
	}
}