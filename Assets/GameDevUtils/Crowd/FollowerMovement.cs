using GameDevUtils.CharacterController;
using UnityEngine;


enum MovementState
{

	Follow,
	MoveToTarget

}


public class FollowerMovement : AIBase
{

	
	[HideInInspector] public  bool         stopAtTargetPos;

	//Private Fields
	private Transform              FollowTarget;
	private Vector3                MoveToTargetPosition;
	private FreeMovementController PlayerController;
	private MovementState          MovementState = MovementState.MoveToTarget;


	//Properties

	bool _canMove;
	bool CanMove
	{
		get
		{
			switch (MovementState)
			{
				case MovementState.MoveToTarget when Vector3.Distance(transform.position, MoveToTargetPosition) < 0.7f:
					stopAtTargetPos = false;
					_canMove        = false;
					return _canMove;

				case MovementState.Follow when PlayerController != null && PlayerController.IsPlayerInputApplied == false && Vector3.Distance(transform.position, FollowTarget.position) < 0.7f:
					_canMove = false;
					return _canMove;

				case MovementState.Follow when FollowTarget != null && PlayerController != null && PlayerController.IsPlayerInputApplied:
					_canMove = true;
					return _canMove;

				default:
					return _canMove;
			}
		}

		set => _canMove = value;
	}

	private Vector3 AgentTargetPosition
	{
		get
		{
			if (MovementState == MovementState.MoveToTarget)
				return MoveToTargetPosition;
			else if (MovementState == MovementState.Follow && FollowTarget != null)
				return FollowTarget.position;
			else
				return AgentTargetPosition;
		}
	}

	Vector3 _lookAtTarget;

	Vector3 LookAtTarget
	{
		get
		{
			_lookAtTarget = MovementState switch
			{
				MovementState.MoveToTarget                                                                                     => MoveToTargetPosition,
				MovementState.Follow when PlayerController != null && PlayerController.IsPlayerInputApplied                    => FollowTarget.position,
				MovementState.Follow when PlayerController != null && !PlayerController.IsPlayerInputApplied && isAgentStopped => PlayerController.gameObject.transform.position,
				_                                                                                                              => _lookAtTarget
			};
			return _lookAtTarget;
		}
	}


	protected override void Init()
	{
		if (MovementState == MovementState.Follow && FollowTarget != null && Agent.isOnNavMesh)
			Agent.SetDestination(FollowTarget.position);
	}

	protected override void OnUpdate()
	{
	}

	protected override void OnFixedUpdate()
	{
		Move(AgentTargetPosition, LookAtTarget, movementSpeed, CanMove);
	}


	public void SetFollowTarget(Transform target)
	{
		CanMove          = true;
		MovementState    = MovementState.Follow;
		FollowTarget     = target.transform;
		PlayerController = target.GetComponent<FreeMovementController>();
		Agent.SetDestination(FollowTarget.position);
	}

	public void MoveToTarget(Vector3 targetPos, bool isStopAtTargetPos)
	{
		this.stopAtTargetPos = isStopAtTargetPos;
		MoveToTarget(targetPos);
	}

	public void MoveToTarget(Vector3 targetPos)
	{
		CanMove              = true;
		MovementState        = MovementState.MoveToTarget;
		MoveToTargetPosition = targetPos;
	}

	void OnTriggerStay(Collider other)
	{
		if(!this.enabled) return;
		if (MovementState == MovementState.Follow && PlayerController != null && !PlayerController.IsPlayerInputApplied && other.CompareTag("Follower"))
		{
			if (other.GetComponent<FollowerMovement>().isAgentStopped)
				CanMove = false;
		}

		if (MovementState == MovementState.MoveToTarget && !stopAtTargetPos && other.CompareTag("Follower"))
		{
			if (other.GetComponent<FollowerMovement>().isAgentStopped)
				CanMove = false;
		}
	}

}