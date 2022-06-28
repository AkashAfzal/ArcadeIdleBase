using System;
using GameDevUtils.HealthSystem;
using UnityEngine;

public enum EnemyMovementState
{

	Idle,
	MoveTowardTarget,
	ReturnBack

}

public class Enemy : AIBase, IDamageable
{

	//Inspector Fields
	[SerializeField] private float        detectionRadius;
	[SerializeField] private float        stoppingDistance;
	[SerializeField] private int          attackDamage;
	[SerializeField] private LayerMask    opponentLayerMask;
	[SerializeField] private Hitbox[]     hitBoxes;
	[SerializeField] private HealthSystem healthSystem;
	EnemyMovementState                    EnemyMovementState = EnemyMovementState.Idle;


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
	float   AgentMovementSpeed  => EnemyMovementState == EnemyMovementState.MoveTowardTarget ? movementSpeed : movementSpeed / 2;
	bool    CanAttack           => Target             != null && (EnemyMovementState == EnemyMovementState.MoveTowardTarget && Vector3.Distance(transform.position, Target.position) < stoppingDistance);


	//Methods
	protected override void Init()
	{
		initialPos           =  transform.position;
		healthSystem.OnDeath += OnDead;
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
		if (Target != null) return;
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
	
	public void Damage(float damageAmount, Vector3 hitPoint)
	{
		healthSystem.TakeDamage(damageAmount, hitPoint);
	}

	private void OnDead()
	{
		enemyGroup.GroupEnemyDead(this);
		Destroy(gameObject);
	}

	public void DestroyObject()
	{
		healthSystem.Death();
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