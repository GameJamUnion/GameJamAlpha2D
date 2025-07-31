using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class WorkShopGauge : MonoBehaviour
    , IGameWaveChangeReceiver
{
	private Slider m_slider;

	[SerializeField]
	private WorkScore m_score;

	private void Awake() {
		TryGetComponent(out m_slider);
	}

	private void Start() {
		//m_score.RegisterGauge(this);
		GameWaveManager.Instance.registerReceiver(this);
	}

	/// <summary>
	/// �Q�[�W�̍X�V
	/// </summary>
	/// <param name="rate"></param>
	public void UpdateValue(float rate) {
		if (m_slider == null) { return; }
		m_slider.value = rate;
	}

    #region IGameWaveChangeReceiver
    public void receiveChangeWave(GameWaveChangeReceiveParam param)
    {
        param.WorkScore.RegisterGauge(this);
    }
    #endregion IGameWaveChangeReceiver

}
