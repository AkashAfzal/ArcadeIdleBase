using System;
using UnityEngine;


namespace GameDevUtils.CharacterController.InputSystem
{


	public abstract class InputBase
	{

		public event Action<Vector3> OnPointerDown, OnPointerUpdate, OnPointerUp;

		public virtual void Calculate()
		{
			if (Input.GetMouseButtonDown(0))
			{
				OnPointerDown?.Invoke(Input.mousePosition);
			}

			if (Input.GetMouseButton(0))
			{
				OnPointerUpdate?.Invoke(Input.mousePosition);
			}

			if (Input.GetMouseButtonUp(0))
			{
				OnPointerUp?.Invoke(Input.mousePosition);
			}
		}

	}


}