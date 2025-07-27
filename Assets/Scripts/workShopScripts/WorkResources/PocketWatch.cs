using UnityEngine;
using UnityEngine.UI;

public class PocketWatch : MonoBehaviour
{
	[SerializeField]
	private Image m_minuteHand;

	[SerializeField]
	private Image m_watchBoard;

	[SerializeField]
	private WorkTime m_workTime;

	[SerializeField]
	private SEReferenceTable m_seTable;

	[SerializeField]
	private float m_seVolume = 1.0f;

	[SerializeField]
	private float m_offset = 90.0f;

	private float m_currentRot = 360.0f;

	private const float FullRot = 360.0f;

	private int m_prevTime = 0;

	private void Start() {
		m_workTime.Initialize();
	}

	// Update is called once per frame
	void Update() {
		if (m_workTime.isTimeup) { return; }
		m_workTime.UpdateElapsedTime(Time.deltaTime);
		UpdateMunuteHandSound();
	}

	/// <summary>
	/// 分針の音のSE更新
	/// </summary>
	private void UpdateMunuteHandSound() {
		if (m_prevTime != m_workTime.elapsedTime) {
			m_seTable.StopSE(SEKind.Watch);
			m_seTable.PlaySE(SEKind.Watch, true, m_seVolume);
			UpdateMinuteHand();
		}
		 m_prevTime = m_workTime.elapsedTime;
	}

	/// <summary>
	/// 分針更新
	/// </summary>
	private void UpdateMinuteHand() {
		m_currentRot = (FullRot + m_offset) + FullRot * (1 - m_workTime.timeRate);
		m_minuteHand.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, m_currentRot));
	}
}

