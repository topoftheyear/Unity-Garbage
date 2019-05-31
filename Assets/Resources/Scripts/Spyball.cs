using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spyball : Enemy
{
    private float maxY;
    private float currentY;
    private float minY;
    private bool up;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        health = 2;
        max_health = health;

        maxY = 0.1f;
        currentY = 0f;
        minY = -0.1f;
        up = true;

        speed = 0.04f;
    }

    // Update is called once per frame
    public override void Move()
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
