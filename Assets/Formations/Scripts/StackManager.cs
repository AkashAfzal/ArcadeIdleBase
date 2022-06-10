using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;


[Serializable]
public class StackPrefab
{

	public string     uniqueID;
	public GameObject prefab;

}

public class StackManager : MonoBehaviour
{
	public static            StackManager  Instance { get; private set; }
	
	
	[SerializeField] private Transform        stackPoint;
	[SerializeField] private StackPrefab[]    stackPrefabs;
	
	public                   List<GameObject> spawnedObjects = new List<GameObject>();
	
	
	
	//Properties
	public                  List<Vector3>    SpawnPoints;

	private StackFormation formation;
	StackFormation Formation
	{
		get
		{
			if (formation == null) formation = GetComponent<StackFormation>();
			return formation;
		}
	}

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		SpawnPoints = Formation.EvaluatePoints().ToList();
	}

	public void AddStack(string prefabID)
	{
		if (SpawnPoints.Count == spawnedObjects.Count) return;
		var pos  = SpawnPoints[spawnedObjects.Count];
		Debug.Log($"{pos} and {spawnedObjects.Count} and {stackPoint.position + pos}");
		var unit = Instantiate(SpawnPrefab(prefabID), stackPoint);
		unit.transform.position = stackPoint.position + pos;
		unit.transform.rotation = stackPoint.rotation;
		spawnedObjects.Add(unit);
	}

	public void RemoveStack()
	{
		var last = spawnedObjects.Last();
		spawnedObjects.Remove(last);
		Destroy(last.gameObject);
	}

	private GameObject SpawnPrefab(string prefabID)
	{
		GameObject prefab = null;
		foreach (StackPrefab stackObject in stackPrefabs)
		{
			if (String.Equals(prefabID, stackObject.uniqueID))
				return stackObject.prefab;
		}

		Debug.LogError($"Prefab ID {prefabID} not Available");
		return prefab;
	}
}