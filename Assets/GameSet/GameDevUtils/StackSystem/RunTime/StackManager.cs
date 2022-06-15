using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using GameDevUtils.StackSystem.UpgradeSystem;


namespace GameDevUtils.StackSystem
{


	[Serializable]
	public class StackPrefab
	{

		public string     uniqueID;
		public GameObject prefab;

	}

	public class StackManager : MonoBehaviour
	{

		//Public and Inspector Fields 
		[SerializeField] private bool          isTestingMode;
		[SerializeField] private UpgradesData  upgradesData;
		[SerializeField] private int           initialCapacity = 10;
		[SerializeField] private Transform     stackPoint;
		[SerializeField] private StackPrefab[] allStackPrefabs;

		//Private Fields
		private readonly List<IStackObject> SpawnedStack = new List<IStackObject>();


		//Properties
		private int  UpgradeLevel        { get; set; } = 0;
		private int  MaxCapacity         => initialCapacity + (upgradesData != null && UpgradeLevel != 0 ? upgradesData.upgrades[UpgradeLevel - 1].upgradeCapacity : 0);
		public  int  CurrentUpgradePrice => upgradesData.upgrades[UpgradeLevel].upgradePrice;
		public  bool IsStackQuantityFull => MaxCapacity == SpawnedStack.Count;
		public  bool IsFullyUpgraded     => upgradesData == null || UpgradeLevel == upgradesData.upgrades.Length;

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
				if (MaxCapacity > Formation.StackMaxSize)
				{
					Formation.SetFormationValues(MaxCapacity);
				}

				return Formation.EvaluatePoints().ToList();
			}
		}

		void Update()
		{
			if (isTestingMode)
			{
				RearrangeStack();
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

		public void RemoveStack(string stackObjectID, out bool isAvailableInStack)
		{
			IStackObject last = LastStackObject(stackObjectID);
			if (last == null)
			{
				isAvailableInStack = false;
				return;
			}

			isAvailableInStack = true;
			SpawnedStack.Remove(last);
			Destroy(last._GameObject);
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
			if (SpawnedStack == null || SpawnedStack.Count == 0) return;
			for (var i = 0; i < SpawnedStack.Count; i++)
			{
				SpawnedStack[i].SetPositionRotation(SpawnPoints[i], stackPoint.rotation);
			}
		}

		public void UpgradeStackCapacity()
		{
			UpgradeLevel++;
		}

	}


}