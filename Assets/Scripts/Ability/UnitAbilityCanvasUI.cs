using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UnitAbilityCanvasUI : MonoBehaviour
{
	[SerializeField]
	private UnitAbilityUI m_abilityUIOriginal;

	[SerializeField]
	private List<Transform> m_abilityUIPositions;

	[SerializeField]
	private float m_playTime = 0.2f;

	private CanvasGroup m_canvas = null;

	private UnitAbilityUI[] m_unitAbilityUIs = new UnitAbilityUI[IntervieweeUnitAbility.AbilityCapacityMax];

	public bool isDisplay { get; private set; } = false;


	/// <summary>
	/// 指定したユニットのアビリティを表示する
	/// </summary>
	/// <param name="unitAbility"></param>
	public void OnDisplay(IntervieweeUnitAbility unitAbility) {
		for(int i = 0; i < m_unitAbilityUIs.Length; ++i) {
			m_unitAbilityUIs[i].SetAbility(unitAbility.abilities[i]);
		}
		StartCoroutine(Display(m_playTime));

		isDisplay = true;
	}


	/// <summary>
	/// 表示を隠す
	/// </summary>
	public void OnHide() {
		StartCoroutine(Hide(m_playTime));
		isDisplay = false;
	}

	#region Unity Events
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
	}
	#endregion

	#region Coroutines
	private IEnumerator Hide(float playTime) {
		float elapsedTime = 0.0f;
		m_canvas.alpha = 1.0f;
		while (elapsedTime < playTime) {
			elapsedTime += Time.deltaTime;
			m_canvas.alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / playTime);
			yield return null;
		}
		m_canvas.alpha = 0.0f;
		isDisplay = false;
	}

	private IEnumerator Display(float playTime) {
		float elapsedTime = 0.0f;
		m_canvas.alpha = 0.0f;
		while (elapsedTime < playTime) {
			elapsedTime += Time.deltaTime;
			m_canvas.alpha = Mathf.Lerp(0.0f, 1.0f, elapsedTime / playTime);
			yield return null;
		}
		m_canvas.alpha = 1.0f;
		isDisplay = true;
	}
	#endregion

	[ContextMenu("Test Display")]
	private void TestDisplay(){
		var data = new IntervieweeUnitAbility();

		data.abilities[0] = new Ability();
		data.abilities[1] = new Ability();

		OnDisplay(data);
	}

	[ContextMenu("Test Hide")]
	private void TestHide() {
		OnHide();
	}
}
