using System.Collections;
using UnityEngine;


public static class QuaternionExtensionMethods
{

	//Quaternion
	public static Quaternion SetX(this Quaternion rot, float x)
	{
		return new Quaternion(x, rot.y, rot.z, rot.w);
	}
	
	public static Quaternion SetY(this Quaternion rot, float y)
	{
		return new Quaternion(rot.x, y, rot.z, rot.w);
	}
	
	public static Quaternion SetZ(this Quaternion rot, float z)
	{
		return new Quaternion(rot.x, rot.y, z, rot.w);
	}
	
	public static Quaternion RandomRotation(this Quaternion rot, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
	{
		return new Quaternion(rot.x + Random.Range(minX, maxX), rot.y + Random.Range(minY, maxY), rot.z + Random.Range(minZ, maxZ), rot.w);
	}
	
	public static Quaternion ClampX(this Quaternion rot, float minRot, float maxRot)
	{
		return new Quaternion(Mathf.Clamp(rot.x, minRot, maxRot), rot.y, rot.z, rot.w);
	}

	public static Quaternion ClampY(this Quaternion rot, float minRot, float maxRot)
	{
		return new Quaternion(rot.x, rot.y + Mathf.Clamp(rot.y, minRot, maxRot), rot.z, rot.w);
	}

	public static Quaternion ClampZ(this Quaternion rot, float minRot, float maxRot)
	{
		return new Quaternion(rot.x, rot.y, Mathf.Clamp(rot.z, minRot, maxRot), rot.w);
	}

	public static float Remap(this float f, float fromMin, float fromMax, float toMin, float toMax)
	{
		float t = (f - fromMin) / (fromMax - fromMin);
		return Mathf.LerpUnclamped(toMin, toMax, t);
	}


	public static IEnumerator RotateAngleOverTime(this Transform transformToRotate, float angle, Vector3 axis, float inTime)
	{
		// calculate rotation speed
		float rotationSpeed = angle / inTime;

		while (true)
		{
			// save starting rotation position
			Quaternion startRotation = transformToRotate.rotation;

			float deltaAngle = 0;

			// rotate until reaching angle
			while (deltaAngle < angle)
			{
				deltaAngle += rotationSpeed * Time.deltaTime;
				deltaAngle =  Mathf.Min(deltaAngle, angle);

				transformToRotate.rotation = startRotation * Quaternion.AngleAxis(deltaAngle, axis);

				yield return null;
			}

			yield break;
		}
	}

}