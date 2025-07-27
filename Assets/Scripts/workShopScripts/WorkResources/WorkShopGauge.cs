using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class WorkShopGauge : MonoBehaviour {
	private Slider m_slider;

	[SerializeField]
	private WorkScore m_score;

	private void Awake() {
		TryGetComponent(out m_slider);
	}

	private void Start() {
		m_score.RegisterGauge(this);
	}

	/// <summary>
	/// ゲージの更新
	/// </summary>
	/// <param name="rate"></param>
	public void UpdateValue(float rate) {
		m_slider.value = rate;
	}
}
