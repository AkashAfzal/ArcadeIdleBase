using GameDevUtils.CurrencySystem;
using GameDevUtils.StackSystem;
using UnityEngine;

public class Collectable : MonoBehaviour
{

	[SerializeField] string stackName;
	[SerializeField] string id;

	void OnTriggerEnter(Collider other)
	{
		var stackManager = other.GetComponent<StackManager>();
		if (other.CompareTag("Player") && stackManager && !stackManager.IsCapacityFullOfStack(stackName))
		{
			stackManager.AddStack(stackName, id);
			if (id == "2")
			{
				CurrencyManager.Instance.AddCurrencyValueWithAnimation("Star", 1, transform.position);
			}

			if (id == "1")
			{
				CurrencyManager.Instance.AddCurrencyValueWithAnimation("Coins", 1, transform.position);
			}

			gameObject.SetActive(false);
		}
	}

}