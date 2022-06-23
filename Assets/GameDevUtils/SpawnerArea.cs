using UnityEngine;
using System.Collections;
using GameDevUtils.StackSystem;

public class SpawnerArea : MonoBehaviour
{
	[SerializeField] float      spawnRate = 0.1f;
	[SerializeField] GameObject spawnPrefab;
	[SerializeField] Transform  spawnPosition;

	public BaseStackArea baseStack;
	bool                IsSpawnPrefabs;
	Vector3             pos;
	Quaternion          rot;

	void OnEnable()
	{
		baseStack.StartSpawnEvent += StartSpawning;
		baseStack.StopSpawnEvent   += StopSpawning;
	}

	void OnDisable()
	{
		baseStack.StartSpawnEvent -= StartSpawning;
		baseStack.StopSpawnEvent  -= StopSpawning;
	}

	private void StartSpawning()
	{
		Debug.Log("dnjklashfdlkajsfhaslj");
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
			if (!baseStack.IsCapacityFullOfStack)
			{
				// Vector3 pos      = Vector3.one;
				var     follower = Instantiate(spawnPrefab, spawnPosition.position, spawnPosition.rotation);
				baseStack.AddStack(follower.GetComponent<IStackObject>(), ref pos, ref rot);
				follower.GetComponent<Follower>().MoveToTarget(pos, true);
			}
			else
			{
				yield break;
			}

			yield return new WaitForSeconds(spawnRate);
		}
	}

}