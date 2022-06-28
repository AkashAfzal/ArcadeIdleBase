using System.Collections.Generic;
using GameDevUtils.StackSystem;
using UnityEngine;

public class FightLeader : MonoBehaviour
{

	public   StackManager     playerStack;
	readonly List<EnemyGroup> FightingEnemiesGroups = new List<EnemyGroup>();

	public void Start()
	{
		CaptureCharacter capture = GetComponent<CaptureCharacter>();
		capture.SetLeader(this);
	}

	public void AddFightingGroup(EnemyGroup enemyGroup)
	{
		if (!FightingEnemiesGroups.Contains(enemyGroup))
		{
			FightingEnemiesGroups.Add(enemyGroup);
			var stackElements = playerStack.AllElementsOfStack("Followers");
			for (int i = 0; i < playerStack.CurrentCountOfStack("Followers"); i++)
			{
				var randomGroup = FightingEnemiesGroups[Random.Range(0, FightingEnemiesGroups.Count)];
				stackElements[i]._GameObject.GetComponent<Follower>().StartFighting(randomGroup.allEnemies[Random.Range(0, randomGroup.allEnemies.Count)].transform);
			}
		}
	}


	public void OnGroupAllEnemiesDead(EnemyGroup enemyGroup)
	{
		if (FightingEnemiesGroups.Contains(enemyGroup))
		{
			FightingEnemiesGroups.Remove(enemyGroup);
			
			if (FightingEnemiesGroups.Count == 0)
			{
				var stackElements = playerStack.AllElementsOfStack("Followers");
				for (int i = 0; i < playerStack.CurrentCountOfStack("Followers"); i++)
				{
					stackElements[i]._GameObject.GetComponent<Follower>().StopFighting();
				}
			}
		}
	}


	public List<Enemy> RandomGroupAllEnemies()
	{
		return FightingEnemiesGroups[Random.Range(0, FightingEnemiesGroups.Count)].allEnemies;
	}

}