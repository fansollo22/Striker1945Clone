using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundaries : MonoBehaviour
{
    public Camera MainCamera; //be sure to assign this in the inspector to your main camera
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    // Use this for initialization
    void Start()
    {
        screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 20));
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float dist = 20.5f;
        float yT = transform.position.y;
        Vector3 localPos = MainCamera.transform.InverseTransformPoint(transform.position);
        Vector3 leftBottom = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, dist));
        Vector3 rightTop = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, dist));
        leftBottom = MainCamera.transform.InverseTransformPoint(leftBottom);
        rightTop = MainCamera.transform.InverseTransformPoint(rightTop);

        float x = Mathf.Clamp(localPos.x, leftBottom.x, rightTop.x);
        float y = Mathf.Clamp(localPos.y, leftBottom.y, rightTop.y);

        Vector3 temp = MainCamera.transform.TransformPoint(new Vector3(x, y, localPos.z));
        temp.y = yT;

        transform.position = temp;
    }

}
