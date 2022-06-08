using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.CameraController.Scripts
{


	public class CameraSwitch : MonoBehaviour
	{

		[SerializeField] bool        autoActive = false;
		[SerializeField] Transform   target;
		[SerializeField] CameraState cameraState;

		public void Active()
		{
			CameraController.anyCamera?.Invoke(cameraState, target);
		}

		void OnEnable()
		{
			if (autoActive)
				CameraController.anyCamera?.Invoke(cameraState, target);
		}

		void OnDisable()
		{
			CameraController.resetToDefaultCamera?.Invoke();
		}

	}


}