using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEPlayer : MonoBehaviour
{
	private AudioSource m_audioSource;

	[SerializeField]
	private SEReferenceTable m_seTable;

	/// <summary>
	/// SE�̍Đ�
	/// </summary>
	/// <param name="kind"></param>
	public void PlaySE(SEKind kind) {
		if (m_seTable == null) {
#if UNITY_EDITOR
			Debug.LogError("Not Reference SE Table");
#endif
			return;
		}

		if (m_audioSource == null) {
			return;
		}

		m_audioSource.PlayOneShot(m_seTable.audioMap[kind]);
	}

	/// <summary>
	/// �������i�Q�Ǝ擾�j
	/// </summary>
	private void Awake() {
		TryGetComponent(out m_audioSource);
	}

	/// <summary>
	/// �e�[�u���̏�����
	/// </summary>
	private void Start() {
		m_seTable.Initialize();
	}

	[ContextMenu("Test Play")]
	private void TestPlay() {
		PlaySE(SEKind.MachineWork);
	}
}
