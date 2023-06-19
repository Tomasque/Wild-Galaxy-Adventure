using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GravityObject
{
    public static PlayerMovement instance;

    [SerializeField] float jumpForce;
    [SerializeField] float walkSpeed;
    [SerializeField] float mouseSpeed;

    [SerializeField] Cam cam;

    private bool isJumping = false;

    private void Awake()
    {
        instance = this;
    }


    private new void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 up = down == Vector3.zero ? transform.up : -down.normalized;


        Vector3 forward = Vector3.forward * Input.GetAxisRaw("Vertical");
        Vector3 right = Vector3.right * Input.GetAxisRaw("Horizontal");

        Vector3 planetForward = Vector3.Cross(cam.transform.right, up).normalized;
        Vector3 planetRight = Vector3.Cross(up, planetForward).normalized;

        Debug.DrawLine(transform.position, transform.position + planetForward * 2f, Color.green);
        Debug.DrawLine(transform.position, transform.position + planetRight * 2f, Color.green);

        Debug.DrawLine(transform.position, transform.position + planetForward * 2f, Color.black);
        Vector3 translation = ((Quaternion.FromToRotation(Vector3.forward, planetForward) * forward) + (Quaternion.FromToRotation(Vector3.right, planetRight) * right)).normalized * walkSpeed;


        rb.MovePosition(rb.position + translation);

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, up) * Quaternion.FromToRotation(transform.forward, translation) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, down.magnitude * Time.deltaTime);

        if (isJumping)
        {
            rb.AddForce(up * jumpForce);
            isJumping = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) isJumping = true;
    }


    new void addPlanet(PlanetGravity p)
    {
        base.addPlanet(p);
        if (activePlanets.Count == 1)
        {
            cam.setCam(p);
        }
    }
    new void removePlanet(PlanetGravity p)
    {
        base.removePlanet(p);
        if (activePlanets.Count == 1)
        {
            cam.setCam(activePlanets[0]);
        }
        else if(activePlanets.Count == 0)
        {
            cam.setCam();
        }
    }

}
