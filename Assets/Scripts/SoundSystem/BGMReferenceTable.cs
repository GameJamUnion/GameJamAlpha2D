using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMReferenceTalbe", menuName = "Scriptable Objects/BGMReferenceTalbe")]
public class BGMReferenceTalble : ScriptableObject
{
	[System.Serializable]
	private class ReferenceData {
		public BGMKind kind;
		public AudioClip clip;
	}


	[SerializeField]
	private List<ReferenceData> m_audios;

	private BGMPlayer m_player;

	/// <summary>
	/// �����^�C���p�̃I�[�f�B�I�}�b�v
	/// </summary>
	public Dictionary<BGMKind, AudioClip> audioMap { get; private set; } = new Dictionary<BGMKind, AudioClip>();

	/// <summary>
	/// �������ς݂��ǂ���
	/// </summary>
	public bool isInitialize { get; private set; } = false;

	/// <summary>
	/// BGM�̍Đ�
	/// </summary>
	/// <param name="kind"></param>
	/// <param name="crossFade"></param>
	/// <param name="offset"></param>
	public void PlayBGM(BGMKind kind, float crossFade = 2.0f, float offset = 1.0f) {
		m_player?.PlayBGM(kind, crossFade, offset);
	}

	/// <summary>
	/// ������
	/// </summary>
	public void Initialize(BGMPlayer player) {
		foreach (var audio in m_audios) {
			audioMap.Add(audio.kind, audio.clip);
		}
		m_player = player;
	}
}

public enum BGMKind {
	Title,
	Result,
	MainGame,
}