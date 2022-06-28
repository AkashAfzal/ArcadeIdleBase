using System.Collections;
using UnityEngine;


public class FightController : AIBase
{

	//Inspector Fields
	[SerializeField] private float     detectionRadius;
	[SerializeField] private float     stoppingDistance;
	[SerializeField] private int       attackDamage;
	[SerializeField] private LayerMask opponentLayerMask;
	[SerializeField] private Hitbox[]  hitBoxes;


	//Private Fields
	bool               DetectOpponent;
	private Collider[] m_Opponents = new Collider[5];


	FightLeader leader;

	//Properties
	Transform Target { get; set; }

	bool _canMove;
	bool CanMove
	{
		get
		{
			_canMove = Target != null && (Vector3.Distance(transform.position, Target.position) > stoppingDistance);
			return _canMove;
		}

		set => _canMove = value;
	}

	bool CanAttack => Target != null && (Vector3.Distance(transform.position, Target.position) < stoppingDistance);


	//Methods
	protected override void Init()
	{
		SetDamageForHitBoxes();
	}

	private void SetDamageForHitBoxes()
	{
		foreach (var hitBox in hitBoxes)
		{
			hitBox.damage = attackDamage;
		}
	}

	protected override void OnUpdate()
	{
		Attack();
		if (Target == null && DetectOpponent && Physics.OverlapSphereNonAlloc(transform.position, detectionRadius, m_Opponents, opponentLayerMask) > 0)
		{
			Debug.Log($"{gameObject.name} FIndNewTarget");
			SetTarget(FindNearestOpponent());
		}
	}

	private Transform FindNearestOpponent()
	{
		float     distance      = Vector3.Distance(transform.position, m_Opponents[0].transform.position);
		Transform nearestTarget = m_Opponents[0].transform;
		if (m_Opponents.Length <= 0) return nearestTarget;
		foreach (Collider opponent in m_Opponents)
		{
			if (opponent == null) continue;
			var tempDistance = Vector3.Distance(transform.position, opponent.transform.position);
			if (tempDistance < distance)
			{
				distance      = tempDistance;
				nearestTarget = opponent.transform;
			}
		}

		return nearestTarget;
	}

	protected override void OnFixedUpdate()
	{
		if (Target == null) return;
		Move(Target.position, Target.position, movementSpeed, CanMove);
	}

	public void SetTarget(Transform target)
	{
		if (Target != null || target == null) return;
		DetectOpponent = true;
		Target         = target;
		HitBoxesTarget();
	}

	void HitBoxesTarget()
	{
		foreach (var hitBox in hitBoxes)
		{
			hitBox.AttackTarget(Target);
		}
	}

	private void Attack()
	{
		Animator.SetBool("Attack", CanAttack);
	}


	public void StopFight()
	{
		Animator.SetBool("Attack", false);
		DetectOpponent = false;
		Target         = null;
	}

}