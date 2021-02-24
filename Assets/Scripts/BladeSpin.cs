using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSpin : MonoBehaviour
{
    public float bladeSpeed;
    public GameObject bladeCenter;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(bladeCenter.transform.position, Vector3.right, bladeSpeed * Time.deltaTime);
    }
}
