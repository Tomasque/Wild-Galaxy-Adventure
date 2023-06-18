using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : GravityObject
{
    public static PlayerMovement instance;

    [SerializeField] float jumpForce;
    [SerializeField] float walkSpeed;
    [SerializeField] float mouseSpeed;
    [SerializeField] Transform cam;

    private bool isJumping = false;
    float xRotation = 0f;

    private void Awake()
    {
        instance = this;
    }


    private new void FixedUpdate()
    {
        base.FixedUpdate();

        Vector3 up = down == Vector3.zero ? transform.up : -down.normalized;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, down.magnitude * Time.deltaTime); ;

        //xRotation -= Input.GetAxisRaw("Mouse Y") * mouseSpeed;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        //cam.localEulerAngles = new Vector3(xRotation, cam.localEulerAngles.y, cam.localEulerAngles.z) ;

        transform.rotation = Quaternion.AngleAxis(Input.GetAxisRaw("Mouse X") * mouseSpeed, up) * transform.rotation;

        Vector3 tangentialDisplacement = (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized * walkSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + tangentialDisplacement);

        if (isJumping)
        {
            rb.AddForce(up * jumpForce);
            isJumping = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) isJumping = true;
    }
}
