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
			gameObject.SetActive(false);
		}
	}

}