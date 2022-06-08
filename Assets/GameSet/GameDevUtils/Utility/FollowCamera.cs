using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform Target;
    // [SerializeField] RangeBy   yOffSetRange;
    // [SerializeField] RangeBy   zOffsetRange;
    // [SerializeField] float     offsetScrollSpeed;
    // [SerializeField] float     offsetLerpSpeed;
    [SerializeField] float     SmoothTime = 0.3f;
    
    
    public Vector3 Offset; 
    // private Vector3 targetOffset;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    // float           yOffset;
    // float           zOffset;
    // public Vector3  OffsetNew;


    private void Start()
    {

        // Offset = transform.position - Target.position;
    }

    private void FixedUpdate()
    {
        // yOffset += verticalInput   * offsetScrollSpeed;
        // zOffset += horizontalInput * offsetScrollSpeed;
      if(!Target)
            return;
        // yOffset = yOffSetRange.Clamp(yOffset);
        // zOffset = zOffsetRange.Clamp(zOffset);
        //
        // targetOffset   = Vector3.Lerp(targetOffset, new Vector3(0, -yOffset, zOffset), offsetLerpSpeed * Time.deltaTime);

        targetPosition = Target.position + Offset;
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
    }
}