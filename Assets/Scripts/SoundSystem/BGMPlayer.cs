using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class BGMPlayer : MonoBehaviour {

	private AudioSource[] m_audioSource = new AudioSource[2];

	private int m_playingAudioIndex = 0;

	private int playingAudioIndex => m_playingAudioIndex % 2;

	private int deactiveAudioIndex => (m_playingAudioIndex + 1) % 2;

	private AudioSource currentAudio => m_audioSource[playingAudioIndex];

	private AudioSource deactiveAudio => m_audioSource[deactiveAudioIndex];

	[SerializeField]
	private BGMReferenceTalble m_table;

	[SerializeField]
	private AudioSource m_audioSource1;

	[SerializeField]
	private AudioSource m_audioSource2;

	private static BGMPlayer _instance = null;

	/// <summary>
	/// SEÇÃçƒê∂
	/// </summary>
	/// <param name="kind"></param>
	public void PlayBGM(BGMKind kind, float crossFadeTime = 1.0f, float offset = 0.0f) {
		if (m_table == null) {
#if UNITY_EDITOR
			Debug.LogError("Not Reference BGM Table");
#endif
			return;
		}

		if (m_audioSource == null) {
			return;
		}
		StartCoroutine(CrossFadeIn(kind, crossFadeTime, offset));
		StartCoroutine(CrossFadeOut(kind, crossFadeTime));
	}

	private void Awake() {
		if (_instance != null) {
			Destroy(this.gameObject);
			return;
		}

		_instance = this;
	}

	/// <summary>
	/// ÉeÅ[ÉuÉãÇÃèâä˙âª
	/// </summary>
	private void Start() {
		m_table.Initialize(this);
		m_audioSource = new AudioSource[2] {
			m_audioSource1,
			m_audioSource2
		};
	}

	[ContextMenu("Test Play 1")]
	private void TestPlay1() {
		PlayBGM(BGMKind.Title, 2.0f, 1);
	}
	[ContextMenu("Test Play 2")]
	private void TestPlay2() {
		PlayBGM(BGMKind.MainGame, 2.0f, 1);
	}

	private void SwapAudio() {
		m_playingAudioIndex = (m_playingAudioIndex + 1) % 2;
	}

	private IEnumerator CrossFadeOut(BGMKind kind, float crossFadeTime) {
		float time = 0.0f;
		AudioSource outAudio = currentAudio;
		while (time < crossFadeTime) {
			outAudio.volume = Mathf.Clamp01((crossFadeTime - time) / crossFadeTime);
			time += Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator CrossFadeIn(BGMKind kind, float crossFadeTime, float offset) {
		yield return new WaitForSeconds(offset);
		float time = 0.0f;
		SwapAudio();
		currentAudio.clip = m_table.audioMap[kind];
		currentAudio.Play();
		while (time < crossFadeTime) {
			currentAudio.volume = Mathf.Clamp01(time / crossFadeTime);
			time += Time.deltaTime;
			yield return null;
		}

		deactiveAudio.Stop();
	}

}