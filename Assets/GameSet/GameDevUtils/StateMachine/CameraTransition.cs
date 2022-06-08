using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.StateMachine
{


	[CreateAssetMenu(fileName = "CameraTransition", menuName = "GameDevUtils/Camera/CameraTransition")]
	public class CameraTransition : ScriptableObject, ITransition
	{

		public string m_ToState;
		public bool   m_condition;
		public float  m_exitTime;
		public string toState => m_ToState;
		public bool condition
		{
			get => m_condition;
			set => m_condition = value;
		}
		public float exitTime => m_exitTime;

	}


}