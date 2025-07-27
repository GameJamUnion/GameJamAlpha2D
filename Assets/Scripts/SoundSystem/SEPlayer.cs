using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEPlayer : MonoBehaviour
{
	/// <summary>
	/// OneShot用のソース
	/// </summary>
	private AudioSource m_audioSource;

	private List<AudioSource> m_loopAudios = new List<AudioSource>();

	[SerializeField]
	private SEReferenceTable m_seTable;

	static SEPlayer _instance = null;

	/// <summary>
	/// SEの再生
	/// </summary>
	/// <param name="kind"></param>
	public void PlaySE(SEKind kind, float volume) {
		if (m_seTable == null) {
#if UNITY_EDITOR
			Debug.LogError("Not Reference SE Table");
#endif
			return;
		}

		if (m_audioSource == null) {
			return;
		}

		m_audioSource.volume = volume;
		m_audioSource.PlayOneShot(m_seTable.audioMap[kind]);
	}

	/// <summary>
	/// SEの再生
	/// </summary>
	/// <param name="kind"></param>
	public void PlayLoopSE(SEKind kind, float volume) {
		if (m_seTable == null) {
#if UNITY_EDITOR
			Debug.LogError("Not Reference SE Table");
#endif
			return;
		}

		AudioSource loopPlayer = m_loopAudios.Find(x => x.isPlaying == false);

		if (loopPlayer == null) {
			loopPlayer = gameObject.AddComponent<AudioSource>();

			m_loopAudios.Add(loopPlayer);
		}

		loopPlayer.loop = true;
		loopPlayer.clip = m_seTable.audioMap[kind];
		loopPlayer.volume = volume;
		loopPlayer.Play();
	}


	/// <summary>
	/// 再生終了
	/// </summary>
	/// <param name="kind"></param>
	public void StopLoopSE(SEKind kind) {
		if (m_seTable == null) {
#if UNITY_EDITOR
			Debug.LogError("Not Reference SE Table");
#endif
			return;
		}

		var audio = m_loopAudios.Find(x => x.clip);

		if (audio == null) {
			return;
		}

		audio.Stop();
		audio.clip = null;
	}

	/// <summary>
	/// 初期化（参照取得）
	/// </summary>
	private void Awake() {
		if (_instance != null) {
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
		TryGetComponent(out m_audioSource);
	}

	/// <summary>
	/// テーブルの初期化
	/// </summary>
	private void Start() {
		m_seTable.Initialize(this);
	}

	[ContextMenu("Test Play")]
	private void TestPlay() {
		PlaySE(SEKind.MachineWork, 1.0f);
	}

	[ContextMenu("Test Play Loop")]
	private void TestPlayLoop() {
		PlayLoopSE(SEKind.MachineWork, 1.0f);
	}
}
