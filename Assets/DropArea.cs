using System;
using System.Collections;
using UnityEngine;

public class DropArea : MonoBehaviour
{

	[SerializeField] string       requiredID;
	[SerializeField] StackManager stackManager;

	bool         IsDropStack;
	StackManager PlayerStack;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player") && other.gameObject.GetComponent<StackManager>())
		{
			if (stackManager.IsStackQuantityFull) return;
			PlayerStack = other.gameObject.GetComponent<StackManager>();
			IsDropStack = true;
			StartCoroutine(nameof(DropStack));
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player") && other.gameObject.GetComponent<StackManager>())
		{
			PlayerStack = null;
			IsDropStack = false;
			StopCoroutine(nameof(DropStack));
		}
	}


	IEnumerator DropStack()
	{
		while (IsDropStack && !stackManager.IsStackQuantityFull)
		{
			PlayerStack.RemoveStack(requiredID, out bool isAvailableInStack);
			if(!isAvailableInStack)
			{
				IsDropStack = false;
				StopCoroutine(nameof(DropStack));
				break;
			}
			CoinsManager.Instance.AddCoins(transform.position,1);
			stackManager.AddStack(requiredID);
			yield return new WaitForSeconds(0.1f);
		}
	}

}