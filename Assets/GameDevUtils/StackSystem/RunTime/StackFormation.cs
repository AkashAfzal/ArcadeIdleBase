using System;
using UnityEngine;
using System.Collections.Generic;


namespace GameDevUtils.StackSystem
{


	[Serializable]
	public class StackFormation
	{

		public enum Formation
		{

			ColumnsBase,
			HeightBase

		}

		public enum FormationType
		{

			Local,
			Global

		}

		public Formation     formation     = Formation.ColumnsBase;
		public FormationType formationType = FormationType.Local;

		[SerializeField, Min(1f)] private int   rows        = 1;
		[SerializeField, Min(1f)] private int   columns     = 1;
		[SerializeField, Min(1f)] private int   heightCount = 1;
		[SerializeField]          private float xSpacing    = 1;
		[SerializeField]          private float zSpacing    = 1;
		[SerializeField]          private float ySpacing    = 1;

		public int StackMaxSize => rows * heightCount * columns;

		public void SetFormationValues(int stackMaxQuantity)
		{
			if (formation == Formation.ColumnsBase)
			{
				while (StackMaxSize < stackMaxQuantity)
				{
					columns++;
				}
			}
			else if (formation == Formation.HeightBase)
			{
				while (StackMaxSize < stackMaxQuantity)
				{
					heightCount++;
				}
			}
		}


		public IEnumerable<Vector3> EvaluatePoints()
		{
			return formation == Formation.ColumnsBase ? Formation1() : Formation2();
		}


		private IEnumerable<Vector3> Formation1()
		{
			var middleOffset = new Vector3(rows * 0.5f, 0, columns - 1);
			for (var z = columns - 1; z >= 0; z--)
			{
				for (int i = 0; i < heightCount; i++)
				{
					for (var x = 0; x < rows; x++)
					{
						var pos = new Vector3(x,              0,             z);
						pos -= (middleOffset + new Vector3(0, -ySpacing * i, 0));
						pos =  new Vector3((pos.x + 0.5f) * xSpacing, pos.y * ySpacing, pos.z * zSpacing);
						yield return pos;
					}
				}
			}
		}

		private IEnumerable<Vector3> Formation2()
		{
			var middleOffset = new Vector3(rows * 0.5f, 0, columns - 1);
			for (int i = 0; i < heightCount; i++)
			{
				for (var z = columns - 1; z >= 0; z--)
				{
					for (var x = 0; x < rows; x++)
					{
						var pos = new Vector3(x,              0,             z);
						pos -= (middleOffset + new Vector3(0, -ySpacing * i, 0));
						pos =  new Vector3((pos.x + 0.5f) * xSpacing, pos.y * ySpacing, pos.z * zSpacing);
						yield return pos;
					}
				}
			}
		}

	}


}