using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    List<PlanetGravity> activePlanets = new List<PlanetGravity>();
    public static PlayerMovement instance;
    [SerializeField] float walkSpeed;
    float forward = 0;
    float rightward = 0;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        Vector3 down = Vector3.zero;
        Transform strongestPlanet = null;
        float strongestGravity = 0;

        foreach(PlanetGravity p in activePlanets)
        {
            Vector3 distanceVector = p.transform.position - transform.position;
            float t = (p.transform.lossyScale.x - distanceVector.magnitude) / p.transform.lossyScale.x;
            float gravity = Mathf.Lerp(p.outerGravity, p.innerGravity, t);

            down += gravity * distanceVector.normalized;

            if(Mathf.Sign(gravity)> strongestGravity)
            {
                strongestPlanet = p.transform;
            }
            
        }

        rb.AddForce(down);
        transform.rotation = Quaternion.FromToRotation(Vector3.down, down.normalized);

        Vector3 tangentialVelocity = (transform.forward * forward + transform.right * rightward).normalized * walkSpeed;
        rb.velocity += tangentialVelocity;

        if (strongestPlanet != null)
        {
            Vector3 centripetalForce = down.normalized * (rb.mass * tangentialVelocity.sqrMagnitude) / strongestPlanet.lossyScale.x;
            rb.AddForce(centripetalForce);
        }
    }

    private void Update()
    {
        forward = 0;
        rightward = 0;

        if (Input.GetKey(KeyCode.W)) forward++;
        if (Input.GetKey(KeyCode.S)) forward--;
        if (Input.GetKey(KeyCode.A)) rightward++;
        if (Input.GetKey(KeyCode.D)) rightward--;
    }

    public void addPlanet(PlanetGravity x)
    {
        print("b ");
        if (activePlanets.Contains(x))
        {
            Debug.LogError($"activePlanets already contains {x}");
        }
        activePlanets.Add(x);
    }

    public void removePlanet(PlanetGravity x)
    {
        if (!activePlanets.Contains(x))
        {
            Debug.LogError($"activePlanets doesn't contain {x}");
        }
        activePlanets.Remove(x);
    }


}
