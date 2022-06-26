using GameDevUtils.CharacterController;
using GameDevUtils.StackSystem;
using UnityEngine;


enum MovementState
{

	Follow,
	MoveToTarget

}


public class Follower : AIBase, IStackObject
{

	//IStackObject Implementation
	public string     ID          => "Follower";
	public GameObject _GameObject => gameObject;

	public void SetPositionRotation(Vector3 position, Quaternion rotation)
	{
		MoveToTarget(position);
	}

	//Private Fields
	private bool                   StopAtTargetPos;
	private Leader                 Leader;
	private Transform              FollowTarget;
	private Vector3                MoveToTargetPosition;
	private CaptureCharacter       CaptureCharacter;
	private FreeMovementController PlayerController;
	private MovementState          MovementState = MovementState.MoveToTarget;


	//Properties
	public Leader GetLeader => Leader;

	bool _canMove;
	bool CanMove
	{
		get
		{
			switch (MovementState)
			{
				case MovementState.MoveToTarget when Vector3.Distance(transform.position, MoveToTargetPosition) < 0.7f:
					StopAtTargetPos = false;
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
				MovementState.MoveToTarget                                                                                      => MoveToTargetPosition,
				MovementState.Follow when PlayerController != null && PlayerController.IsPlayerInputApplied                     => FollowTarget.position,
				MovementState.Follow when PlayerController != null && !PlayerController.IsPlayerInputApplied && isAgentStopped  => PlayerController.gameObject.transform.position,
				_                                                                                                               => _lookAtTarget
			};
			return _lookAtTarget;
		}
	}


	protected override void Init()
	{
		CaptureCharacter = GetComponent<CaptureCharacter>();
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


	public void ActiveCharacter(Leader target, bool forceFollow = false)
	{
		if (forceFollow) StopAtTargetPos = false;
		if (StopAtTargetPos || Leader == target) return;
		CaptureCharacter.SetLeader(target);
		Leader = target;
		FollowToTarget();
	}


	public void FollowToTarget()
	{
		CanMove          = true;
		MovementState    = MovementState.Follow;
		FollowTarget     = Leader.transform;
		PlayerController = Leader.GetComponent<FreeMovementController>();
		Agent.SetDestination(FollowTarget.position);
	}

	public void MoveToTarget(Vector3 targetPos, bool stopAtTargetPos)
	{
		StopAtTargetPos = stopAtTargetPos;
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
		if (MovementState == MovementState.Follow && PlayerController != null && !PlayerController.IsPlayerInputApplied && other.CompareTag("Follower"))
		{
			if (other.GetComponent<Follower>().isAgentStopped)
				CanMove = false;
		}

		if (MovementState == MovementState.MoveToTarget && !StopAtTargetPos && other.CompareTag("Follower"))
		{
			if (other.GetComponent<Follower>().isAgentStopped)
				CanMove = false;
		}
	}

}