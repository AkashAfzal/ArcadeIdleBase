using GameDevUtils.Controller.InputSystem;
using UnityEngine;

public class JoyStickUI : MonoBehaviour
{

	private          Vector2       m_DummyVector2;
	[SerializeField] RectTransform m_Joystick;
	[SerializeField] RectTransform m_JoystickHandle;
	[SerializeField] CanvasGroup   canvasGroup;
	[SerializeField] Canvas        m_Canvas;


	void Update()
	{
		if (JoyStickInput.instance == null) return;
		canvasGroup.alpha = JoyStickInput.instance.JoystickDirection.magnitude > 0 ? 1 : 0;
		m_DummyVector2.Set(JoyStickInput.instance.Radius * 2f, JoyStickInput.instance.Radius * 2f);
		m_Joystick.sizeDelta              = m_DummyVector2;
		m_Joystick.anchoredPosition       = JoyStickInput.instance.JoystickPosition / m_Canvas.scaleFactor;
		m_JoystickHandle.anchoredPosition = JoyStickInput.instance.JoystickHandlePosition;
	}

}