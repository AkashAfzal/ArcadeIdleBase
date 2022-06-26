using UnityEngine;


public class AnimatorIDs
{

	public static readonly int k_MovementValue       = Animator.StringToHash("Value");
	public static readonly int k_ActionIndex         = Animator.StringToHash("Action");
	public static readonly int k_GetHurtTrigger      = Animator.StringToHash("GetHurt");
	public static readonly int k_DoAnimationAction   = Animator.StringToHash("DoAction");
	public static readonly int k_CanRevive           = Animator.StringToHash("CanRevive");
	public static readonly int k_DeathAnimationIndex = 6;
	public static readonly int AttackTrigger         = Animator.StringToHash("Attack");

}

public enum AttackState
{

	None,
	Attacking,
	SearchForTarget,
	MoveToWardsTarget

}

public abstract class AttackBase : MonoBehaviour, IAttack
{
	
	[SerializeField] protected AttackState attackState = AttackState.None;
	[SerializeField] protected bool        canDetectEnemy;
	[SerializeField] protected LayerMask   opponentLayerMask;
	[SerializeField] protected AIBase      controller;
	[SerializeField] protected int         damage;
	[SerializeField] protected float       attackRadius;
	[SerializeField] protected float       searchRadius;
	[SerializeField]           Hitbox[]    hitBoxes;
	
	[SerializeField] private ParticleSystem m_PunchParticles;
	
	protected Collider[] m_NearEnemies = new Collider[5];
	protected AttackBase nearFightController;
	protected bool       CanFight = false;
	protected GameObject target;
	//
	//
	// void Start()
	// {
	// 	Init();
	// }
	//
	// protected virtual void Init()
	// {
	// 	SetDamageForHitBoxes();
	// }
	//
	//
	// protected virtual void FixedUpdate()
	// {
	// 	var detectionLevel = EnemyDetection();
	// 	if (target && !controller.Animator.applyRootMotion && (CurrentState is {CanDoAction: true} || attackState == AttackState.MoveToWardsTarget))
	// 	{
	// 		Movement(detectionLevel);
	// 	}
	// }
	//
	// protected virtual float EnemyDetection()
	// {
	// 	
	// 	if (Physics.OverlapSphereNonAlloc(transform.position, attackRadius, m_NearEnemies, opponentLayerMask) > 0)
	// 	{
	// 		controller
	// 	}
	//
	// 	if (Physics.OverlapSphereNonAlloc(transform.position, m_Parameters.sneakRadius, m_NearEnemies, m_EnemyLayerMask) > 0)
	// 	{
	// 		detectionLevel = 0.5f;
	// 		return detectionLevel;
	// 	}
	//
	// 	detectionLevel = 1;
	// 	return detectionLevel;
	// }
	//
	//
	// protected virtual void Movement(float i_DetectionLevel)
	// {
	// 	var currentPosition = transform.position;
	// 	var target          = Target.position;
	// 	//	target.y = currentPosition.y;
	// 	var direction      = (target - currentPosition).normalized;
	// 	var deltaChange    = direction * (m_Parameters.moveSpeed * i_DetectionLevel * Time.deltaTime);
	// 	var desirePosition = currentPosition + deltaChange;
	// 	Rigidbody.MovePosition(desirePosition);
	// 	transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
	// 	UpdateMovementAnimation(i_DetectionLevel * 1.5f);
	// }
	//
	// protected void UpdateMovementAnimation(float i_MovementValue)
	// {
	// 	Animator.SetFloat(AnimatorIDs.k_MovementValue, i_MovementValue);
	// }
	//
	// #endregion
	//
	//
	// #region AnimationActions
	//
	// public virtual void Attack(int i_fightStateIndex)
	// {
	// 	if (!m_NearEnemies[0]) return;
	// 	nearFightController = m_NearEnemies[0].GetComponent<FightController>();
	// 	if (CanFight && CurrentState is {CanDoAction: true} && nearFightController.CurrentState is {CanDoAction: true})
	// 	{
	// 		CurrentState = m_FightStates[i_fightStateIndex];
	// 		m_FightStates[i_fightStateIndex].Attack(Animator, Rigidbody, i_fightStateIndex);
	// 	}
	// }

	public abstract void Attack();


	public abstract void Search();


	public abstract void MoveForAttack();

	private void SetDamageForHitBoxes()
	{
		foreach (var hitBox in hitBoxes)
		{
			hitBox.damage = damage;
		}
	}

}