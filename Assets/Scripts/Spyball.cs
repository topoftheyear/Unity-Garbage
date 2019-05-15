﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spyball : MonoBehaviour
{
    private int health;
    private int iFrames;
    private int currentFrames;

    private float maxY;
    private float currentY;
    private float minY;
    private bool up;
    private float speed;

    public GameObject explosion;

    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        health = 2;
        iFrames = 10;
        currentFrames = 0;

        maxY = 0.1f;
        currentY = 0f;
        minY = -0.1f;
        up = true;

        speed = 0.04f;

        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rend.isVisible)
        {
            Vector3 move;

            Vector3 current = this.transform.position;

            if (up)
            {
                currentY += speed / 4;

                if (currentY > maxY)
                {
                    up = !up;
                }
            }
            else
            {
                currentY -= speed / 4;

                if (currentY < minY)
                {
                    up = !up;
                }
            }

            move = new Vector3(-speed, currentY, 0);

            this.transform.position = this.transform.position + move;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rend.isVisible)
        {
            if (currentFrames > iFrames)
            {
                if (collision.gameObject.ToString().Contains("Laser"))
                {
                    Laser laser = collision.gameObject.GetComponent<Laser>();
                    print(laser.damage);
                    health -= laser.damage;
                    currentFrames = 0;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (rend.isVisible)
        {
            currentFrames++;
            if (health <= 0)
            {
                GameObject thing = Instantiate(explosion);
                ExplosionBehavior behavior = thing.GetComponent<ExplosionBehavior>();

                behavior.transform.position = this.transform.position;

                Object.Destroy(gameObject);
            }
        }
    }
}
