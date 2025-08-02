using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TutorialEvent : MonoBehaviour, IPointerClickHandler
{
	[SerializeField]
	private List<Sprite> m_sprites;

	private Image m_tutorialImage;

	private int m_currentTutorialIndex = 0;

	private void Start() {
		TryGetComponent(out m_tutorialImage);
		m_tutorialImage.sprite = m_sprites[m_currentTutorialIndex];
	}

	public void OnPointerClick(PointerEventData eventData) {
		m_currentTutorialIndex++;
		// 次の画像がない
		if (m_currentTutorialIndex >= m_sprites.Count){
			// チュートリアル終了
			SceneManager.Instance.requestEndTutorialScene();
			return;
		}

		m_tutorialImage.sprite = m_sprites[m_currentTutorialIndex];
	}

}
