using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // The target the camera follows
    //[SerializeField] private float smoothSpeed = 5f;

    private void LateUpdate()
    {
        if (target == null) return;
        
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

    }

}
