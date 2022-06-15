using System;
using UnityEngine;


namespace GameDevUtils.CameraController
{

	
	[Flags]
	public enum AxisMask
	{

		X = 1,
		Y = 2,
		Z = 4

	}

	[CreateAssetMenu(fileName = "FollowAxis", menuName = "GameDevUtils/Camera/FollowAxis")]
	public class FollowAxis : CameraState
	{

		[SerializeField] protected     Vector3  posOffset, lookOffset;
		[SerializeField, Range(1, 20)] float    followSpeed = 5;
		[SerializeField, Range(1, 20)] float    lookSpeed   = 5;
		public                         AxisMask axis;

		public override void Execute()
		{
			UpdateCamera(cameraDetails.camera, cameraDetails.pivot, cameraDetails.target, cameraDetails.deltaTime);
		}
		void UpdateCamera(Transform camera, Transform pivot, Transform target, float deltaTime)
		{
			var cameraPosition = camera.position;
			if ((axis & AxisMask.X) == AxisMask.X)
				cameraPosition.x = target.position.x;
			if ((axis & AxisMask.Y) == AxisMask.Y)
				cameraPosition.y = target.position.y;
			if ((axis & AxisMask.Z) == AxisMask.Z)
				cameraPosition.z = target.position.z;
			pivot.localPosition = Vector3.Lerp(pivot.localPosition, posOffset,      followSpeed             * deltaTime);
			camera.position     = Vector3.Lerp(camera.position,     cameraPosition, followSpeed             * deltaTime);
			camera.rotation     = Quaternion.Slerp(camera.rotation, Quaternion.Euler(lookOffset), lookSpeed * deltaTime);
		}

	}


}