using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UnitAbilityCanvasUI : MonoBehaviour
{
	[SerializeField]
	private UnitAbilityUI m_abilityUIOriginal;

	[SerializeField]
	private List<Transform> m_abilityUIPositions;

	private Canvas m_canvas = null;

	private UnitAbilityUI[] m_unitAbilityUIs = new UnitAbilityUI[IntervieweeUnitAbility.AbilityCapacityMax];

	public bool isDisplay { get; private set; } = false;

	private void Awake() {
		TryGetComponent(out m_canvas);

	}

	private void Start() {
		for (int i = 0; i < m_unitAbilityUIs.Length; ++i) {
			if (m_abilityUIPositions.Count <= i) {
#if UNITY_EDITOR
				Debug.LogError("アビリティを表示するためのポジション設定が足りません");
#endif
				return;
			}

			m_unitAbilityUIs[i] = Instantiate<UnitAbilityUI>(m_abilityUIOriginal, m_abilityUIPositions[i]);
		}

		m_canvas.enabled = false;
	}

	/// <summary>
	/// 指定したユニットのアビリティを表示する
	/// </summary>
	/// <param name="unitAbility"></param>
	public void Display(IntervieweeUnitAbility unitAbility) {

		m_canvas.enabled = true;
		for(int i = 0; i < m_unitAbilityUIs.Length; ++i) {
			m_unitAbilityUIs[i].SetAbility(unitAbility.abilities[i]);
		}

		isDisplay = true;
	}

	/// <summary>
	/// 表示を隠す
	/// </summary>
	public void Hide() {
		m_canvas.enabled = false;
		isDisplay = false;
	}

	[ContextMenu("Test Display")]
	private void TestDisplay(){
		var data = new IntervieweeUnitAbility();

		data.abilities[0] = new Ability();
		data.abilities[1] = new Ability();

		Display(data);
	}

	[ContextMenu("Test Hide")]
	private void TestHide() {
		Hide();
	}
}
