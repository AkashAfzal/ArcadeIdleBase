using UnityEngine;

public class StackFormationRenderer : MonoBehaviour
{

	public                   Transform stackPoint;
	[SerializeField] private Vector3   gizmoSize;
	[SerializeField] private Color     gizmoColor;

	private StackFormation formation;
	StackFormation Formation
	{
		get
		{
			if (formation == null) formation = GetComponent<StackFormation>();
			return formation;
		}
	}
	
	
	// positions returned by Formation.EvaluatePoints
	private void OnDrawGizmos()
	{
		if (Formation == null || Application.isPlaying) return;
		Gizmos.color = gizmoColor;
		foreach (Vector3 pos in Formation.EvaluatePoints())
		{
			Gizmos.DrawCube(stackPoint.position + pos + new Vector3(0, gizmoSize.y * 0.5f, 0), gizmoSize);
		}
	}

}