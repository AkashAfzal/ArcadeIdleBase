using System;
using System.Linq;
using GameAssets.GameSet.GameDevUtils.StateMachine;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.CameraController.Scripts
{


	[Serializable]
	public struct CameraData
	{

		public                   Transform staticTarget, movingTarget;
		public                   Transform target,       pivot, camera;
		[HideInInspector] public float     deltaTime;

	}

	public abstract class CameraState : ScriptableObject, IState
	{

		[SerializeField] protected  string             m_StateName;
		public                      string             stateName    => m_StateName;
		public                      IStateMachine      stateMachine { get; private set; }
		[SerializeField]  protected CameraTransition[] transitions;
		[HideInInspector] public    CameraData         cameraDetails;

		public virtual void Init(IStateMachine stateMachine)
		{
			this.stateMachine = stateMachine;
			InitState();
		}

		protected virtual void InitState()
		{
			
		}

		void IState.SetTransitions(params ITransition[] transitions)
		{
		}

		public void SetTransitions(params CameraTransition[] transitions)
		{
			this.transitions = transitions;
		}

		public ITransition[] GetTransitions()
		{
			return transitions;
		}

		public void SetTransitionCondition(string stateName, bool value)
		{
			foreach (ITransition transition in transitions)
			{
				if (transition.toState == stateName)
				{
					transition.condition = value;
				}
			}
		}


		public virtual void Execute()
		{
			var executableTransition = transitions.FirstOrDefault(x => x.condition);
			if (executableTransition != null)
			{
				stateMachine.Transition(executableTransition);
			}

			UpdateCamera(cameraDetails.camera, cameraDetails.pivot, cameraDetails.target, cameraDetails.deltaTime);
		}

		protected abstract void UpdateCamera(Transform camera, Transform pivot, Transform target, float deltaTime);

		public virtual void Enter()
		{
		}

		public virtual void Exit()
		{
			foreach (var transition in transitions)
			{
				transition.condition = false;
			}
		}

	}


}