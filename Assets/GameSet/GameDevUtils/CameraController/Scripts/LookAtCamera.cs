using UnityEngine;


namespace GameDevUtils.CameraController
{


	[CreateAssetMenu(fileName = "LookAtCamera", menuName = "GameDevUtils/Camera/LookAtCamera")]
	public class LookAtCamera : CameraState
	{

		[SerializeField] protected     Vector3 lookOffset;
		[SerializeField, Range(1, 20)] float   lookSpeed = 5;
		
		public override void Execute()
		{
			UpdateCamera(cameraDetails.camera, cameraDetails.pivot, cameraDetails.target, cameraDetails.deltaTime);
		}
		void UpdateCamera(Transform camera, Transform pivot, Transform target, float deltaTime)
		{
			camera.rotation = Quaternion.Slerp(camera.rotation, Quaternion.Euler(lookOffset), lookSpeed * deltaTime);
		}

	}


}