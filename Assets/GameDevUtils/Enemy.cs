using UnityEngine;

public enum EnemyMovementState
{

	Idle,
	MoveTowardTarget,
	ReturnBack

}

public class Enemy : AIBase
{

	[SerializeField] float stoppingDistance;
	[SerializeField] float coolDownDistance;
	EnemyMovementState     EnemyMovementState = EnemyMovementState.Idle;

	Vector3   initialPos;
	Transform _target;
	Transform Target
	{
		get
		{
			if (Vector3.Distance(transform.position, _target.position) > coolDownDistance)
			{
				_target            = null;
				CanMove            = true;
				EnemyMovementState = EnemyMovementState.ReturnBack;
			}

			return _target;
		}
		set => _target = value;
	}


	bool _canMove;
	bool CanMove
	{
		get
		{
			if (EnemyMovementState == EnemyMovementState.MoveTowardTarget && Vector3.Distance(transform.position, Target.position) < stoppingDistance)
				_canMove = false;
			if (EnemyMovementState == EnemyMovementState.ReturnBack && Vector3.Distance(transform.position, initialPos) < stoppingDistance)
			{
				EnemyMovementState = EnemyMovementState.Idle;
				_canMove           = false;
			}
			return _canMove;
		}

		set => _canMove = value;
	}

	Vector3 AgentTargetPosition => EnemyMovementState == EnemyMovementState.MoveTowardTarget ? Target.position : initialPos;

	protected override void Init()
	{
		initialPos = transform.position;
	}

	protected override void OnUpdate()
	{
	}

	protected override void OnFixedUpdate()
	{
		Move(AgentTargetPosition, AgentTargetPosition, movementSpeed, CanMove);
	}

	public void SetTarget(Transform target)
	{
		if(Target == null) return;
		Target             = target;
		EnemyMovementState = EnemyMovementState.MoveTowardTarget;
	}

}