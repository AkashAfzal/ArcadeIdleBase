using System;
using UnityEngine;


namespace GameDevUtils.CameraController
{


	[Serializable]
	public struct CameraData
	{
		public                   Transform target,       pivot, camera;
		[HideInInspector] public float     deltaTime;

	}

	public abstract class CameraState : ScriptableObject
	{

		[HideInInspector] public CameraData cameraDetails;

		public virtual void InitState() { }

		public abstract void Execute();


	}


}