using UnityEngine;


namespace GameDevUtils
{


	public class Range
	{

		public float min;
		public float max;

		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public virtual bool Validate(float value)
		{
			return value >= min && value <= max;
			
		}

		public virtual float RandomValue()
		{
			return Random.Range(min, max);
		}

		public virtual float Clamp(float value)
		{
			return Mathf.Clamp(value, min, max);
		}

	}

	[System.Serializable]
	public class RangeBy : Range
	{

		public float from;

		public RangeBy(float from, float min, float max) : base(min, max)
		{
			this.@from = from;
		}

		public override float RandomValue()
		{
			return Random.Range(@from - min, @from + max);
		}

		public override bool Validate(float value)
		{
			return value >= @from - min && value <= @from + max;
		}

		public override float Clamp(float value)
		{
			return Mathf.Clamp(value, @from - min, @from + max);
		}

	}

	public class RectangleBoundary
	{

		float width;
		float height;
		float centerX, centerY;

		public RectangleBoundary(float centerX, float centerY, float width, float height)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			this.width   = width;
			this.height  = height;
		}

		public bool Validate(float x, float y)
		{
			return x >= centerX - width / 2 && x <= centerX + width / 2 && y >= centerY - height / 2 && y <= centerY + height / 2;
		}

		public void Clamp(ref float x, ref float y)
		{
			x = Mathf.Clamp(x, centerX - width  / 2, centerX + width  / 2);
			y = Mathf.Clamp(x, centerY - height / 2, centerY + height / 2);
		}

	}


	public class CubicBoundary
	{

		float width;
		float height;
		float length;
		float centerX, centerY, centerZ;

		public CubicBoundary(float centerX, float centerY, float centerZ, float width, float height, float length)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			this.centerZ = centerZ;
			this.width   = width;
			this.height  = height;
			this.length  = length;
		}

		public bool Validate(float x, float y, float z)
		{
			return x >= centerX - width / 2 && x <= centerX + width / 2 && y >= centerY - height / 2 && y <= centerY + height / 2 && z >= centerZ - length / 2 && z <= centerZ + length / 2;
		}

		public void Clamp(ref float x, ref float y, ref float z)
		{
			x = Mathf.Clamp(x, centerX - width  / 2, centerX + width  / 2);
			y = Mathf.Clamp(x, centerY - height / 2, centerY + height / 2);
			z = Mathf.Clamp(z, centerZ - length / 2, centerZ + length / 2);
		}

	}

	public class SphereBoundary
	{

		float radius;
		float centerX, centerY, centerZ;

		public SphereBoundary(float centerX, float centerY, float centerZ, float radius)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			this.centerZ = centerZ;
			this.radius  = radius;
		}

		public bool Validate(float x, float y, float z)
		{
			return x >= centerX - radius && x <= centerX + radius && y >= centerY - radius && y <= centerY + radius && z >= centerZ - radius && z <= centerZ + radius;
		}

		public void Clamp(ref float x, ref float y, ref float z)
		{
			x = Mathf.Clamp(x, centerX - radius, centerX + radius);
			y = Mathf.Clamp(x, centerY - radius, centerY + radius);
			z = Mathf.Clamp(z, centerZ - radius, centerZ + radius);
		}

	}

	public class CircleBoundary
	{

		float radius;
		float centerX, centerY;

		public CircleBoundary(float centerX, float centerY, float radius)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			this.radius  = radius;
		}

		public void Clamp(ref float x, ref float y)
		{
			x = Mathf.Clamp(x, centerX - radius, centerX + radius);
			y = Mathf.Clamp(x, centerY - radius, centerY + radius);
		}

	}


}