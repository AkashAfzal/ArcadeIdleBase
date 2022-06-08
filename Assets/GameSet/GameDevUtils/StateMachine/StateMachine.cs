using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.StateMachine
{


	public abstract class StateMachine : MonoBehaviour, IStateMachine
	{

		public             string                     currentStateName;
		protected readonly Dictionary<string, IState> cachedStates = new Dictionary<string, IState>();
		protected          IState                     currentState, previousState;
		protected readonly Stack<IState>              anyStates = new Stack<IState>();
		public             bool                       isTransiting { get; private set; }

		public abstract void LoadState(string id, Action<IState> onStateLoad);

		protected IState currentAnyState;

		public IState GetState(string id)
		{
			
			return cachedStates[id];
		}

		public void Entry(IState state)
		{
			currentState = state;
			currentState?.Enter();
		}

		public void Transition(ITransition transition)
		{
			if (isTransiting) return;
			isTransiting = true;
			StartCoroutine(TransitionTo(transition));
		}

		private IEnumerator TransitionTo(ITransition transition)
		{
			yield return new WaitForSecondsRealtime(transition.exitTime);
			MoveToState(GetState(transition.toState));
		}

		void MoveToState(IState state)
		{
			currentState?.Exit();
			previousState = currentState;
			currentState  = state;
			currentState?.Enter();
			isTransiting = false;
		}

		public void AnyTransition(IState state)
		{
			currentAnyState = state;
			anyStates.Push(currentAnyState);
			currentAnyState.Enter();
		}

		public void ExitAnyStates()
		{
			if (anyStates.Count > 0)
			{
				currentAnyState.Exit();
				anyStates.Pop();
				if (anyStates.Count > 0)
				{
					AnyTransition(anyStates.Pop());
				}
				else
				{
					currentAnyState = null;
				}
			}
		}

		public void StatesExecution()
		{
			if (isTransiting) return;
			if (currentAnyState != null)
			{
				currentAnyState.Execute();
			}
			else
			{
				currentStateName = currentState.stateName;
				currentState?.Execute();
			}
		}

		public void Exit(IState state)
		{
			state?.Exit();
			if (currentState == state)
			{
				previousState = currentState;
				currentState  = null;
			}
		}

	}


}