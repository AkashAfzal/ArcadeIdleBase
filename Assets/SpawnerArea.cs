using UnityEngine;
using System.Collections;
using GameDevUtils.StackSystem;

public class SpawnerArea : MonoBehaviour
{

	[SerializeField] float      spawnRate = 0.1f;
	[SerializeField] GameObject spawnPrefab;
	[SerializeField] Transform  spawnPosition;

	public StackManager stackManager;
	bool                IsSpawnPrefabs;
	Vector3             pos;
	Quaternion          rot;

	void OnEnable()
	{
		stackManager.OnStackValueRemove += StartSpawning;
		stackManager.OnStackValueFull   += StopSpawning;
		if (!stackManager.IsStackQuantityFull) StartSpawning();
	}

	void OnDisable()
	{
		stackManager.OnStackValueRemove -= StartSpawning;
		stackManager.OnStackValueFull   -= StopSpawning;
		StopSpawning();
	}

	private void StartSpawning()
	{
		IsSpawnPrefabs = true;
		StartCoroutine(nameof(Spawn));
	}

	private void StopSpawning()
	{
		IsSpawnPrefabs = false;
		StopCoroutine(nameof(Spawn));
	}

	IEnumerator Spawn()
	{
		while (IsSpawnPrefabs)
		{
			if (!stackManager.IsStackQuantityFull)
			{
				var follower = Instantiate(spawnPrefab, spawnPosition.position, spawnPosition.rotation);
				stackManager.AddStack(follower.GetComponent<IStackObject>(), ref pos, ref rot);
				follower.GetComponent<FollowerMovement>().MoveToTarget(pos,true);
			}
			else
			{
				yield break;
			}
			
			yield return new WaitForSeconds(spawnRate);
		}
	}

}