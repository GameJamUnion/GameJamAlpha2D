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

	/// <summary>
	/// ランタイム用のオーディオマップ
	/// </summary>
	public Dictionary<SEKind, AudioClip> audioMap { get; private set; } = new Dictionary<SEKind, AudioClip>();

	/// <summary>
	/// 初期化済みかどうか
	/// </summary>
	public bool isInitialize { get; private set; } = false;

	/// <summary>
	/// 初期化
	/// </summary>
	public void Initialize() {
		if (isInitialize) { return; }
		foreach(var audio in m_audios) {
			audioMap.Add(audio.kind, audio.clip);
		}

		isInitialize = true;
	}
}

public enum SEKind {
	Stamp,
	Fire,
	DoorClose,
	DoorOpen,
	Paper,
	MachineWork,
}