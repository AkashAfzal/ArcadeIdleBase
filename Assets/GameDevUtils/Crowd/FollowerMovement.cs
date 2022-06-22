using System;
using GameDevUtils.CharacterController;
using UnityEngine;
using UnityEngine.AI;


public enum FollowerState
{

	Idle,
	MoveToTarget,
	Following,
	Attack,
	Dead

}

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class FollowerMovement : MonoBehaviour
{

	[SerializeField]  private float        speedRun = 1;
	[SerializeField]  private Transform    target;
	[HideInInspector] public  NavMeshAgent agent;

	[HideInInspector] public bool    StopAtTargetPos;
	bool    moveToTarget;
	Vector3 targetPosition;
	Vector3 lookAtTarget;

	public  FreeMovementController playerController;
	private Follower               follower;


	private Animator animator;
	public Animator Animator
	{
		get
		{
			if (animator == null)
			{
				var result = this.GetComponent<Animator>();
				if (result == null)
				{
					result = this.GetComponentInChildren<Animator>();
				}

				animator = result;
			}

			return animator;
		}
	}

	void Awake()
	{
		agent                =  GetComponent<NavMeshAgent>();
		follower             =  GetComponent<Follower>();
		follower.onSetLeader += SetFollowTarget;
		agent.speed          =  speedRun;
		if (target != null && agent.isOnNavMesh)
			agent.SetDestination(target.position);
	}


	void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		if (!moveToTarget && playerController != null && playerController.IsPlayerInputApplied && target != null)
		{
			agent.SetDestination(target.position);
		}

		if (moveToTarget && agent.isOnNavMesh)
		{
			agent.SetDestination(targetPosition);
		}

		Animator.SetFloat("Value", !agent.isStopped ? speedRun : 0);
		LookAt();
		StopAgent();
	}

	private void LookAt()
	{
		lookAtTarget = moveToTarget switch
		{
			true                                                                                             => targetPosition,
			false when playerController != null && playerController.IsPlayerInputApplied                     => target.position,
			false when playerController != null && !playerController.IsPlayerInputApplied && agent.isStopped => playerController.gameObject.transform.position,
			_                                                                                                => lookAtTarget
		};
		transform.LookAt(lookAtTarget);
	}

	void StopAgent()
	{
		if (moveToTarget && Vector3.Distance(transform.position, targetPosition) < 0.7f)
		{
			agent.isStopped = true;
			StopAtTargetPos = false;
			moveToTarget    = false;
		}
		else if (playerController != null && playerController.IsPlayerInputApplied == false && Vector3.Distance(transform.position, target.position) < 0.7f)
		{
			agent.isStopped = true;
		}
		else if (playerController != null && playerController.IsPlayerInputApplied == true)
		{
			agent.isStopped = false;
		}
	}


	public void SetFollowTarget(Leader leader)
	{
		target           = leader.transform;
		playerController = leader.GetComponent<FreeMovementController>();
		if (agent.isOnNavMesh)
			agent.SetDestination(target.position);
	}

	public void MoveToTarget(Vector3 targetPos, bool stopAtTargetPos)
	{
		StopAtTargetPos = stopAtTargetPos;
		MoveToTarget(targetPos);
	}

	public void MoveToTarget(Vector3 targetPos)
	{
		agent.isStopped = false;
		moveToTarget    = true;
		targetPosition  = targetPos;
	}


	void OnTriggerStay(Collider other)
	{
		if (!moveToTarget && playerController != null && !playerController.IsPlayerInputApplied && other.CompareTag("Follower"))
		{
			if (other.GetComponent<FollowerMovement>().agent.isStopped)
				agent.isStopped = true;
		}

		if (!StopAtTargetPos && moveToTarget && other.CompareTag("Follower"))
		{
			if (other.GetComponent<FollowerMovement>().agent.isStopped)
				agent.isStopped = true;
		}
	}

}