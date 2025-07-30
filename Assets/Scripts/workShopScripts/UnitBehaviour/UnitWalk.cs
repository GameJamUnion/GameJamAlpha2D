using UnityEngine;
using UnityEngine.Events;

public class UnitWalk : MonoBehaviour
{
	[SerializeField]
	private float m_moveSpeed;

	[SerializeField]
	private Animator m_animator;

	public enum State {
		Wait,
		Walking,
	}


	private struct MoveInfo {
		public int nextPointIndex;
	}

	// 移動情報
	private MoveInfo m_moveInfo;

	// 状態
	private State m_state = State.Wait;

	// 移動ルート
	UnitWalkRoute unitWalkRoute = null;

	UnityAction<GoalPointType> m_onExitMoveEvent;

	/// <summary>
	/// 到着イベント登録
	/// </summary>
	/// <param name="exitMoveEvent"></param>
	private void RegisterExitMoveEvent(UnityAction<GoalPointType> exitMoveEvent) {
		m_onExitMoveEvent += exitMoveEvent;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="route"></param>
	public void StartWalk(UnitWalkRoute route) {
		if(unitWalkRoute != null || m_state == State.Walking) { return; }
		unitWalkRoute = route;

		transform.position = unitWalkRoute.pointList[0];

		m_moveInfo.nextPointIndex = 1;

		m_state = State.Walking;
	}

	private void UpdateWalk() {
		Vector3 target = unitWalkRoute.pointList[m_moveInfo.nextPointIndex];
		Vector3 direction = target - transform.position;

		// 移動
		transform.position = Vector3.MoveTowards(transform.position, target, m_moveSpeed * Time.deltaTime);

		// 到達チェック
		if (direction.magnitude < 0.1f) {
			transform.position = target;
			m_moveInfo.nextPointIndex++;

			// 到達
			if (m_moveInfo.nextPointIndex >= unitWalkRoute.pointList.Count) {
				unitWalkRoute = null;
				m_onExitMoveEvent?.Invoke(unitWalkRoute.goalType);
				m_state = State.Wait;
			}
		}
	}

	private void Update() {
		switch(m_state) {
			case State.Wait:
				break;
			case State.Walking:
				UpdateWalk();
				break;
		}
	}

	#region Test Code

	[Header("Test Property")]
	[SerializeField]
	private UnitWalkRoute m_route;

	[ContextMenu("TestRoute")]
	private void TestRoute() {
		StartWalk(m_route);
	}

	#endregion
}
