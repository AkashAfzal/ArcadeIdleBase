using UnityEngine;
using System.Collections;
using GameDevUtils.StackSystem;

public class SpawnerArea : MonoBehaviour
{

	[SerializeField] string     stackName;
	[SerializeField] float      spawnRate = 0.1f;
	[SerializeField] GameObject spawnPrefab;
	[SerializeField] Transform  spawnPosition;

	public StackManager stackManager;
	bool                IsSpawnPrefabs;
	Vector3             pos;
	Quaternion          rot;

	void OnEnable()
	{
		stackManager.OnStackValueRemoveEvent += StartSpawning;
		stackManager.OnStackValueFullEvent   += StopSpawning;
		if (!stackManager.IsCapacityFullOfStack(stackName)) StartSpawning(stackName);
	}

	void OnDisable()
	{
		stackManager.OnStackValueRemoveEvent -= StartSpawning;
		stackManager.OnStackValueFullEvent   -= StopSpawning;
		StopSpawning(stackName);
	}

	private void StartSpawning(string stackName)
	{
		if (stackName != this.stackName) return;
		IsSpawnPrefabs = true;
		StartCoroutine(nameof(Spawn));
	}

	private void StopSpawning(string stackName)
	{
		if (stackName != this.stackName) return;
		IsSpawnPrefabs = false;
		StopCoroutine(nameof(Spawn));
	}

	IEnumerator Spawn()
	{
		while (IsSpawnPrefabs)
		{
			if (!stackManager.IsCapacityFullOfStack(stackName))
			{
				var follower = Instantiate(spawnPrefab, spawnPosition.position, spawnPosition.rotation);
				stackManager.AddStack(stackName,follower.GetComponent<IStackObject>(), ref pos, ref rot);
				follower.GetComponent<FollowerMovement>().MoveToTarget(pos, true);
			}
			else
			{
				yield break;
			}

			yield return new WaitForSeconds(spawnRate);
		}
	}

}