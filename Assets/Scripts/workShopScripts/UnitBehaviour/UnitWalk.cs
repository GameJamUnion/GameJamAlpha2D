using JetBrains.Annotations;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class UnitWalk : MonoBehaviour
{
	[SerializeField]
	private float m_moveSpeed;

	[SerializeField]
	private Animator m_animator;

	[SerializeField]
	private UnitWalkRoute m_route;

	public enum State {
		Wait,
		Walking,
	}


	private struct MoveInfo {
		public int nextPointIndex;
		public int prevPointIndex;
		public float distance;
	}

	private MoveInfo m_moveInfo;

	private State m_state = State.Wait;

	UnitWalkRoute unitWalkRoute = null;


	public void StartWalk(UnitWalkRoute route) {
		if(unitWalkRoute != null || m_state == State.Walking) { return; }
		unitWalkRoute = route;

		transform.position = unitWalkRoute.pointList[0];

		m_moveInfo.distance = Vector2.Distance(unitWalkRoute.pointList[0], unitWalkRoute.pointList[1]);
		m_moveInfo.nextPointIndex = 1;
		m_moveInfo.prevPointIndex = 0;

		m_state = State.Walking;
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

	private void UpdateWalk() {
		Vector3 target = unitWalkRoute.pointList[m_moveInfo.nextPointIndex];
		Vector3 direction = target - transform.position;

		// 移動
		transform.position = Vector3.MoveTowards(transform.position, target, m_moveSpeed * Time.deltaTime);

		// 到達チェック
		if (direction.magnitude < 0.1f) {
			transform.position = target;
			m_moveInfo.nextPointIndex++;
			m_moveInfo.prevPointIndex++;


			if (m_moveInfo.nextPointIndex >= unitWalkRoute.pointList.Count) {
				unitWalkRoute = null;
				m_state = State.Wait;
			}
		}
	}

	[ContextMenu("TestRoute")]
	private void TestRoute() {
		StartWalk(m_route);
	}
	
}
