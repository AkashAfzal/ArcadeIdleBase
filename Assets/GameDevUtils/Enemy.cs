using System;
using UnityEngine;

public enum EnemyMovementState
{

	Idle,
	MoveTowardTarget,
	ReturnBack

}

public class Enemy : AIBase
{

	//Inspector Fields
	[SerializeField] private float     detectionRadius;
	[SerializeField] private float     stoppingDistance;
	[SerializeField] private int       attackDamage;
	[SerializeField] private LayerMask opponentLayerMask;
	[SerializeField] private Hitbox[]  hitBoxes;
	EnemyMovementState                 EnemyMovementState = EnemyMovementState.Idle;


	//Private Fields
	EnemyGroup         enemyGroup;
	Vector3            initialPos;
	bool               DetectOpponent;
	private Collider[] m_Opponents = new Collider[5];


	//Properties
	Transform Target { get; set; }

	bool _canMove;
	bool CanMove
	{
		get
		{
			if (EnemyMovementState == EnemyMovementState.MoveTowardTarget && Vector3.Distance(transform.position, Target.position) < stoppingDistance)
			{
				_canMove = false;
			}
			else if (EnemyMovementState == EnemyMovementState.MoveTowardTarget && Vector3.Distance(transform.position, Target.position) > stoppingDistance)
			{
				_canMove = true;
			}

			if (EnemyMovementState == EnemyMovementState.ReturnBack && Vector3.Distance(transform.position, initialPos) < stoppingDistance)
			{
				EnemyMovementState = EnemyMovementState.Idle;
				_canMove           = false;
			}
			else if (EnemyMovementState == EnemyMovementState.ReturnBack && Vector3.Distance(transform.position, initialPos) > stoppingDistance)
			{
				_canMove = true;
			}

			return _canMove;
		}

		set => _canMove = value;
	}


	Vector3 AgentTargetPosition => EnemyMovementState == EnemyMovementState.MoveTowardTarget ? Target.position : initialPos;

	float AgentMovementSpeed => EnemyMovementState == EnemyMovementState.MoveTowardTarget ? movementSpeed : movementSpeed / 2;
	bool  CanAttack          => Target             != null && (EnemyMovementState == EnemyMovementState.MoveTowardTarget && Vector3.Distance(transform.position, Target.position) < stoppingDistance);


	//Methods
	protected override void Init()
	{
		initialPos = transform.position;
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
			SetTarget(FindNearestOpponent());
		}
	}

	private Transform FindNearestOpponent()
	{
		float     distance      = 10000;
		Transform nearestTarget = null;
		foreach (var opponent in m_Opponents)
		{
			var tempDistance = Vector3.Distance(transform.position, opponent.transform.position);
			if (distance < tempDistance)
			{
				distance      = tempDistance;
				nearestTarget = opponent.transform;
			}
		}

		return nearestTarget;
	}

	protected override void OnFixedUpdate()
	{
		Move(AgentTargetPosition, AgentTargetPosition, AgentMovementSpeed, CanMove);
	}

	public void SetEnemyGroup(EnemyGroup group)
	{
		enemyGroup                         =  group;
		enemyGroup.OnGroupAttackInvoke     += SetTarget;
		enemyGroup.OnGroupStopAttackInvoke += StopFight;
	}

	public void SetTarget(Transform target)
	{
		if (Target == null) return;
		DetectOpponent     = true;
		Target             = target;
		HitBoxesTarget();
		EnemyMovementState = EnemyMovementState.MoveTowardTarget;
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


	private void StopFight()
	{
		DetectOpponent     = false;
		Target             = null;
		EnemyMovementState = EnemyMovementState.ReturnBack;
	}


	void OnDisable()
	{
		enemyGroup.OnGroupAttackInvoke     -= SetTarget;
		enemyGroup.OnGroupStopAttackInvoke -= StopFight;
	}

	void OnDestroy()
	{
		enemyGroup.OnGroupAttackInvoke     -= SetTarget;
		enemyGroup.OnGroupStopAttackInvoke -= StopFight;
	}

}