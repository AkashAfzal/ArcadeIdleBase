using System.Collections;
using System.Collections.Generic;
using GameDevUtils.StackSystem;
using UnityEngine;

public class Follower : MonoBehaviour, IStackObject
{
    
    //IStackObject Implementation
    public string     ID          => "Follower";
    public GameObject _GameObject => gameObject;

    public void SetPositionRotation(Vector3 position, Quaternion rotation)
    {
        movement.MoveToTarget(position);
    }
    
    private CaptureCharacter CaptureCharacter;
    private FightLeader      FightLeader;
    public  FollowerMovement movement;
    
    public FightLeader GetFightLeader => FightLeader;
    void Start()
    {
        CaptureCharacter = GetComponent<CaptureCharacter>();
    }
    
    public void ActiveCharacter(FightLeader target, bool forceFollow = false)
    {
        if (forceFollow) movement.stopAtTargetPos = false;
        if (movement.stopAtTargetPos || FightLeader == target) return;
        CaptureCharacter.SetLeader(target);
        FightLeader = target;
        movement.SetFollowTarget(target.transform);
    }

}
