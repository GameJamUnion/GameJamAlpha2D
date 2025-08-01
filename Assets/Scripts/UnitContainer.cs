using RI;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UnitContainer", menuName = "Scriptable Objects/UnitContainer")]
public class UnitContainer : ScriptableObject
{
    /// <summary>
    /// �ٗp�C�x���g
    /// </summary>
    private UnityAction<BaseUnit> onHired;

	/// <summary>
	/// ���كC�x���g
	/// </summary>
    private UnityAction<BaseUnit> onRemove;

    /// <summary>
    /// �Ăяo���C�x���g
    /// </summary>
    private UnityAction<BaseUnit> onCall;

    /// <summary>
    /// �Ăяo������Ԃ��C�x���g
    /// </summary>
    private UnityAction<BaseUnit> onCallBack;

	/// <summary>
	/// ��Ə�ύX�C�x���g
	/// </summary>
	private UnityAction<int, RI.PlacementState> onMoveSectionByWorker;

	/// <summary>
	/// �W�Q�C�x���g
	/// </summary>
	private UnityAction<int> onInterfere;

    /// <summary>
    /// �W�Q�I���C�x���g
    /// </summary>
    private UnityAction<int, int> onStopInterfere;


    /// <summary>
    /// �ٗp
    /// </summary>
    public void Hired(BaseUnit unit) {
		// ���ۂ̏���


		// �C�x���g����
		onHired?.Invoke(unit);
	}

	/// <summary>
	/// ����
	/// </summary>
	public void Remove(BaseUnit unit)
	{
		onRemove?.Invoke(unit);
	}

	/// <summary>
	/// �Ăяo��
	/// </summary>
	public void Call(BaseUnit unit)
	{
		onCall?.Invoke(unit);
	}

    /// <summary>
    /// �Ăяo������Ԃ�
    /// </summary>
    public void CallBack(BaseUnit unit)
	{
		onCallBack?.Invoke(unit);
	}

    /// <summary>
    /// ��Ə�ύX
    /// </summary>
    public void MoveSectionByWorker(int originId, RI.PlacementState placementState)
	{
		onMoveSectionByWorker?.Invoke(originId, placementState);
	}

	/// <summary>
	/// �W�Q�J�n
	/// </summary>
	/// <param name="interfereOriginId"></param>
	public void Interfere(int interfereOriginId)
	{
        onInterfere?.Invoke(interfereOriginId);
	}

    /// <summary>
    /// �W�Q�I��
    /// </summary>
    /// <param name="interfereOriginId"></param>
    /// <param name="beInterferedOriginId"></param>
    public void StopInterfere(int interfereOriginId, int beInterferedOriginId)
    {
        onStopInterfere?.Invoke(interfereOriginId, beInterferedOriginId);
    }

    /// <summary>
    /// �ٗp�C�x���g�o�^
    /// </summary>
    /// <param name="onHired"></param>
    public void RegisterEventOnHired(UnityAction<BaseUnit> onHired)
	{
        this.onHired += onHired;
    }

	/// <summary>
	/// ���كC�x���g�o�^
	/// </summary>
	/// <param name="onRemove"></param>
	public void RegisterEventOnRemove(UnityAction<BaseUnit> onRemove)
	{
		this.onRemove += onRemove;
	}

	/// <summary>
	/// �Ăяo���C�x���g�o�^
	/// </summary>
	/// <param name="onCall"></param>
	public void RegisterEventOnCall(UnityAction<BaseUnit> onCall)
	{
		this.onCall += onCall;
	}

    /// <summary>
    /// �Ăяo������Ԃ��C�x���g�o�^
    /// </summary>
    /// <param name="onCallBack"></param>
    public void RegisterEventOnCallBack(UnityAction<BaseUnit> onCallBack)
	{
        this.onCallBack += onCallBack;
    }

    /// <summary>
    /// ��Ə�ύX�C�x���g�o�^
    /// </summary>
    /// <param name="onMoveSectionByWorker"></param>
    public void RegisterEventOnMoveSectionByWorker(UnityAction<int, RI.PlacementState> onMoveSectionByWorker)
    {
        this.onMoveSectionByWorker += onMoveSectionByWorker;
    }

    /// <summary>
    /// �W�Q�C�x���g�o�^
    /// </summary>
    /// <param name="onInterfere"></param>
    public void RegisterEventInterfereOn(UnityAction<int> onInterfere)
    {
        this.onInterfere += onInterfere;
    }

    /// <summary>
    /// �W�Q�I���C�x���g�o�^
    /// </summary>
    /// <param name="onStopInterfere"></param>
    public void RegisterEventInterfereOff(UnityAction<int, int> onStopInterfere)
    {
        this.onStopInterfere += onStopInterfere;
    }
}
