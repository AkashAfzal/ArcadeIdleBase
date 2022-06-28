using System.Collections.Generic;
using GameDevUtils.StackSystem;
using UnityEngine;

public class EnemyGroup : MonoBehaviour
{

	[SerializeField] private float       detectRadius;
	[SerializeField] private LayerMask   opponentLayerMask;
	public                   List<Enemy> allEnemies;


	private bool       attacking;
	private Collider[] m_Opponents = new Collider[3];


	public delegate void OnAttackDelegate(Transform target);

	public event OnAttackDelegate OnGroupAttackInvoke;


	public delegate void OnAttackStopDelegate();

	public event OnAttackStopDelegate OnGroupStopAttackInvoke;


	FightLeader _fightLeader;
	FightLeader fightLeader
	{
		get
		{
			if (_fightLeader == null)
			{
				_fightLeader = FindObjectOfType<FightLeader>();
			}

			return _fightLeader;
		}
	}
	StackManager _playerStack;
	StackManager PlayerStack
	{
		get
		{
			if (_playerStack == null)
			{
				_playerStack = fightLeader.GetComponent<StackManager>();
			}

			return _playerStack;
		}
	}

	void Start()
	{
		SetEnemyGroupFoAllEnemies();
	}

	void SetEnemyGroupFoAllEnemies()
	{
		foreach (var enemy in allEnemies)
		{
			enemy.SetEnemyGroup(this);
		}
	}

	public void GroupEnemyDead(Enemy enemy)
	{
		if (allEnemies.Contains(enemy))
		{
			allEnemies.Remove(enemy);
			if (allEnemies.Count == 0)
			{
				fightLeader.OnGroupAllEnemiesDead(this);
				Destroy(this.gameObject);
			}
		}
	}


	void Update()
	{
		if (!attacking && Physics.OverlapSphereNonAlloc(transform.position, detectRadius, m_Opponents, opponentLayerMask) > 0)
		{
			attacking = true;
			fightLeader.AddFightingGroup(this);
			StartFightSetTarget();
		}

		if (attacking && PlayerStack.IsStackEmpty("Followers") && Physics.OverlapSphereNonAlloc(transform.position, detectRadius, m_Opponents, opponentLayerMask) == 0)
		{
			attacking = false;
			OnGroupStopAttackInvoke?.Invoke();
		}
	}


	private void StartFightSetTarget()
	{
		if (!PlayerStack.IsStackEmpty("Followers"))
		{
			foreach (Enemy enemy in allEnemies)
			{
				enemy.SetTarget(PlayerStack.AllElementsOfStack("Followers")[Random.Range(0, PlayerStack.AllElementsOfStack("Followers").Count)]._GameObject.transform);
			}
		}
		else
		{
			OnGroupAttackInvoke?.Invoke(m_Opponents[0].transform);
		}
	}

}