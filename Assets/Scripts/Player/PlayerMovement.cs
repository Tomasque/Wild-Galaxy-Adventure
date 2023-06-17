using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] Rigidbody rb;
    [SerializeField] float walkSpeed;
    [SerializeField] Transform cam;

    List<PlanetGravity> activePlanets = new List<PlanetGravity>();

    Vector3 prevVelocity = Vector3.zero;
    float forward = 0f;
    float rightward = 0f;
    float xRotation = 0f;

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        Vector3 down = Vector3.zero;

        foreach(PlanetGravity p in activePlanets)
        {
            Vector3 distanceVector = p.transform.position - transform.position;
            float t = (p.transform.lossyScale.x - distanceVector.magnitude) / p.transform.lossyScale.x;
            float gravity = Mathf.Lerp(p.outerGravity, p.innerGravity, t);

            down += gravity * distanceVector.normalized;
        }

        rb.AddForce(down);

        if(down != Vector3.zero)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.down, down.normalized);
        }


        xRotation += Input.GetAxisRaw("Mouse X");

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        forward = 0;
        rightward = 0;

        if (Input.GetKey(KeyCode.W)) forward++;
        if (Input.GetKey(KeyCode.S)) forward--;
        if (Input.GetKey(KeyCode.A)) rightward++;
        if (Input.GetKey(KeyCode.D)) rightward--;
        if (Input.GetKey(KeyCode.D)) rightward--;

        Vector3 tangentialDisplacement = (transform.forward * forward + transform.right * rightward).normalized * walkSpeed * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + tangentialDisplacement);
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
