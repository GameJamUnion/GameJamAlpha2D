using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UnitContainer", menuName = "Scriptable Objects/UnitContainer")]
public class UnitContainer : ScriptableObject
{
    /// <summary>
    /// 雇用イベント
    /// </summary>
    private UnityAction<BaseUnit> onHired;

	/// <summary>
	/// 解雇イベント
	/// </summary>
    private UnityAction<BaseUnit> onRemove;

    /// <summary>
    /// 呼び出しイベント
    /// </summary>
    private UnityAction<BaseUnit> onCall;

    /// <summary>
    /// 呼び出した後返すイベント
    /// </summary>
    private UnityAction<BaseUnit> onCallBack;

    /// <summary>
    /// 雇用
    /// </summary>
    public void Hired(BaseUnit unit) {
		// 実際の処理


		// イベント発火
		onHired?.Invoke(unit);
	}

	/// <summary>
	/// 解雇
	/// </summary>
	public void Remove(BaseUnit unit)
	{
		onRemove?.Invoke(unit);
	}

	/// <summary>
	/// 呼び出し
	/// </summary>
	public void Call(BaseUnit unit)
	{
		onCall?.Invoke(unit);
	}

    /// <summary>
    /// 呼び出した後返す
    /// </summary>
    public void CallBack(BaseUnit unit)
	{
		onCallBack?.Invoke(unit);
	}

	/// <summary>
	/// 雇用イベント登録
	/// </summary>
	/// <param name="onHired"></param>
    public void RegisterEventOnHired(UnityAction<BaseUnit> onHired)
	{
        this.onHired += onHired;
    }

	/// <summary>
	/// 解雇イベント登録
	/// </summary>
	/// <param name="onRemove"></param>
	public void RegisterEventOnRemove(UnityAction<BaseUnit> onRemove)
	{
		this.onRemove += onRemove;
	}

	/// <summary>
	/// 呼び出しイベント登録
	/// </summary>
	/// <param name="onCall"></param>
	public void RegisterEventOnCall(UnityAction<BaseUnit> onCall)
	{
		this.onCall += onCall;
	}

    /// <summary>
    /// 呼び出した後返すイベント登録
    /// </summary>
    /// <param name="onCallBack"></param>
    public void RegisterEventOnCallBack(UnityAction<BaseUnit> onCallBack)
	{
        this.onCallBack += onCallBack;
    }
}
