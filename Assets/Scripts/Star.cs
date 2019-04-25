﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float speed;
    Camera main;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.value / 32;
        main = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position - new Vector3(speed, 0, 0);

        if (this.transform.position.x <= main.transform.position.x - 40f)
        {
            Destroy(this.gameObject);
        }
    }
}