using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * cameraSpeed);
    }
}
