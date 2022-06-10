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

	//Singleton 
	public static StackManager Instance { get; private set; }

	//Inspector Fields 
	[SerializeField] private Transform     stackPoint;
	[SerializeField] private StackPrefab[] allStackPrefabs;

	//Private Fields
	private          List<Vector3>    SpawnPoints    = new List<Vector3>();
	private readonly List<GameObject> SpawnedObjects = new List<GameObject>();


	//Properties

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
		if (SpawnPoints.Count == SpawnedObjects.Count) return;
		var pos  = SpawnPoints[SpawnedObjects.Count];
		var item = Instantiate(SpawnPrefab(prefabID), stackPoint);
		item.transform.localPosition = pos;
		item.transform.rotation      = stackPoint.rotation;
		SpawnedObjects.Add(item);
	}

	public void RemoveStack()
	{
		var last = SpawnedObjects.Last();
		SpawnedObjects.Remove(last);
		Destroy(last.gameObject);
	}

	private GameObject SpawnPrefab(string prefabID)
	{
		GameObject prefab = null;
		foreach (StackPrefab stackObject in allStackPrefabs)
		{
			if (String.Equals(prefabID, stackObject.uniqueID))
				return stackObject.prefab;
		}

		Debug.LogError($"Prefab ID {prefabID} not Available");
		return prefab;
	}

}