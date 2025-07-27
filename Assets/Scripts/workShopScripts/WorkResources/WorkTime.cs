using UnityEngine;

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

	/// <summary>
	/// ŽžŠÔŒo‰ß
	/// </summary>
	/// <param name="deltaTime"></param>
	public void UpdateElapsedTime(float deltaTime) {
		counter += deltaTime;
		elapsedTime = (int)counter;

		float rate = (float)(timeLimit - elapsedTime) / (float)timeLimit;
	}

	public void Initialize() {
		counter = 0.0f;
		UpdateElapsedTime(0.0f);
	}
}
