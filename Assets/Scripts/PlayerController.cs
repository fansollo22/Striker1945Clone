using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20.0f;

    public float maxAngleH;
    public float angleChangeSpeed;

    public GameObject movingContainer;
    public float containerSpeed;
    
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

    void Update()
    {
        MoveContainer();
    }

    void FixedUpdate()
    {
        AirplaneMovement();
        RotateBasedOnMovement();
    }

    void AirplaneMovement()
    {
        Vector3 v = new Vector3(Input.GetAxis("Vertical") , 0, -Input.GetAxis("Horizontal")) * speed * Time.deltaTime;
        Vector3 pos = transform.position + v;
        Vector3 clamp = ClampToCamera(pos);
        
        transform.position = clamp;
    }

    Vector3 ClampToCamera(Vector3 playerPos)
    {
        float dist = 20.5f;
        float initialHeight = playerPos.y;
        Vector3 localPos = MainCamera.transform.InverseTransformPoint(playerPos);
        Vector3 leftBottom = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, dist));
        Vector3 rightTop = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, dist));
        leftBottom = MainCamera.transform.InverseTransformPoint(leftBottom);
        rightTop = MainCamera.transform.InverseTransformPoint(rightTop);

        float x = Mathf.Clamp(localPos.x, leftBottom.x, rightTop.x);
        float y = Mathf.Clamp(localPos.y, leftBottom.y, rightTop.y);
        Vector3 correctionPos = MainCamera.transform.TransformPoint(new Vector3(x, y, localPos.z));
        correctionPos.y = initialHeight;
        return correctionPos;
    }

    void MoveContainer()
    {
        movingContainer.transform.Translate(-Vector3.forward * Time.deltaTime * containerSpeed);
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
