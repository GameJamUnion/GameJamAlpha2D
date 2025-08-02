using UnityEngine;
using UnityEngine.Events;

public class ProductMove : MonoBehaviour
{
	[SerializeField]
	private float m_moveTime = 1.0f;

	[SerializeField]
	private Animator m_animator;

	private float m_moveSpeed;

    public enum State {
		Wait,
		Walking,
	}


	private struct MoveInfo {
		public int nextPointIndex;
	}

	// �ړ����
	private MoveInfo m_moveInfo;

	// ���
	private State m_state = State.Wait;

	// �ړ����[�g
	UnitWalkRoute unitWalkRoute = null;

	UnityAction m_onExitMoveEvent;

	/// <summary>
	/// �����C�x���g�o�^
	/// </summary>
	/// <param name="exitMoveEvent"></param>
	public void RegisterExitMoveEvent(UnityAction exitMoveEvent) {
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

		CulcMoveSpeed();
		m_state = State.Walking;
	}

	private void UpdateWalk() {
		Vector3 target = unitWalkRoute.pointList[m_moveInfo.nextPointIndex];
		Vector3 direction = target - transform.position;

		// �ړ�
		transform.position = Vector3.MoveTowards(transform.position, target, m_moveSpeed * Time.deltaTime);

		// ���B�`�F�b�N
		if (direction.magnitude < 0.1f) {
			transform.position = target;
			m_moveInfo.nextPointIndex++;

			// ���B
			if (m_moveInfo.nextPointIndex >= unitWalkRoute.pointList.Count) {
				unitWalkRoute = null;
				m_onExitMoveEvent?.Invoke();
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

	private void CulcMoveSpeed() {
		if (unitWalkRoute == null) { return; }

		float distance = 0.0f;

		for (int i = 0; i < unitWalkRoute.pointList.Count - 1; ++i) {
			Vector3 from = unitWalkRoute.pointList[i];
			Vector3 to = unitWalkRoute.pointList[i + 1];
			distance += Vector3.Distance(from, to);
		}

		m_moveSpeed = distance / m_moveTime;
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
