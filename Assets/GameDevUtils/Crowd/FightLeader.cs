using System.Collections.Generic;
using GameDevUtils.StackSystem;
using UnityEngine;

public class FightLeader : MonoBehaviour
{

	public StackManager playerStack;
	List<EnemyGroup>    fightingEnemiesGroups;
	
	public delegate void          OnAttackDelegate(Transform target);
	public event OnAttackDelegate OnGroupAttackInvoke;


	public delegate void              OnAttackStopDelegate();
	public event OnAttackStopDelegate OnGroupStopAttackInvoke;

	public void Start()
	{
		CaptureCharacter capture = GetComponent<CaptureCharacter>();
		capture.SetLeader(this);
	}

	public void AddFightingGroup(EnemyGroup enemyGroup)
	{
		if (!fightingEnemiesGroups.Contains(enemyGroup))
		{
			fightingEnemiesGroups.Add(enemyGroup);

			for (int i = 0; i < playerStack.CurrentCountOfStack("Followers"); i++)
			{
				if (fightingEnemiesGroups.Count >1)
				{
					var randomGroup = fightingEnemiesGroups[Random.Range(0, fightingEnemiesGroups.Count)];
					// OnGroupAttackInvoke.Invoke(randomGroup.allEnemies[0, randomGroup.allEnemies.Count]);
				}
			}
			
		}
			
	}


	public void OnGroupAllEnemiesDead(EnemyGroup enemyGroup)
	{
		if (fightingEnemiesGroups.Contains(enemyGroup))
			fightingEnemiesGroups.Remove(enemyGroup);
	}

}