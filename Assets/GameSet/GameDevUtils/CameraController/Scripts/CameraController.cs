using System;
using System.Collections;
using GameAssets.GameSet.GameDevUtils.Managers;
using GameAssets.GameSet.GameDevUtils.StateMachine;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.CameraController.Scripts
{


	public class CameraController : StateMachine.StateMachine
	{

		protected enum UpdateType
		{

			Update,
			FixedUpdate,
			LateUpdate

		}

		//public static event Action<int> changeCamera;
		public static Action<CameraState, Transform> anyCamera;
		public static Action                         resetToDefaultCamera;
		public        CameraData                     details;
		public        CameraState[]                  cameraStates;

		[SerializeField] protected UpdateType updateType;
		Transform                             defualtTarget;
		public GameObject                     celebrationParticles;

		void Awake()
		{
			anyCamera            = AnyCamera;
			resetToDefaultCamera = DefaultCamera;
			//changeCamera = ChangeCamera;
			foreach (CameraState state in cameraStates)
			{
				cachedStates.Add(state.stateName, state);
			}

			foreach (CameraState cameraState in cameraStates)
			{
				cameraState.Init(this);
			}

			defualtTarget    = details.target;
			currentStateName = string.IsNullOrEmpty(currentStateName) ? cameraStates[0].stateName : currentStateName;
			LoadState(currentStateName, Entry);
		}

		void OnEnable()
		{
			GameManager.onCompleteEvent += ONLevelComplete;
		}

		void OnDisable()
		{
			GameManager.onCompleteEvent -= ONLevelComplete;
		}

		private void ONLevelComplete()
		{
			celebrationParticles.SetActive(true);
			
		}
		
		private void Update()
		{
			if (updateType == UpdateType.Update)
			{
				details.deltaTime = Time.deltaTime;
				UpdateCamera();
			}
		}

		public void CameraSwitch(bool staticCam, bool movingCam)
		{
			if (staticCam)
			{
				details.target = details.staticTarget;
				defualtTarget  = details.target;
			}
			else if (movingCam)
			{
				details.target = details.movingTarget;
				defualtTarget  = details.target;
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
			if (currentAnyState != null)
			{
				((CameraState) currentAnyState).cameraDetails = details;
			}
			else
				((CameraState) currentState).cameraDetails = details;

			StatesExecution();
		}

		public void AnyCamera(CameraState cameraState, Transform target)
		{
			if (target)
				details.target = target;
			AnyTransition(cameraState);
		}

		public void DefaultCamera()
		{
			details.target = defualtTarget;
			ExitAnyStates();
		}

		public override void LoadState(string id, Action<IState> onStateLoad)
		{
			if (cachedStates.ContainsKey(id))
			{
				onStateLoad?.Invoke(cachedStates[id]);
			}
		}

		void OnDestroy()
		{
			anyCamera            = null;
			resetToDefaultCamera = null;
		}

		public void ChangeCam(int id)
		{
			cameraStates[id].SetTransitions();
		}

	}


}