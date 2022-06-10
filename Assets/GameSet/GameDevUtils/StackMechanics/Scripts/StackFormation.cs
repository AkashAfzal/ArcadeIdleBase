using System.Collections.Generic;
using UnityEngine;

public class StackFormation : MonoBehaviour
{

	[SerializeField] private int   rows        = 5;
	[SerializeField] private int   columns     = 5;
	[SerializeField] private int   heightCount = 1;
	[SerializeField] private float xSpacing    = 1;
	[SerializeField] private float zSpacing    = 1;
	[SerializeField] private float ySpacing    = 1;

	public int StackMaxSize => rows * heightCount * columns;


	public IEnumerable<Vector3> EvaluatePoints()
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

}