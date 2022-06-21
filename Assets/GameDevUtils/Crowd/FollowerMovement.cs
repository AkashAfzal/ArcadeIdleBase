using System;
using GameDevUtils.CharacterController;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(NavMeshAgent))]
public class FollowerMovement : MonoBehaviour
{

	[SerializeField] private float        speedRun = 1;
	[SerializeField] private Transform    target;
	Rigidbody                             rigifBody;
	[HideInInspector] public NavMeshAgent agent;
	bool                                  moveToTarget;
	Vector3                               targetPosition;
	Vector3                               lookAtTarget;

	public FreeMovementController playerController;


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
		rigifBody   = GetComponent<Rigidbody>();
		agent       = GetComponent<NavMeshAgent>();
		agent.speed = speedRun;
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
			if (Vector3.Distance(transform.position, targetPosition) < 0.5f)
			{
				moveToTarget    = false;
				agent.isStopped = true;
			}
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
		agent.isStopped = playerController != null && playerController.IsPlayerInputApplied switch
		{
			false when Vector3.Distance(transform.position, target.position) < 0.7f => true,
			true                                                                    => false,
			_                                                                       => agent.isStopped
		};
	}


	public void SetFollowTarget(Transform followTarget, FreeMovementController controller)
	{
		target           = followTarget;
		playerController = controller;
		agent.SetDestination(target.position);
	}

	public void MoveToTarget(Vector3 targetPos)
	{
		moveToTarget   = true;
		targetPosition = targetPos;
	}


	void OnTriggerStay(Collider other)
	{
		if (!moveToTarget && !playerController.IsPlayerInputApplied && other.CompareTag("Follower"))
		{
			if (other.GetComponent<FollowerMovement>().agent.isStopped)
				agent.isStopped = true;
		}
		
		if (moveToTarget && other.CompareTag("Follower"))
		{
			if (other.GetComponent<FollowerMovement>().agent.isStopped)
				agent.isStopped = true;
		}
	}

}