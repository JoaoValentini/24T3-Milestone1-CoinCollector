using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamera : MonoBehaviour
{
    public Transform target;  // target for camera to follow

    public float distance;  // distance from the camera to the target
    public float heightOffset; // camera height offset from the tagert


    // LateUpdate is called after all update functions have been called
    void LateUpdate()
    {
        FollowTarget();  
    }

    // Follow a target with a distance and height offset
    // then rotates the camera to look at the camera using the LookAt function
    void FollowTarget()
    {
        Vector3 targetPosition = target.position - target.forward * distance + Vector3.up * heightOffset;
        transform.position = targetPosition;
        transform.LookAt(target.position);
    }

}
