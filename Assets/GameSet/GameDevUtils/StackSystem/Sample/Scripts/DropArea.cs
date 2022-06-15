using System.Collections;
using GameDevUtils.CurrencySystem;
using GameDevUtils.StackSystem;
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
			if (!isAvailableInStack)
			{
				IsDropStack = false;
				StopCoroutine(nameof(DropStack));
				break;
			}

			CurrencyManager.Instance.PlusCurrencyValueWithAnimation("Coins",  1, transform.position);
			stackManager.AddStack(requiredID);
			yield return new WaitForSeconds(0.1f);
		}
	}

}