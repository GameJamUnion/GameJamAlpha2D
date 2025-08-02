using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialEvent : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private List<GameObject> m_sheets;

	private GameObject m_currentSheet;

	private int m_currentTutorialIndex = 0;

	private void Start() {
		m_currentTutorialIndex = 0;
		m_currentSheet = m_sheets[m_currentTutorialIndex];
		m_currentSheet.SetActive(true);
	}

	public void OnPointerClick(PointerEventData eventData) {
		m_currentTutorialIndex++;

		// 次の画像がない
		if (m_currentTutorialIndex >= m_sheets.Count){
			// チュートリアル終了
			SceneManager.Instance.requestEndTutorialScene();
			return;
		}

		m_currentSheet.SetActive(false);
		m_currentSheet = m_sheets[m_currentTutorialIndex];

		m_currentSheet.SetActive(true);
	}


}
