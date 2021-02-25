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

    // Screen Clamping
    public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalRot = transform.localRotation;

        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 20));
    }

    void FixedUpdate()
    {
        AirplaneMovement();
    }

    void AirplaneMovement()
    {
        Vector3 targetVelocity = new Vector3(-Input.GetAxis("Horizontal"), 0, -Input.GetAxis("Vertical"));

        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
        ClampToCamera();
        RotateBasedOnMovement();
    }

    void ClampToCamera()
    {
        float dist = 20.5f;
        float initialHeight = transform.position.y;
        Vector3 localPos = MainCamera.transform.InverseTransformPoint(transform.position);
        Vector3 leftBottom = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, dist));
        Vector3 rightTop = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, dist));
        leftBottom = MainCamera.transform.InverseTransformPoint(leftBottom);
        rightTop = MainCamera.transform.InverseTransformPoint(rightTop);

        float x = Mathf.Clamp(localPos.x, leftBottom.x, rightTop.x);
        float y = Mathf.Clamp(localPos.y, leftBottom.y, rightTop.y);

        Vector3 correctionPos = MainCamera.transform.TransformPoint(new Vector3(x, y, localPos.z));
        correctionPos.y = initialHeight;
        transform.position = correctionPos;
    }


    void RotateBasedOnMovement()
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
