using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WorkTime", menuName = "Scriptable Objects/WorkTime")]
public class WorkTime : ScriptableObject
{
	[SerializeField]
	private int m_timeLimit;
	public int elapsedTime { get; private set; }
	public int timeLimit => m_timeLimit;

	private float counter { get; set; } = 0.0f;

	public float timeRate {
		get {
			return counter / (float)timeLimit;
		}
	}

	public bool isTimeup { get{ return elapsedTime >= timeLimit; } }

	private UnityAction timeupEvent;

	/// <summary>
	/// 時間経過
	/// </summary>
	/// <param name="deltaTime"></param>
	public void UpdateElapsedTime(float deltaTime) {
		counter += deltaTime;
		elapsedTime = (int)counter;

		if (isTimeup) {
			timeupEvent?.Invoke();
		}
	}

	public void Initialize() {
		counter = 0.0f;
		UpdateElapsedTime(0.0f);
	}

	/// <summary>
	/// タイムアップイベントを登録
	/// </summary>
	/// <param name="timeupEvent"></param>
	public void RegisterTimeupEvent(UnityAction timeupEvent) {
		this.timeupEvent += timeupEvent;
	}
}
