using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.CameraController.Scripts
{


	[CreateAssetMenu(fileName = "SimpleFollow", menuName = "GameDevUtils/Camera/SimpleFollow")]
	public class SimpleFollow : CameraState
	{

		Vector3                                 backupPosOffset, backupLookOffset;
		[SerializeField] public         Vector3 posOffset,       lookOffset;
		[SerializeField, Range(1, 120)] float   followSpeed = 5;
		[SerializeField, Range(1, 120)] float   lookSpeed   = 5;
		Vector3                                 dir;
		[SerializeField] protected bool         lockCamera;
		[SerializeField] protected bool         followTargetDirection;

		protected override void InitState()
		{
			backupPosOffset  = posOffset;
			backupLookOffset = lookOffset;
		}

		protected override void UpdateCamera(Transform camera, Transform pivot, Transform target, float deltaTime)
		{
			pivot.localPosition = Vector3.Lerp(pivot.localPosition, posOffset, followSpeed * deltaTime);
			dir                 = target.position - camera.position;
			dir.Normalize();
			Vector3 newPos = target.position - (followTargetDirection ? dir : Vector3.zero);
			if (lockCamera)
				newPos = newPos.SetX(0);
			camera.position = Vector3.Lerp(camera.position, newPos, followSpeed * deltaTime);
			var rotation = target.rotation * Quaternion.Euler(lookOffset);
			camera.rotation = Quaternion.Slerp(camera.rotation, rotation, Time.deltaTime * lookSpeed);
		}

		public void ResetCameraValue()
		{
			posOffset  = backupPosOffset;
			lookOffset = backupLookOffset;
		}

	}


}