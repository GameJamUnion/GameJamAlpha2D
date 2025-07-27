using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UnitContainer", menuName = "Scriptable Objects/UnitContainer")]
public class UnitContainer : ScriptableObject
{
    private UnityAction onHired;

    private UnityAction onRemove;

	private UnityAction onCall;
   

	/// <summary>
	/// �ٗp
	/// </summary>
	public void Hired() {
		// ���ۂ̏���


		// �C�x���g����
		onHired?.Invoke();
	}

	

	/// <summary>
	/// �ٗp�C�x���g�o�^
	/// </summary>
	/// <param name="onHired"></param>
    public void RegisterEventOnHired(UnityAction onHired) {
        this.onHired += onHired;
    }

	/// <summary>
	/// ���كC�x���g�o�^
	/// </summary>
	/// <param name="onRemove"></param>
	public void RegisterEventOnRemove(UnityAction onRemove) {
		this.onRemove += onRemove;
	}

	/// <summary>
	/// �Ăяo���C�x���g�o�^
	/// </summary>
	/// <param name="onCall"></param>
	public void RegisterEventOnCall(UnityAction onCall) {
		this.onCall += onCall;
	}
}
