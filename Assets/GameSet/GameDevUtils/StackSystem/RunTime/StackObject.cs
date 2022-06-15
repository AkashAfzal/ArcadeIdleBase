using UnityEngine;

namespace GameDevUtils.StackSystem
{


	public class StackObject : MonoBehaviour, IStackObject
	{

		[SerializeField] string     id;
		public           string     ID          => id;
		public           GameObject _GameObject => gameObject;

		public void SetPositionRotation(Vector3 position, Quaternion rotation)
		{
			transform.localPosition = position;
			transform.rotation      = rotation;
		}


	}


}