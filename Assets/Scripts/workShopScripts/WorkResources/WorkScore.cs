using UnityEngine;

[CreateAssetMenu(fileName = "WorkScore", menuName = "Scriptable Objects/WorkScore")]
public class WorkScore : ScriptableObject
{
	[SerializeField]
	private int m_goalScore;

	public int score { get; private set; }
	
	public int goalScore => m_goalScore;

	public WorkShopGauge scoreGauge { get; private set; } = null;

	/// <summary>
	/// �X�R�A�̉��Z
	/// ���Z�̓}�C�i�X����Ă�����
	/// </summary>
	/// <param name="score"></param>
	public void AddScore(int score) {
		this.score += score;

		float rate = (float)this.score / (float)goalScore;
		scoreGauge.UpdateValue(rate);
	}

	/// <summary>
	/// ������
	/// </summary>
	/// <param name="timeLimit"></param>
	/// <param name="goalScore"></param>
	public void Initialize() {
		score = 0;
		scoreGauge?.UpdateValue((float)score / (float)goalScore);
	}


	/// <summary>
	/// �Q�[�W�̓o�^
	/// </summary>
	/// <param name="type"></param>
	/// <param name="gauge"></param>
	public void RegisterGauge(WorkShopGauge gauge) {
		scoreGauge = gauge;
		scoreGauge.UpdateValue((float)score / (float)goalScore);
	}
}
