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

        Vector3 up = down == Vector3.zero ? transform.up : -down.normalized;

        //forward and right up
        Vector3 planetForward = Vector3.Cross(Vector3.Cross(up, cam.transform.up), up).normalized;
        Vector3 planetRight = Vector3.Cross(up, Vector3.Cross(cam.transform.right, up)).normalized;

        //Vector3 newForward = planetForward * 0.1f, newRight = planetRight * 0.1f;
        //Vector3 prevPointF = transform.position, prevPointR = transform.position;

        //for (int i = 0; activePlanets.Count == 1 && i < 380; i++)
        //{
        //    Debug.DrawLine(prevPointF, prevPointF + newForward, Color.black);
        //    prevPointF += newForward;
        //    Vector3 newUp = prevPointF - activePlanets[0].transform.position;
        //    newForward = Vector3.Cross(Vector3.Cross(up, cam.transform.up), newUp).normalized * 0.1f;


        //    Debug.DrawLine(prevPointR, prevPointR + newRight, Color.green);
        //    prevPointR += newRight;
        //    newUp = prevPointR - activePlanets[0].transform.position;
        //    newRight = Vector3.Cross(newUp, Vector3.Cross(cam.transform.right, up)).normalized * 0.1f;
        //}

        
        

        Vector3 translation = ((Quaternion.FromToRotation(Vector3.forward, planetForward) * forward) + (Quaternion.FromToRotation(Vector3.right, planetRight) * right)).normalized * walkSpeed;
        rb.MovePosition(rb.position + translation);

        if (activePlanets.Count == 1)
        {
            float forwardAngle = Vector3.SignedAngle(up, cam.transform.up, cam.transform.right);
            float rightAngle = Vector3.SignedAngle(cam.transform.right, up, cam.transform.up);

            if (forwardAngle < 50)
            {
                cam.target.parent.rotation = Quaternion.AngleAxis(50 - forwardAngle, cam.transform.right) * cam.target.parent.rotation;
            }

            if (forwardAngle > 130)
            {
                cam.target.parent.rotation = Quaternion.AngleAxis(130 - forwardAngle, cam.transform.right) * cam.target.parent.rotation;
            }

            if (rightAngle < 50)
            {
                cam.target.parent.rotation = Quaternion.AngleAxis(rightAngle - 50, cam.transform.up) * cam.target.parent.rotation;
            }

            if (rightAngle > 130)
            {
                cam.target.parent.rotation = Quaternion.AngleAxis(rightAngle - 130, cam.transform.up) * cam.target.parent.rotation;
            }
        }

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
