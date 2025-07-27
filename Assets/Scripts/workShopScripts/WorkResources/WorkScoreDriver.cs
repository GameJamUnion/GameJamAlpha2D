using UnityEngine;

public class WorkScoreDriver : MonoBehaviour {
	[SerializeField]
	private WorkScore m_score;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start() {
		m_score.Initialize();
	}

	// Update is called once per frame
	void Update() {
		m_score.UpdateElapsedTime(Time.deltaTime);
	}

	[ContextMenu("Add Score Test")]
	private void AddScoreTest(){
		m_score.AddScore(1);
	}
}
