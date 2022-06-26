using UnityEngine;
using UnityEngine.AI;
using GameDevUtils.HealthSystem;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
public abstract class AIBase : MonoBehaviour, IDamageable
{
	
	[SerializeField] private float        movementSpeed = 1;
	[SerializeField] private NavMeshAgent agent;
	[SerializeField] private Animator     animator;
	[SerializeField]         HealthSystem healthSystem;
	
	public Animator     Animator    => animator;
	public NavMeshAgent Agent       => agent;

	public bool isAgentStopped => Agent.isStopped;
	public bool IsDestroyed    { get; set; }

	void Start()
	{
		Init();
	}
	
	void Update()
	{
		OnUpdate();
	}
	
	void FixedUpdate()
	{
		OnFixedUpdate();
	}

	protected abstract void Init();
	protected abstract void OnUpdate();

	protected abstract void OnFixedUpdate();

	protected void Move(Vector3 targetPosition, Vector3 lookAtTarget, bool canMove)
	{
		agent.isStopped = !canMove;
		if (canMove && agent.isOnNavMesh)
		{
			agent.speed     = movementSpeed;
			agent.SetDestination(targetPosition);
		}
		transform.LookAt(lookAtTarget);
		Animator.SetFloat("Value", canMove ? movementSpeed : 0);
	}

	public void Damage(float damageAmount, Vector3 hitPoint)
	{
		healthSystem.TakeDamage(damageAmount, hitPoint);
	}

	public void DestroyObject()
	{
		healthSystem.Death();
	}

}