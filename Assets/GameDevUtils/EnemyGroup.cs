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


	FightLeader  fightLeader;
	StackManager _playerStack;
	StackManager PlayerStack
	{
		get
		{
			if (_playerStack == null)
			{
				fightLeader  = FindObjectOfType<FightLeader>();
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
		}
	}


	void Update()
	{
		if (!attacking && Physics.OverlapSphereNonAlloc(transform.position, detectRadius, m_Opponents, opponentLayerMask) > 0)
		{
			attacking = true;
			OnGroupAttackInvoke?.Invoke(m_Opponents[0].transform);
		}

		if (attacking && PlayerStack.IsStackEmpty("Followers") && Physics.OverlapSphereNonAlloc(transform.position, detectRadius, m_Opponents, opponentLayerMask) == 0)
		{
			attacking = false;
			OnGroupStopAttackInvoke?.Invoke();
		}
	}

}