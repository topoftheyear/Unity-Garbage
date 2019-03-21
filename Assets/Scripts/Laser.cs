using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.16f;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position + new Vector3(speed, 0);
    }
}
