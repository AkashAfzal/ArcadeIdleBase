using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.CameraController.Scripts
{


	[CreateAssetMenu(fileName = "LookAtCamera", menuName = "GameDevUtils/Camera/LookAtCamera")]
	public class LookAtCamera : CameraState
	{

		[SerializeField] protected     Vector3 lookOffset;
		[SerializeField, Range(1, 20)] float   lookSpeed = 5;

		protected override void UpdateCamera(Transform camera, Transform pivot, Transform target, float deltaTime)
		{
			camera.rotation = Quaternion.Slerp(camera.rotation, Quaternion.Euler(lookOffset), lookSpeed * deltaTime);
		}

	}


}