using UnityEngine;

public class CaptureCharacter : MonoBehaviour
{
	[SerializeField] private float coolDown  = 0.7f;
	[SerializeField] private float timeScape = 0;

	private FightLeader FightLeader;

	private void Start()
	{
		timeScape = coolDown;
	}

	private void Update()
	{
		if (timeScape > 0)
		{
			timeScape -= Time.deltaTime;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		InviteFollower(other);
	}

	public void SetLeader(FightLeader fightLeader)
	{
		FightLeader = fightLeader;
	}

	private void InviteFollower(Collider other)
	{
		if (FightLeader == null || timeScape > 0)
		{
			return;
		}

		if (other.GetComponent<CaptureCharacter>() && !other.GetComponent<CaptureCharacter>().enabled) return;
		
		Follower captureMover = other.gameObject.GetComponent<Follower>();
		if (captureMover != null)
		{
			if (captureMover.GetFightLeader == null || captureMover.GetFightLeader.playerStack.CurrentCountOfStack("Followers") < FightLeader.playerStack.CurrentCountOfStack("Followers"))
			{
				captureMover.GetComponent<Follower>().ActiveCharacter(FightLeader);
				timeScape = coolDown;
			}
		}
	}

}