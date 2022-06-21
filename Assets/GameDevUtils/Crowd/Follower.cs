using UnityEngine;

public class Follower : MonoBehaviour
{

	public event OnSetLeader onSetLeader;

	public delegate void OnSetLeader(Leader leader);

	private CaptureCharacter CaptureCharacter;
	private Leader           Leader;

	public Leader GetLeader => Leader;

	private void Awake()
	{
		CaptureCharacter = GetComponent<CaptureCharacter>();
	}

	public void ActiveCharacter(Leader target)
	{
		if (Leader == target) return;
		if (HasLeader()) Leader.MinusAgent();
		CaptureCharacter.SetLeader(target);
		target.AddAgent();
		Leader = target;
		onSetLeader?.Invoke(target);
	}

	public bool HasLeader() => Leader != null;

}