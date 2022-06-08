using UnityEngine;

public static class PositionExtensionMethods
{
	
	/// <summary>
	/// Set X of given position to given X value 
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="x"></param>
	/// <returns></returns>
	public static Vector3 SetX(this Vector3 vector, float x)
	{
		return new Vector3(x, vector.y, vector.z);
	}

	/// <summary>
	/// Set Y of given position to given Y value 
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static Vector3 SetY(this Vector3 vector, float y)
	{
		return new Vector3(vector.x, y, vector.z);
	}

	/// <summary>
	/// Set Z of given position to given Z value 
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static Vector3 SetZ(this Vector3 vector, float z)
	{
		return new Vector3(vector.x, vector.y, z);
	}
	
	/// <summary>
	/// Set X and Y of given position to given X and Y value
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public static Vector3 VectorXY(this Vector3 vector, float x, float y)
	{
		return new Vector3(x, y, vector.z);
	}
	
	/// <summary>
	/// Set X and Z of given position to given X and Z value
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static Vector3 SetXZ(this Vector3 vector, float x, float z)
	{
		return new Vector3(x, vector.y, z);
	}

	/// <summary>
	/// Set Y and Z of given position to given Y and Z value
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static Vector3 VectorYZ(this Vector3 vector, float y, float z)
	{
		return new Vector3(vector.x, y, z);
	}

	
	/// <summary>
	/// Return a new "Position" of given position After Multiply these given values in it
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static Vector3 Multiply(this Vector3 vec, float x, float y, float z)
	{
		return new Vector3(vec.x * x, vec.y * y, vec.z * z);
	}

	
	/// <summary>
	/// Return a new "Position" of given position After Multiply these given value in it
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="other"></param>
	/// <returns></returns>
	public static Vector3 Multiply(this Vector3 vec, Vector3 other)
	{
		return Multiply(vec, other.x, other.y, other.z);
	}
	
	/// <summary>
	/// Return a new "Position" of given position After Plus these given value in it
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="z"></param>
	/// <returns></returns>
	public static Vector3 Plus(this Vector3 vec, float x, float y, float z)
	{
		return new Vector3(vec.x + x, vec.y + y, vec.z + z);
	}
	
	/// <summary>
	/// Return a new "Position" of given position After Plus these given value in it
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="other"></param>
	/// <returns></returns>
	public static Vector3 Plus(this Vector3 vec, Vector3 other)
	{
		return Plus(vec, other.x, other.y, other.z);
	}
	
	/// <summary>
	/// Return a new "Position" of given position After Plus Random X, Y, Z values Between their "Minimum" and "Maximum" Limits
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="minX"></param>
	/// <param name="maxX"></param>
	/// <param name="minY"></param>
	/// <param name="maxY"></param>
	/// <param name="minZ"></param>
	/// <param name="maxZ"></param>
	/// <returns></returns>
	public static Vector3 RandomPosition(this Vector3 vec, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
	{
		return new Vector3(vec.x + Random.Range(minX, maxX), vec.y + Random.Range(minY, maxY), vec.z + Random.Range(minZ, maxZ));
	}

	/// <summary>
	/// Return a Clamped Position of X Between Minimum value and Maximum Value
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns></returns>
	public static Vector3 ClampX(this Vector3 vec, float min, float max)
	{
		vec = new Vector3(Mathf.Clamp(vec.x, min, max), vec.y, vec.z);
		return vec;
	}
	
	/// <summary>
	/// Return a Clamped Position of Y Between Minimum value and Maximum Value
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns></returns>
	public static Vector3 ClampY(this Vector3 vec, float min, float max)
	{
		vec = new Vector3(vec.x, Mathf.Clamp(vec.y, min, max), vec.z);
		return vec;
	}
	
	/// <summary>
	/// Return a Clamped Position of Z Between Minimum value and Maximum Value
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns></returns>
	public static Vector3 ClampZ(this Vector3 vec, float min, float max)
	{
		vec = new Vector3(vec.x, vec.y, Mathf.Clamp(vec.z, min, max));
		return vec;
	}
	
	/// <summary>
	/// Return a Clamped Position Between Minimum value and Maximum Value
	/// </summary>
	/// <param name="vec"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <returns></returns>
	public static Vector3 Clamp(this Vector3 vec, Vector3 min, Vector3 max)
	{
		vec.x = Mathf.Clamp(vec.x, min.x, max.x);
		vec.y = Mathf.Clamp(vec.y, min.y, max.y);
		vec.z = Mathf.Clamp(vec.z, min.z, max.z);
		return vec;
	}

}