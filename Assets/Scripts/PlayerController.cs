using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    public float maxVelocityChange = 10.0f;

    public float maxAngleH;
    public float angleChangeSpeed;
    
    private Quaternion originalRot;
    private Rigidbody rb;

    public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalRot = transform.localRotation;


        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        //objectWidth = 0.1; //extents = size of width / 2
        //objectHeight = 0.1; //extents = size of height / 2
    }

    void FixedUpdate()
    {
        airplaneMovement();
    }

        // Update is called once per frame
    void LateUpdate()
    {
        //restrictPlaneToCamera();
    }

    void airplaneMovement()
    {
        Vector3 targetVelocity = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));
        //Debug.Log(moveBack);

        //Debug.Log(targetVelocity);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        //Debug.Log(-Input.GetAxis("Vertical"));
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        rotateBasedOnMovement();
    }


    void rotateBasedOnMovement()
    {
        if(Input.GetAxis("Horizontal") != 0)
        {
            Quaternion target = new Quaternion();
            target.eulerAngles = new Vector3(0, 0, Input.GetAxis("Horizontal") * maxAngleH);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, target, Time.time * angleChangeSpeed);
        }
        else if(transform.localRotation != originalRot)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, originalRot, Time.time * angleChangeSpeed);
        }
    }
}
