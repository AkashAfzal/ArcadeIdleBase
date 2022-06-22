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

	[Serializable]
	public class Stack
	{

		//Fields 


		#region Fields

		//Public and Inspector Fields 
		public                   string         uniqueStackName;
		[SerializeField] private bool           withoutFormation;
		[SerializeField] private StackFormation formation;
		[SerializeField] private UpgradesData   upgradesData;
		[SerializeField] private int            initialCapacity = 10;
		[SerializeField] private Transform      stackPoint;
		[SerializeField] private StackPrefab[]  allStackPrefabs;

		//Private Fields
		private readonly List<IStackObject> SpawnedStack = new List<IStackObject>();

		#endregion


		//Properties


		#region PrivateProperties

		private int UpgradeLevel { get; set; } = 0;
		private int MaxCapacity  => initialCapacity + (upgradesData != null && UpgradeLevel != 0 ? upgradesData.upgrades[UpgradeLevel - 1].upgradeCapacity : 0);

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

		#endregion


		#region PublicProperties

		public bool IsStackCapacityFull => MaxCapacity == SpawnedStack.Count;
		public int  CurrentUpgradePrice => upgradesData.upgrades[UpgradeLevel].upgradePrice;
		public bool IsFullyUpgraded     => upgradesData == null || UpgradeLevel == upgradesData.upgrades.Length;

		#endregion


		//Methods


		#region PublicMethods

		public void AddStack(IStackObject iStackObject)
		{
			if (IsStackCapacityFull) return;
			iStackObject._GameObject.transform.parent = stackPoint;
			if (SpawnPoints != null) iStackObject.SetPositionRotation(SpawnPoints[SpawnedStack.Count], stackPoint.rotation);
			SpawnedStack.Add(iStackObject);
		}

		public void AddStack(string prefabID)
		{
			if (IsStackCapacityFull) return;
			var item         = UnityEngine.Object.Instantiate(SpawnPrefab(prefabID), stackPoint);
			var iStackObject = item.GetComponent<IStackObject>();
			if (SpawnPoints != null) iStackObject.SetPositionRotation(SpawnPoints[SpawnedStack.Count], stackPoint.rotation);
			SpawnedStack.Add(iStackObject);
		}


		public void AddStack(IStackObject iStackObject, ref Vector3 pos, ref Quaternion rot)
		{
			if (IsStackCapacityFull) return;
			if (SpawnPoints != null)
			{
				pos = formation.formationType == StackFormation.FormationType.Local ? SpawnPoints[SpawnedStack.Count] : stackPoint.transform.position + SpawnPoints[SpawnedStack.Count];
				rot = stackPoint.rotation;
			}

			SpawnedStack.Add(iStackObject);
		}

		public IStackObject RemoveStack(string stackObjectID)
		{
			IStackObject last = LastStackObject(stackObjectID);
			if (last != null)
			{
				SpawnedStack.Remove(last);
				RearrangeStack();
			}
			return last;
		}


		public void RemoveStack(IStackObject iStackObject)
		{
			SpawnedStack.Remove(iStackObject);
			RearrangeStack();
		}

		/// <summary>
		/// Rearrange stack on the base formation
		/// </summary>
		public void RearrangeStack()
		{
			if (SpawnPoints == null || SpawnedStack == null || SpawnedStack.Count == 0) return;
			for (var i = 0; i < SpawnedStack.Count; i++)
			{
				SpawnedStack[i].SetPositionRotation(formation.formationType == StackFormation.FormationType.Local ? SpawnPoints[i] : SpawnPoints[i] + stackPoint.transform.position, stackPoint.rotation);
			}
		}

		public void UpgradeStackCapacity()
		{
			UpgradeLevel++;
		}

		#endregion


		#region PrivateMethods

		/// <summary>
		/// Find stack prefab for given id
		/// </summary>
		/// <param name="prefabID"></param>
		/// <returns></returns>
		private GameObject SpawnPrefab(string prefabID)
		{
			foreach (StackPrefab stackObject in allStackPrefabs)
			{
				if (String.Equals(prefabID, stackObject.uniqueID))
					return stackObject.prefab;
			}

			Debug.LogError($"Prefab ID {prefabID} not Available");
			return null;
		}


		/// <summary>
		/// Find Last stack object of given id
		/// </summary>
		/// <param name="stackObjectID"></param>
		/// <returns></returns>
		private IStackObject LastStackObject(string stackObjectID)
		{
			for (int i = 0; i < SpawnedStack.Count; i++)
			{
				if (SpawnedStack[i].ID == stackObjectID)
				{
					return SpawnedStack[i];
				}
			}

			return null;
		}

		#endregion

	}

	public class StackManager : MonoBehaviour
	{

		public delegate void StackValueRemoveDelegate(string stackName);

		public event StackValueRemoveDelegate OnStackValueRemoveEvent;

		public delegate void StackValueFullDelegate(string stackName);

		public event StackValueFullDelegate OnStackValueFullEvent;


		[SerializeField] private bool    isTestingMode;
		[SerializeField]         Stack[] allStacks;


		void Update()
		{
			if (isTestingMode)
			{
				foreach (var stack in allStacks)
				{
					stack.RearrangeStack();
				}
			}
		}

		/// <summary>
		/// is that stack capacity full
		/// </summary>
		/// <param name="stackName"></param>
		/// <returns></returns>
		public bool IsCapacityFullOfStack(string stackName)
		{
			var stack = StackOfName(stackName);
			return stack is {IsStackCapacityFull: true};
		}

		/// <summary>
		/// Current upgrade of specific stack 
		/// </summary>
		/// <param name="stackName"></param>
		/// <returns></returns>
		public int CurrentUpgradePriceOfStack(string stackName)
		{
			var stack = StackOfName(stackName);
			return stack != null ? StackOfName(stackName).CurrentUpgradePrice : 0;
		}

		/// <summary>
		/// Is that stack is fully upgraded
		/// </summary>
		/// <param name="stackName"></param>
		/// <returns></returns>
		public bool IsStackFullyUpgraded(string stackName)
		{
			var stack = StackOfName(stackName);
			return stack is {IsFullyUpgraded: true};
		}


		/// <summary>
		/// Upgrade Capacity of specific stack
		/// </summary>
		/// <param name="stackName">Stack name which capacity to increase</param>
		public void UpgradeStackCapacity(string stackName)
		{
			StackOfName(stackName)?.UpgradeStackCapacity();
		}


		/// <summary>
		/// Add specific stack object to specific stack and set position and rotation
		/// </summary>
		/// <param name="stackName"></param>
		/// <param name="iStackObject"></param>
		public void AddStack(string stackName, IStackObject iStackObject)
		{
			var stack = StackOfName(stackName);
			if(stack == null) return;
			stack.AddStack(iStackObject);
			if (stack.IsStackCapacityFull)
				OnStackValueFullEvent?.Invoke(stackName);
		}

		/// <summary>
		/// Instantiate and add stack object of specific stack of given id and set position
		/// </summary>
		/// <param name="stackName"></param>
		/// <param name="prefabID"></param>
		public void AddStack(string stackName, string prefabID)
		{
			var stack = StackOfName(stackName);
			if(stack == null) return;
			stack.AddStack(prefabID);
			if (stack.IsStackCapacityFull)
				OnStackValueFullEvent?.Invoke(stackName);
		}


		/// <summary>
		/// Add specific stack object to specific stack and out position and rotation for stack object
		/// </summary>
		/// <param name="stackName"></param>
		/// <param name="iStackObject"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		public void AddStack(string stackName, IStackObject iStackObject, ref Vector3 pos, ref Quaternion rot)
		{
			var stack = StackOfName(stackName);
			if(stack == null) return;
			stack.AddStack(iStackObject, ref pos, ref rot);
			if (stack.IsStackCapacityFull)
				OnStackValueFullEvent?.Invoke(stackName);
		}


		/// <summary>
		/// Remove stack object form specific stack for stack object id if available
		/// </summary>
		/// <param name="stackName"></param>
		/// <param name="stackObjectID"></param>
		public IStackObject RemoveStack(string stackName, string stackObjectID)
		{
			var stack = StackOfName(stackName);
			if(stack == null) return null;
			var iStackObject = stack.RemoveStack(stackObjectID);
			if (iStackObject != null)
				OnStackValueRemoveEvent?.Invoke(stackName);
			return iStackObject;
		}


		/// <summary>
		/// Remove Passed stack object form specific stack
		/// </summary>
		/// <param name="stackName"></param>
		/// <param name="iStackObject"></param>
		public void RemoveStack(string stackName, IStackObject iStackObject)
		{
			var stack = StackOfName(stackName);
			if(stack == null) return;
			stack.RemoveStack(iStackObject);
			OnStackValueRemoveEvent?.Invoke(stackName);
		}


		/// <summary>
		/// Return stack object of given name if available
		/// </summary>
		/// <param name="stackName"></param>
		/// <returns></returns>
		private Stack StackOfName(string stackName)
		{
			foreach (Stack stackItem in allStacks)
			{
				if (String.Equals(stackName, stackItem.uniqueStackName))
				{
					return stackItem;
				}
			}

			Debug.LogError($"Stack With name {stackName} is not Available");
			return null;
		}

	}


}