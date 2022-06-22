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

		public event Action OnStackValueRemove;
		public event Action OnStackValueFull;

		//Public and Inspector Fields 
		[SerializeField] private bool           isTestingMode;
		[SerializeField] private bool           withoutFormation;
		[SerializeField] private StackFormation formation;
		[SerializeField] private UpgradesData   upgradesData;
		[SerializeField] private int            initialCapacity = 10;
		[SerializeField] private Transform      stackPoint;
		[SerializeField] private StackPrefab[]  allStackPrefabs;

		//Private Fields
		private readonly List<IStackObject> SpawnedStack = new List<IStackObject>();


		//Properties
		private int  UpgradeLevel        { get; set; } = 0;
		private int  MaxCapacity         => initialCapacity + (upgradesData != null && UpgradeLevel != 0 ? upgradesData.upgrades[UpgradeLevel - 1].upgradeCapacity : 0);
		public  int  CurrentUpgradePrice => upgradesData.upgrades[UpgradeLevel].upgradePrice;
		public  bool IsFullyUpgraded     => upgradesData == null || UpgradeLevel == upgradesData.upgrades.Length;
		public bool IsStackQuantityFull
		{
			get
			{
				if (MaxCapacity == SpawnedStack.Count)
				{
					OnStackValueFull?.Invoke();
				}

				return MaxCapacity == SpawnedStack.Count;
			}
		}

		private List<Vector3> SpawnPoints
		{
			get
			{
				if (withoutFormation) return null;
				if (MaxCapacity > formation.StackMaxSize)
				{
					formation.SetFormationValues(MaxCapacity);
				}

				return formation.EvaluatePoints().ToList();
			}
		}

		void Update()
		{
			if (isTestingMode)
			{
				RearrangeStack();
			}
		}

		/// <summary>
		/// Add specific stack object to stack and set position and rotation
		/// </summary>
		/// <param name="iStackObject"></param>
		public void AddStack(IStackObject iStackObject)
		{
			if (IsStackQuantityFull) return;
			if (SpawnPoints != null) iStackObject.SetPositionRotation(SpawnPoints[SpawnedStack.Count], stackPoint.rotation);
			SpawnedStack.Add(iStackObject);
		}

		/// <summary>
		/// Instantiate stack object of given id and add stack and set position
		/// </summary>
		/// <param name="prefabID"></param>
		public void AddStack(string prefabID)
		{
			if (IsStackQuantityFull) return;
			var item         = Instantiate(SpawnPrefab(prefabID), stackPoint);
			var iStackObject = item.GetComponent<IStackObject>();
			if (SpawnPoints != null) iStackObject.SetPositionRotation(SpawnPoints[SpawnedStack.Count], stackPoint.rotation);
			SpawnedStack.Add(iStackObject);
		}


		/// <summary>
		/// Add specific stack object to stack and out position and rotation for stack object
		/// </summary>
		/// <param name="iStackObject"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="localPosition"></param>
		public void AddStack(IStackObject iStackObject, ref Vector3 pos, ref Quaternion rot)
		{
			if (IsStackQuantityFull) return;
			if (SpawnPoints != null)
			{
				pos = formation.formationType == StackFormation.FormationType.Local ? SpawnPoints[SpawnedStack.Count] : stackPoint.transform.position + SpawnPoints[SpawnedStack.Count];
				rot = stackPoint.rotation;
			}

			SpawnedStack.Add(iStackObject);
		}

		/// <summary>
		/// Find stack prefab for given id
		/// </summary>
		/// <param name="prefabID"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Destroy Object form stack of stack object id if available
		/// </summary>
		/// <param name="stackObjectID"></param>
		/// <param name="isAvailableInStack"></param>
		public void RemoveStack(string stackObjectID, out bool isAvailableInStack)
		{
			IStackObject last = LastStackObject(stackObjectID);
			if (last == null)
			{
				isAvailableInStack = false;
				return;
			}

			isAvailableInStack = true;
			OnStackValueRemove?.Invoke();
			SpawnedStack.Remove(last);
			Destroy(last._GameObject);
			RearrangeStack();
		}


		/// <summary>
		/// Remove Passed stack object form stack 
		/// </summary>
		/// <param name="iStackObject"></param>
		/// <param name="isDestroy"></param>
		public void RemoveStack(IStackObject iStackObject, bool isDestroy = false)
		{
			OnStackValueRemove?.Invoke();
			SpawnedStack.Remove(iStackObject);
			if (isDestroy)
			{
				Destroy(iStackObject._GameObject);
			}

			RearrangeStack();
		}

		/// <summary>
		/// Find Last stack object of given id
		/// </summary>
		/// <param name="stackObjectID"></param>
		/// <returns></returns>
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


		/// <summary>
		/// Rearrange stack on the base formation
		/// </summary>
		void RearrangeStack()
		{
			if (SpawnPoints == null || SpawnedStack == null || SpawnedStack.Count == 0) return;
			for (var i = 0; i < SpawnedStack.Count; i++)
			{
				SpawnedStack[i].SetPositionRotation(formation.formationType == StackFormation.FormationType.Local ? SpawnPoints[i] : SpawnPoints[i] + stackPoint.transform.position, stackPoint.rotation);
			}
		}

		/// <summary>
		/// Upgrade Stack Capacity
		/// </summary>
		public void UpgradeStackCapacity()
		{
			UpgradeLevel++;
		}

	}


}