using UnityEngine;

public class BGMPlayer : MonoBehaviour {
	private AudioSource m_audioSource;

	[SerializeField]
	private BGMReferenceTalble m_table;

	/// <summary>
	/// SEの再生
	/// </summary>
	/// <param name="kind"></param>
	public void PlayBGM(BGMKind kind) {
		if (m_table == null) {
#if UNITY_EDITOR
			Debug.LogError("Not Reference BGM Table");
#endif
			return;
		}

		if (m_audioSource == null) {
			return;
		}

		m_audioSource.PlayOneShot(m_table.audioMap[kind]);
	}

	/// <summary>
	/// 初期化（参照取得）
	/// </summary>
	private void Awake() {
		TryGetComponent(out m_audioSource);
	}

	/// <summary>
	/// テーブルの初期化
	/// </summary>
	private void Start() {
		m_table.Initialize();
	}

	[ContextMenu("Test Play")]
	private void TestPlay() {
		PlayBGM(BGMKind.MainGame);
	}
}
