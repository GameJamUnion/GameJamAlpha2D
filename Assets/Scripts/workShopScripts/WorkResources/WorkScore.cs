using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WorkScore", menuName = "Scriptable Objects/WorkScore")]
public class WorkScore : ScriptableObject
{
	[SerializeField]
	private int m_goalScore;

	public int score { get; private set; }
	
	public int goalScore => m_goalScore;

	public WorkShopGauge scoreGauge { get; private set; } = null;

	public bool isFullScore { get { return score >= goalScore; } }

	private UnityAction fullScoreEvent;

	/// <summary>
	/// スコアの加算
	/// 減算はマイナス入れておくれ
	/// </summary>
	/// <param name="score"></param>
	public void AddScore(int score) {
		if (isFullScore) { return; }
		this.score += score;

		float rate = (float)this.score / (float)goalScore;
		scoreGauge.UpdateValue(rate);

		if (isFullScore) {
			fullScoreEvent?.Invoke();
		}
	}

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="timeLimit"></param>
	/// <param name="goalScore"></param>
	public void Initialize() {
		score = 0;
		scoreGauge?.UpdateValue((float)score / (float)goalScore);
	}


	/// <summary>
	/// ゲージの登録
	/// </summary>
	/// <param name="type"></param>
	/// <param name="gauge"></param>
	public void RegisterGauge(WorkShopGauge gauge) {
		scoreGauge = gauge;
		scoreGauge.UpdateValue((float)score / (float)goalScore);
	}

	/// <summary>
	/// スコアマックスになった時のイベント登録
	/// </summary>
	/// <param name="fullScoreEvent"></param>
	public void RegisterFullScoreEvent(UnityAction fullScoreEvent) {
		this.fullScoreEvent += fullScoreEvent;
	}
}
