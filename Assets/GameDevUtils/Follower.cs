using GameDevUtils.HealthSystem;
using GameDevUtils.StackSystem;
using UnityEngine;

public class Follower : MonoBehaviour, IStackObject, IDamageable
{

	//IStackObject Implementation
	public string     ID          => "Follower";
	public GameObject _GameObject => gameObject;

	public void SetPositionRotation(Vector3 position, Quaternion rotation)
	{
		Movement.MoveToTarget(position);
	}

	//IDamageable Implementation 
	public bool IsDestroyed { get; set; }

	public void Damage(float damageAmount, Vector3 hitPoint)
	{
		healthSystem.TakeDamage(damageAmount, hitPoint);
	}

	public void DestroyObject()
	{
		healthSystem.Death();
	}


	[SerializeField] private HealthSystem     healthSystem;
	private                  FightLeader      FightLeader;
	private                  FollowerMovement Movement;
	private                  FightController  fightController;
	private                  CaptureCharacter captureCharacter;

	public FightLeader GetFightLeader => FightLeader;

	void Start()
	{
		Movement                 =  GetComponent<FollowerMovement>();
		fightController          =  GetComponent<FightController>();
		captureCharacter         =  GetComponent<CaptureCharacter>();
		Movement.enabled         =  true;
		fightController.enabled  =  false;
		captureCharacter.enabled =  false;
		healthSystem.OnDeath     += OnDeath;
	}

	public void ActiveCharacter(FightLeader target, bool forceFollow = false)
	{
		if (forceFollow) Movement.stopAtTargetPos = false;
		if (Movement.stopAtTargetPos || FightLeader == target) return;
		captureCharacter.SetLeader(target);
		FightLeader = target;
		Movement.SetFollowTarget(target.transform);
	}

	public void StartFighting(Transform target)
	{
		Movement.enabled         = false;
		captureCharacter.enabled = false;
		fightController.SetTarget(target);
		fightController.enabled = true;
	}


	public void StopFighting()
	{
		fightController.StopFight();
		fightController.enabled  = false;
		captureCharacter.enabled = true;
		Movement.enabled         = true;
	}


	private void OnDeath()
	{
		FightLeader.playerStack.RemoveStack("Followers", this);
		Destroy(gameObject);
	}

}