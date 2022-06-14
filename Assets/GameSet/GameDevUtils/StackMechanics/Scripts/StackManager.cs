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

	//Inspector Fields 
	public                   int           maxQuantity = 10;
	[SerializeField] private Transform     stackPoint;
	[SerializeField] private StackPrefab[] allStackPrefabs;

	//Private Fields
	private readonly List<IStackObject> SpawnedStack = new List<IStackObject>();


	//Properties

	public bool IsStackQuantityFull => maxQuantity == SpawnedStack.Count;

	private StackFormation formation;
	StackFormation Formation
	{
		get
		{
			if (formation == null) formation = GetComponent<StackFormation>();
			return formation;
		}
	}

	private List<Vector3> SpawnPoints
	{
		get
		{
			if (maxQuantity > Formation.StackMaxSize)
			{
				Formation.SetFormationValues(maxQuantity);
			}

			return Formation.EvaluatePoints().ToList();
		}
	}


	public void AddStack(string prefabID)
	{
		if (IsStackQuantityFull) return;
		var item         = Instantiate(SpawnPrefab(prefabID), stackPoint);
		var iStackObject = item.GetComponent<IStackObject>();
		iStackObject.SetPositionRotation(SpawnPoints[SpawnedStack.Count], stackPoint.rotation);
		SpawnedStack.Add(iStackObject);
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

	public void RemoveStack(string stackObjectID)
	{
		IStackObject last = LastStackObject(stackObjectID);
		SpawnedStack.Remove(last);
		Destroy(last.gameObject);
		RearrangeStack();
	}

	private IStackObject LastStackObject(string stackObjectID)
	{
		IStackObject iStackObject = null;
		for (int i = 0; i < SpawnedStack.Count; i++)
		{
			if (SpawnedStack[i].ID == stackObjectID)
			{
				iStackObject = SpawnedStack[i];
				return iStackObject;
			}
		}

		return iStackObject;
	}


	void RearrangeStack()
	{
		for (var i = 0; i < SpawnedStack.Count; i++)
		{
			SpawnedStack[i].SetPositionRotation(SpawnPoints[i], stackPoint.rotation);
		}
	}

}