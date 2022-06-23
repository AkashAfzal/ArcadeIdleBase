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
	// public delegate void     OnSetLeader(Leader leader);
	// public event OnSetLeader onSetLeader;

	public string     ID          => "Follower";
	public GameObject _GameObject => gameObject;

	// [SerializeField] Follower movement;

	public void SetPositionRotation(Vector3 position, Quaternion rotation)
	{
		MoveToTarget(position);
	}


	private CaptureCharacter CaptureCharacter;
	private Leader           Leader;

	public Leader GetLeader => Leader;
	

	public void ActiveCharacter(Leader target, bool forceFollow = false)
	{
		if (forceFollow) StopAtTargetPos = false;
		if (StopAtTargetPos || Leader == target) return;
		// if (HasLeader()) Leader.MinusAgent();
		CaptureCharacter.SetLeader(target);
		// target.AddAgent();
		Leader = target;
		// onSetLeader?.Invoke(target);
	}

	// public bool HasLeader() => Leader != null;
	

	[SerializeField]  private Transform     target;

	[HideInInspector] public bool StopAtTargetPos;
	bool                          moveToTarget;
	Vector3                       targetPosition;
	Vector3                       lookAtTarget;

	public  FreeMovementController playerController;

	bool canMove;

	MovementState movementState = MovementState.MoveToTarget;

	private Vector3 AgentTargetPosition
	{
		get
		{
			if (movementState == MovementState.MoveToTarget)
				return targetPosition;
			else if (movementState == MovementState.Follow && target != null)
				return target.position;
			else
				return AgentTargetPosition;
			
		}
	}
	
	

	protected override void Init()
	{
		CaptureCharacter = GetComponent<CaptureCharacter>();
		if (target != null && Agent.isOnNavMesh)
			Agent.SetDestination(target.position);
	}

	protected override void OnUpdate()
	{
	}

	protected override void OnFixedUpdate()
	{
		if (movementState == MovementState.Follow && target != null && playerController != null && playerController.IsPlayerInputApplied)
			canMove = true;

		Move(AgentTargetPosition, canMove);

		LookAt();
		StopAgent();
		
	}

	private void LookAt()
	{
		lookAtTarget = moveToTarget switch
		{
			true                                                                                             => targetPosition,
			false when playerController != null && playerController.IsPlayerInputApplied                     => target.position,
			false when playerController != null && !playerController.IsPlayerInputApplied && Agent.isStopped => playerController.gameObject.transform.position,
			_                                                                                                => lookAtTarget
		};
		transform.LookAt(lookAtTarget);
	}

	void StopAgent()
	{
		if (moveToTarget && Vector3.Distance(transform.position, targetPosition) < 0.7f)
		{
			canMove         = false;
			StopAtTargetPos = false;
			moveToTarget    = false;
		}
		else if (playerController != null && playerController.IsPlayerInputApplied == false && Vector3.Distance(transform.position, target.position) < 0.7f)
		{
			canMove = false;
		}
		else if (playerController != null && playerController.IsPlayerInputApplied == true)
		{
			canMove = true;
		}
	}


	public void SetFollowTarget(Leader leader)
	{
		canMove          = true;
		moveToTarget     = false;
		target           = leader.transform;
		playerController = leader.GetComponent<FreeMovementController>();
		if (Agent.isOnNavMesh)
			Agent.SetDestination(target.position);
	}

	public void MoveToTarget(Vector3 targetPos, bool stopAtTargetPos)
	{
		StopAtTargetPos = stopAtTargetPos;
		MoveToTarget(targetPos);
	}

	public void MoveToTarget(Vector3 targetPos)
	{
		canMove        = true;
		movementState  = MovementState.MoveToTarget;
		moveToTarget   = true;
		targetPosition = targetPos;
	}


	void OnTriggerStay(Collider other)
	{
		if (!moveToTarget && playerController != null && !playerController.IsPlayerInputApplied && other.CompareTag("Follower"))
		{
			if (other.GetComponent<Follower>().Agent.isStopped)
				Agent.isStopped = true;
		}

		if (!StopAtTargetPos && moveToTarget && other.CompareTag("Follower"))
		{
			if (other.GetComponent<Follower>().Agent.isStopped)
				Agent.isStopped = true;
		}
	}

}