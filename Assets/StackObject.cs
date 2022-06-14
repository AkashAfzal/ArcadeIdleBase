using UnityEngine;

public class StackObject : MonoBehaviour, IStackObject
{

	[SerializeField] string     id;
	public           string     ID         => id;
	public           GameObject gameObject => gameObject;

	public void SetPositionRotation(Vector3 position, Quaternion rotation)
	{
		transform.localPosition = position;
		transform.rotation      = rotation;
	}


}