using UnityEngine;


namespace GameDevUtils.CameraController
{


	public class CameraController : MonoBehaviour
	{

		protected enum UpdateType
		{

			Update,
			FixedUpdate,
			LateUpdate

		}

		public CameraData    details;
		public CameraState[] cameraStates;
		public CameraState   CurrentCameraState;


		[SerializeField] protected UpdateType updateType;

		void Awake()
		{
			foreach (CameraState cameraState in cameraStates)
			{
				cameraState.InitState();
			}
			details.camera.position = Vector3.zero;
			ChangeCam(0);
		}

		private void Update()
		{
			if (updateType == UpdateType.Update)
			{
				details.deltaTime = Time.deltaTime;
				UpdateCamera();
			}
		}

		private void FixedUpdate()
		{
			if (updateType == UpdateType.FixedUpdate)
			{
				details.deltaTime = Time.fixedDeltaTime;
				UpdateCamera();
			}
		}

		private void LateUpdate()
		{
			if (updateType == UpdateType.LateUpdate)
			{
				details.deltaTime = Time.deltaTime;
				UpdateCamera();
			}
		}

		protected virtual void UpdateCamera()
		{
			if (details.target != null)
			{
				CurrentCameraState.cameraDetails = details;
				CurrentCameraState.Execute();
			}
		}


		public void ChangeCam(int id)
		{
			CurrentCameraState = cameraStates[id];
		}

	}


}