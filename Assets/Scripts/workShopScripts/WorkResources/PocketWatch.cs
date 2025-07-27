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
	private float m_offset = 90.0f;

	private float m_currentRot = 360.0f;

	private const float FullRot = 360.0f;

	private void Start() {
		m_workTime.Initialize();
	}

	// Update is called once per frame
	void Update() {
		if (m_workTime.isTimeup) { return; }
		m_workTime.UpdateElapsedTime(Time.deltaTime);
		m_currentRot = (FullRot + m_offset) + FullRot *  (1 - m_workTime.timeRate);
		m_minuteHand.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, m_currentRot));
	}
}

