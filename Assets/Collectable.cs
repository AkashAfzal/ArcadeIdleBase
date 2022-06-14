using UnityEngine;

public class Collectable : MonoBehaviour
{

	[SerializeField] string id;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			other.GetComponent<StackManager>().AddStack(id);
			gameObject.SetActive(false);
		}
	}

}