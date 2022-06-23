using UnityEngine;


public enum AttackState
{

	None,
	Attacking,
	SearchForTarget,
	MoveToWardsTarget

}

public abstract class AttackBase : MonoBehaviour, IAttack
{

	[SerializeField] protected AttackState      attackState = AttackState.None;
	[SerializeField] protected FollowerMovement followerMovement;
	[SerializeField] protected int              damage;
	[SerializeField] protected float            searchRadius;
	[SerializeField]           Hitbox[]         hitBoxes;
	protected                  GameObject       target;


	void Start()
	{
		Init();
	}

	protected virtual void Init()
	{
		SetDamageForHitBoxes();
	}

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