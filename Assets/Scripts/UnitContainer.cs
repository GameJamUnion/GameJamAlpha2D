using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitContainer", menuName = "Scriptable Objects/UnitContainer")]
public class UnitContainer : ScriptableObject
{
    private UnityAction onHired;

    private UnityAction onRemove;

	private UnityAction onCall;

	// �I�t�B�X���ň����Ă��郆�j�b�g
	private List<BaseUnit> _officeBaseUnit;

    #region �v���p�e�B
	public List<BaseUnit> OfficeBaseUnit
	{
		get { return _officeBaseUnit; }
		set { _officeBaseUnit = value; }
	}
    #endregion

    /// <summary>
    /// �ٗp
    /// </summary>
    public void Hired() {
		// ���ۂ̏���


		// �C�x���g����
		onHired?.Invoke();
	}

	/// <summary>
	/// ����
	/// </summary>
	public void Remove()
	{
		onRemove?.Invoke();
	}

	/// <summary>
	/// �Ăяo��
	/// </summary>
	public void Call()
	{
		onCall?.Invoke();
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
