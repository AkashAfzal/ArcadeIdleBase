using GameDevUtils.StackSystem;
using UnityEngine;

public class Follower : MonoBehaviour, IStackObject
{

	public delegate void OnSetLeader(Leader leader);

	public event OnSetLeader onSetLeader;

	public string     ID          => "Follower";
	public GameObject _GameObject => gameObject;

	[SerializeField] FollowerMovement movement;

	public void SetPositionRotation(Vector3 position, Quaternion rotation)
	{
		movement.MoveToTarget(position);
	}


	private CaptureCharacter CaptureCharacter;
	private Leader           Leader;

	public Leader GetLeader => Leader;

	private void Awake()
	{
		CaptureCharacter = GetComponent<CaptureCharacter>();
	}

	public void ActiveCharacter(Leader target)
	{
		if (movement.StopAtTargetPos) return;
		if (Leader == target) return;
		if (HasLeader()) Leader.MinusAgent();
		CaptureCharacter.SetLeader(target);
		target.AddAgent();
		Leader = target;
		onSetLeader?.Invoke(target);
	}

	public bool HasLeader() => Leader != null;

}