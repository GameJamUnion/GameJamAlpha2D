using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "SEReferenceTable", menuName = "Scriptable Objects/SEReferenceTable")]
public class SEReferenceTable : ScriptableObject
{
	[System.Serializable]
	private class ReferenceData {
		public SEKind kind;
		public AudioClip clip;
	}

	[SerializeField]
	private List<ReferenceData> m_audios;

	private SEPlayer m_player;

	/// <summary>
	/// ランタイム用のオーディオマップ
	/// </summary>
	public Dictionary<SEKind, AudioClip> audioMap { get; private set; } = new Dictionary<SEKind, AudioClip>();


	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize(SEPlayer player) {
		if (m_player != null) { return; }
		foreach(var audio in m_audios) {
			audioMap.Add(audio.kind, audio.clip);
		}

		m_player = player;
	}

	/// <summary>
	/// SE再生
	/// </summary>
	/// <param name="kind"></param>
	/// <param name="isLoop"></param>
	public void PlaySE(SEKind kind, bool isLoop = false, float volume = 1.0f) {
		if (isLoop) {
			m_player?.PlayLoopSE(kind, volume);
		}
		else {
			m_player?.PlaySE(kind, volume);
		}
	}

	/// <summary>
	/// 再生停止
	/// </summary>
	/// <param name="kind"></param>
	public void StopSE(SEKind kind) {
		m_player?.StopLoopSE(kind);
	}
}

public enum SEKind {
	Stamp,
	Fire,
	DoorClose,
	DoorOpen,
	Paper,
	MachineWork,
	Watch,
}