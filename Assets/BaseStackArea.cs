using System;
using GameDevUtils.StackSystem;
using UnityEngine;

public class BaseStackArea : MonoBehaviour
{
	
	[SerializeField] StackManager stackManager;
	[SerializeField] string       stackName;
	[SerializeField] string       stackObjectId;

	public bool         IsCapacityFullOfStack => stackManager.IsCapacityFullOfStack(stackName);
	public event Action StartSpawnEvent;
	public event Action StopSpawnEvent;
	
	
	void OnEnable()
	{
		stackManager.OnStackValueRemoveEvent += OnStackRemove;
		stackManager.OnStackValueFullEvent   += OnStackFull;
		if (!IsCapacityFullOfStack) StartSpawnEvent?.Invoke();;
	}

	void OnDisable()
	{
		stackManager.OnStackValueRemoveEvent -= OnStackRemove;
		stackManager.OnStackValueFullEvent   -= OnStackFull;
		StopSpawnEvent?.Invoke();
	}

	private void OnStackRemove(string invokedStackName)
	{
		if (String.Equals(invokedStackName, this.stackName))
		{
			StartSpawnEvent?.Invoke();
		}
	}
	
	private void OnStackFull(string invokedStackName)
	{
		if (String.Equals(invokedStackName, this.stackName))
		{
			StopSpawnEvent?.Invoke();
		}
	}


	public void AddStack(IStackObject iStackObject, ref Vector3 pos, ref Quaternion rot)
	{
		stackManager.AddStack(stackName, iStackObject, ref pos, ref rot);
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			var playerStack = other.GetComponent<StackManager>();
			if (playerStack != null)
			{
				StopSpawnEvent?.Invoke();
				while (!stackManager.IsStackEmpty(stackName) || !playerStack.IsCapacityFullOfStack(stackName))
				{
					var iStackObject = stackManager.RemoveStack(stackName);
					playerStack.AddStack(stackName, iStackObject);
					iStackObject._GameObject.GetComponent<CaptureCharacter>().enabled = true;
					iStackObject._GameObject.GetComponent<Follower>().ActiveCharacter(other.GetComponent<Leader>(), true);
				}
			}
		}
	}
	
	
	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && other.GetComponent<StackManager>())
		{
			StartSpawnEvent?.Invoke();
		}
	}

}