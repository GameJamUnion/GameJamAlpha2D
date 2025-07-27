using UnityEngine;

[CreateAssetMenu(fileName = "WorkScore", menuName = "Scriptable Objects/WorkScore")]
public class WorkScore : ScriptableObject
{
	public enum Type {
		Time,
		Score,
	}
	[SerializeField]
	private int goalScore;

	[SerializeField]
	private int timeLimit;
	public int score { get; private set; }
	
	public int elapsedTime { get; private set; }

	private float counter { get; set; } = 0.0f;

	public WorkShopGauge timeGauge { get; private set; } = null;
	public WorkShopGauge scoreGauge { get; private set; } = null;

	/// <summary>
	/// 時間経過
	/// </summary>
	/// <param name="deltaTime"></param>
	public void UpdateElapsedTime(float deltaTime) {
		counter += deltaTime;
		elapsedTime = (int)counter;

		float rate = (float)(timeLimit - elapsedTime) / (float)timeLimit;
		timeGauge?.UpdateValue(rate);
	}

	/// <summary>
	/// スコアの加算
	/// 減算はマイナス入れておくれ
	/// </summary>
	/// <param name="score"></param>
	public void AddScore(int score) {
		this.score += score;

		float rate = (float)this.score / (float)goalScore;
		scoreGauge.UpdateValue(rate);
	}

	/// <summary>
	/// 初期化
	/// </summary>
	/// <param name="timeLimit"></param>
	/// <param name="goalScore"></param>
	public void Initialize() {
		counter = 0.0f;
		UpdateElapsedTime(0.0f);

		score = 0;
		scoreGauge?.UpdateValue((float)score / (float)goalScore);
	}


	/// <summary>
	/// ゲージの登録
	/// </summary>
	/// <param name="type"></param>
	/// <param name="gauge"></param>
	public void RegisterGauge(Type type, WorkShopGauge gauge) {
		switch(type) {
			case Type.Score:
				scoreGauge = gauge;
				scoreGauge.UpdateValue((float)score / (float)goalScore);
				break;
			case Type.Time:
				timeGauge = gauge;
				break;
		}
	}
}
