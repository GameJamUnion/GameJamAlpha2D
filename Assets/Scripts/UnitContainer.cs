using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UnitContainer", menuName = "Scriptable Objects/UnitContainer")]
public class UnitContainer : ScriptableObject
{
    private UnityAction onHired;

    private UnityAction onRemove;

	private UnityAction onCall;

	// オフィス側で扱っているユニット
	private List<BaseUnit> _officeBaseUnit;

    #region プロパティ
	public List<BaseUnit> OfficeBaseUnit
	{
		get { return _officeBaseUnit; }
		set { _officeBaseUnit = value; }
	}
    #endregion

    /// <summary>
    /// 雇用
    /// </summary>
    public void Hired() {
		// 実際の処理


		// イベント発火
		onHired?.Invoke();
	}

	/// <summary>
	/// 解雇
	/// </summary>
	public void Remove()
	{
		onRemove?.Invoke();
	}

	/// <summary>
	/// 呼び出し
	/// </summary>
	public void Call()
	{
		onCall?.Invoke();
	}

	

	/// <summary>
	/// 雇用イベント登録
	/// </summary>
	/// <param name="onHired"></param>
    public void RegisterEventOnHired(UnityAction onHired) {
        this.onHired += onHired;
    }

	/// <summary>
	/// 解雇イベント登録
	/// </summary>
	/// <param name="onRemove"></param>
	public void RegisterEventOnRemove(UnityAction onRemove) {
		this.onRemove += onRemove;
	}

	/// <summary>
	/// 呼び出しイベント登録
	/// </summary>
	/// <param name="onCall"></param>
	public void RegisterEventOnCall(UnityAction onCall) {
		this.onCall += onCall;
	}
}
