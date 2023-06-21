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

    Vector3 forward = Vector3.zero, right = Vector3.zero;
    Vector3 forwardUp = Vector3.zero, rightUp = Vector3.zero;
    private bool isJumping = false;

    private void Awake()
    {
        instance = this;
    }


    private new void FixedUpdate()
    {
        base.FixedUpdate();

        //Get relative up (either from planet or player character)
        Vector3 up = down == Vector3.zero ? transform.up : -down.normalized;

        //Get relative forward and right
        Vector3 planetForward = Vector3.Cross(Vector3.Cross(up, cam.transform.up), up).normalized;
        Vector3 planetRight = Vector3.Cross(up, Vector3.Cross(cam.transform.right, up)).normalized;

        //Move player, rotating inputs by relative directions
        Vector3 translation = ((Quaternion.FromToRotation(Vector3.forward, planetForward) * forward) + (Quaternion.FromToRotation(Vector3.right, planetRight) * right)).normalized * walkSpeed;
        rb.MovePosition(rb.position + translation);


        //Check if camera needs to move
        if (activePlanets.Count == 1)
        {
            Transform target = cam.target;

            float angle = Vector3.Angle(-target.forward, up);

            if(angle > 50)
            {
                Vector3 rotationAxis = Vector3.Cross(-target.forward, up);
                target.parent.rotation = Quaternion.AngleAxis(angle - 50, rotationAxis) * target.parent.rotation;
            }
        }

        //Rotate player character towards movement direction + orient towards planet
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
        if (Input.GetKeyDown(KeyCode.W))
        {
            forward.z++;
            forwardUp = down == Vector3.zero ? transform.up : -down.normalized;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            forward.z--;
            forwardUp = down == Vector3.zero ? transform.up : -down.normalized;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            right.x++;
            rightUp = down == Vector3.zero ? transform.up : -down.normalized;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            right.x--;
            rightUp = down == Vector3.zero ? transform.up : -down.normalized;
        }



        if (Input.GetKeyUp(KeyCode.W))
        {
            forward.z--;
            forwardUp = down == Vector3.zero ? transform.up : -down.normalized;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            forward.z++;
            forwardUp = down == Vector3.zero ? transform.up : -down.normalized;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            right.x--;
            rightUp = down == Vector3.zero ? transform.up : -down.normalized;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            right.x++;
            rightUp = down == Vector3.zero ? transform.up : -down.normalized;
        }

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
