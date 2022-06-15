using UnityEngine;


namespace GameDevUtils.CharacterController.InputSystem
{


	public class JoyStickInput : InputBase
	{

		private Vector2 m_JoystickDirection      = Vector2.zero;
		private Vector2 m_JoystickPosition       = Vector2.zero;
		private Vector2 m_JoystickHandlePosition = Vector2.zero;
		bool            isStatic;
		bool            isResetDirection;
		float           radius;

		public Vector2 JoystickDirection      => m_JoystickDirection;
		public Vector2 JoystickPosition       => m_JoystickPosition;
		public Vector2 JoystickHandlePosition => m_JoystickHandlePosition;
		public float   Radius                 => radius;

		public static JoyStickInput instance;

		public JoyStickInput(float radius, bool isStatic, bool isResetDirection)
		{
			instance              =  this;
			this.radius           =  radius;
			this.isStatic         =  isStatic;
			this.isResetDirection =  isResetDirection;
			OnPointerDown         += PointerDown;
			OnPointerUp           += PointerUp;
			OnPointerUpdate       += PointerUpdate;
		}

		void PointerDown(Vector3 mousePosition)
		{
			if (isResetDirection)
			{
				//We add small offset in order to make Joystick more stable on tapping
				if (m_JoystickDirection != Vector2.zero)
				{
					m_JoystickDirection      = m_JoystickDirection.normalized * 0.05f;
					m_JoystickPosition       = mousePosition - (Vector3) m_JoystickDirection;
					m_JoystickHandlePosition = mousePosition - (Vector3) m_JoystickDirection;
				}
				else
				{
					m_JoystickDirection      = Vector3.zero;
					m_JoystickPosition       = mousePosition;
					m_JoystickHandlePosition = mousePosition;
				}
			}
			else
			{
				m_JoystickPosition       = (Vector2) Input.mousePosition - m_JoystickDirection * radius;
				m_JoystickHandlePosition = Input.mousePosition;
			}
		}

		void PointerUp(Vector3 mousePosition)
		{
			m_JoystickDirection = Vector2.zero;
		}

		void PointerUpdate(Vector3 mousePosition)
		{
			m_JoystickDirection = ((Vector2) mousePosition - m_JoystickPosition) / radius;
			if (isStatic)
			{
				//Clamp
				if (m_JoystickDirection.magnitude > 1)
				{
					m_JoystickDirection.Normalize();
				}
			}
			else
			{
				//Clamp => move joystick position
				if (m_JoystickDirection.magnitude > 1)
				{
					var moveDistance = Vector2.Distance(mousePosition, m_JoystickPosition) - radius;
					m_JoystickDirection.Normalize();
					m_JoystickPosition += moveDistance * JoystickDirection;
				}
			}

			m_JoystickHandlePosition = m_JoystickDirection * radius;
		}

	}


}