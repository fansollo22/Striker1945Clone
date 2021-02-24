using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float fireRate;
    public GameObject firePoint;
    public List<GameObject> vfx = new List<GameObject>();

    private GameObject vfxSpawn;
    private float timeToFire = 0;
    // Start is called before the first frame update
    void Start()
    {
        vfxSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space) && Time.time > timeToFire)
        {
            SpawnVFX();
            timeToFire = Time.time + 1 / fireRate;
        }
    }

    void SpawnVFX()
    {
        GameObject vfx;

        if(firePoint != null)
        {
            vfx = Instantiate(vfxSpawn, firePoint.transform.position, firePoint.transform.rotation);
        }
    }
}
