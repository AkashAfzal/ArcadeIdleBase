using UnityEngine;

public class Leader : MonoBehaviour
{
    // private const string playerTag = "Player";

    // public event OnSetFollower onSetFollowers;
    // public event OnPlayerAddFollower onPlayerAddFollower;
    // public event OnDestroyLeader onDestroy;

    // public delegate void OnSetFollower(int followers);
    // public delegate void OnPlayerAddFollower();
    // public delegate void OnDestroyLeader();
    

    private int Followers = 1;
    
    public int AgentCount => Followers;

    public void Start()
    {
        CaptureCharacter capture = GetComponent<CaptureCharacter>();
       capture.SetLeader(this);
    }

    public void AddAgent()
    {
        Followers++;
        // onSetFollowers?.Invoke(Followers);
        // if(gameObject.CompareTag(playerTag))
        // {
        //     onPlayerAddFollower?.Invoke();
        // }
    }

    public void MinusAgent()
    {
        Followers--;
        // onSetFollowers?.Invoke(Followers);
    }
}
