using UnityEngine;

public enum Axis
{

	X,
	Y,
	Z

}

public enum AxisT
{

	XY,
	XZ,
	YZ

}

public static class DistanceExtensionMethods
{

	public static float PositiveDistanceOfAnAxis(this Transform transform, Transform other, Axis axis)
	{
		return PositiveDistanceOfAnAxis(transform, other.position, axis);
	}

	public static float PositiveDistanceOfAnAxis(this Transform transform, Vector3 other, Axis axis)
	{
		float distance = 0;
		switch (axis)
		{
			case Axis.X:
				distance = Mathf.Abs(transform.position.x - other.x);
				break;

			case Axis.Y:
				distance = Mathf.Abs(transform.position.y - other.y);
				break;

			case Axis.Z:
				distance = Mathf.Abs(transform.position.z - other.z);
				break;

			default:
				break;
		}

		return distance;
	}

	public static float DistanceOfAnAxis(this Transform transform, Vector3 other, Axis axis)
	{
		return DistanceAnAxis(transform.position, other, axis);
	}

	public static float DistanceAnAxis(this Vector3 vector, Vector3 other, Axis axis)
	{
		float distance = 0;
		switch (axis)
		{
			case Axis.X:
				distance = vector.x - other.x;
				break;

			case Axis.Y:
				distance = vector.y - other.y;
				break;

			case Axis.Z:
				distance = vector.z - other.z;
				break;

			default:
				break;
		}

		return distance;
	}


	public static float DistanceOfTwoAxis(this Vector3 vector, Vector3 other, AxisT axisT)
	{
		float distance = 0;
		switch (axisT)
		{
			case AxisT.XY:
				other.z  = vector.z;
				distance = Vector3.Distance(vector, other);
				break;

			case AxisT.XZ:
				other.y  = vector.y;
				distance = Vector3.Distance(vector, other);
				break;

			case AxisT.YZ:
				other.x  = vector.x;
				distance = Vector3.Distance(vector, other);
				break;

			default:
				break;
		}

		return distance;
	}

}