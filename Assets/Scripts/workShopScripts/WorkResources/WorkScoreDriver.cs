using UnityEngine;

public class WorkScoreDriver : MonoBehaviour {
	[SerializeField]
	private WorkScore m_score;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start() {
		m_score.Initialize();
	}

	[ContextMenu("Add Score Test")]
	private void AddScoreTest(){
		m_score.AddScore(1);
	}
}
