using UnityEngine;

public class CaptureCharacter : MonoBehaviour
{
    // [SerializeField] private int agentPerLeader;
    // [SerializeField] private GameObject agentPrefab;
    [SerializeField] private float coolDown = 0.7f;
    [SerializeField] private float timeScape = 0;

    private Leader Leader;

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
    
    public void SetLeader(Leader leader)
    {
        Leader = leader;
    }

    private void InviteFollower(Collider other)
    {
        if (Leader == null || timeScape > 0)
        {
            return;
        }

        Follower captureMover = other.gameObject.GetComponent<Follower>();
        // Leader    enemyLeader  = other.gameObject.GetComponent<Leader>();

        if (captureMover != null)
        {
            if (captureMover.GetLeader == null || captureMover.GetLeader.AgentCount < Leader.AgentCount)
            {
                captureMover.GetComponent<Follower>().ActiveCharacter(Leader);
                timeScape = coolDown;
            }
        }
        // else if (enemyLeader != null)
        // {
        //     if (enemyLeader.AgentCount < 2 && Leader.AgentCount > 1)
        //     {
        //         Vector3 spawnPos = other.gameObject.transform.position;
        //         Destroy(other.gameObject);
        //         for (int i = 0; i < agentPerLeader; i++)
        //         {
        //             GameObject newAgent = Instantiate(agentPrefab, new Vector3 (spawnPos.x, 2.5f, spawnPos.z), Quaternion.identity);
        //             newAgent.GetComponent<Follower>().ActiveCharacter(Leader);
        //         }
        //         timescape = coolDown + Vector3.Distance(transform.position, Leader.gameObject.transform.position) / 100;
        //     }
        // }
    }
}
