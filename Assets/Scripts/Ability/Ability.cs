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
/// �����̉e���^�C�v
/// </summary>
[System.Flags]
public enum AbilityType {
	None			= 0,
	Vitality_Up		= 1 << 0,		// �̗̓A�b�v
	Vitality_Down	= 1 << 1,		// �̗̓_�E��
	Hearing_Down	= 1 << 2,		// ���̓_�E��
	Disturber		= 1 << 3,		// �ז���
	Agility_Up		= 1 << 4,		// ���x�A�b�v
	Agility_Down	= 1 << 5,		// ���x�_�E��
}

/// <summary>
/// �A�r���e�B�e�[�u��
/// </summary>
[CreateAssetMenu(fileName = "AbilityTable", menuName = "Scriptable Objects/AbilityTable")]
public class AbilityTable : ScriptableObject{
	[SerializeField]
	private List<Ability> m_abilityDatas;

	public List<Ability> abilityDatas => m_abilityDatas;

	/// <summary>
	/// �ʐڎҗp�̃A�r���e�B�f�[�^�̍쐬
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
/// �ʐڎ҂�������
/// </summary>
public class IntervieweeUnitAbility {
	public const int AbilityCapacityMax = 2;

	private Ability[] m_abilities;

	public Ability[] abilities => m_abilities;

	public IntervieweeUnitAbility() {
		m_abilities = new Ability[AbilityCapacityMax];
	}
}