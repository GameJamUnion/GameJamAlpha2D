using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "UnitContainer", menuName = "Scriptable Objects/UnitContainer")]
public class UnitContainer : ScriptableObject
{
    private UnityAction onHired;

    private UnityAction onRemove;

	private UnityAction onCall;
   

	/// <summary>
	/// 雇用
	/// </summary>
	public void Hired() {
		// 実際の処理


		// イベント発火
		onHired?.Invoke();
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
