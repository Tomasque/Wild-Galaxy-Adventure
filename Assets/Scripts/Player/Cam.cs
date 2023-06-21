using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] Transform playerCam;
    

    Vector3 startPos;
    Quaternion startRot;
    public Transform target;
    float progress;

    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        target = playerCam;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(startPos, target.position, progress);
        transform.rotation = Quaternion.Lerp(startRot, target.rotation, progress);

        progress += Time.deltaTime;
    }

    public void setCam(PlanetGravity p = null)
    {
        
        startPos = transform.position;
        startRot = transform.rotation;

        target = p == null ? playerCam : p.cam;
        progress = 0f;
    }
}
