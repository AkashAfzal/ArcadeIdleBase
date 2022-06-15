using GameDevUtils.StackSystem;
using UnityEngine;

public class Collectable : MonoBehaviour
{

	[SerializeField] string id;

	void OnTriggerEnter(Collider other)
	{
		var stackManager = other.GetComponent<StackManager>();
		if (other.CompareTag("Player") && stackManager && !stackManager.IsStackQuantityFull)
		{
			stackManager.AddStack(id);
			gameObject.SetActive(false);
		}
	}

}