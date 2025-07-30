using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// アビリティ単体の表示コンポーネント
/// </summary>
public class UnitAbilityUI : MonoBehaviour
{
	[SerializeField]
	private Image m_window;

	[SerializeField]
	private TextMeshProUGUI m_text;

	[SerializeField]
	private AbilityTable m_abilityTable;

	public void SetAbility(Ability ability) {
		if (ability == null) {
			gameObject.SetActive(false);
		}
		m_text.text = ability.name;
		gameObject.SetActive(true);
	}
}

