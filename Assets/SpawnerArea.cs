using UnityEngine;
using System.Collections;

public class SpawnerArea : MonoBehaviour
{

	[SerializeField] float      spawnRate = 0.1f;
	[SerializeField] GameObject spawnPrefab;
	[SerializeField] Transform  spawnPosition;

	BaseStackArea BaseStackArea;
	bool          IsSpawnPrefabs;

	void OnEnable()
	{
		BaseStackArea                    =  FindObjectOfType<BaseStackArea>();
		BaseStackArea.OnStackValueRemove += StartSpawning;
		BaseStackArea.OnStackValueFull   += StopSpawning;
		if (!BaseStackArea.IsAreaFUll) StartSpawning();
	}

	void OnDisable()
	{
		BaseStackArea.OnStackValueRemove -= StartSpawning;
		BaseStackArea.OnStackValueFull   -= StopSpawning;
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
			var follower = Instantiate(spawnPrefab, spawnPosition.position, spawnPosition.rotation);
			follower.GetComponent<FollowerMovement>().MoveToTarget(BaseStackArea.targetPoint.position);
			BaseStackArea.StackValueUp();
			yield return new WaitForSeconds(spawnRate);
		}
	}

}