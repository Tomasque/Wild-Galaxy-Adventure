using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{
    [SerializeField] Vector3 angularVelocity;
    [SerializeField] Rigidbody rb;

    private void FixedUpdate()
    {
        Quaternion newRotation = transform.rotation * Quaternion.Euler(angularVelocity);
        rb.MoveRotation(newRotation);
    }
}
