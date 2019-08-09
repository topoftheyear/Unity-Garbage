using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sloom : Enemy
{
    float maxVSpeed;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        health = 4;
        max_health = health;

        maxVSpeed = 0.01f;
        speed = 0.04f;
    }

    public override void Move()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            float y = 0;
            if (player.transform.position.y > transform.position.y)
            {
                y = Mathf.Min(player.transform.position.y - transform.position.y, maxVSpeed);
            }
            else if (player.transform.position.y < transform.position.y)
            {
                y = Mathf.Max(player.transform.position.y - transform.position.y, -maxVSpeed);
            }
            this.transform.position = this.transform.position + new Vector3(-speed, y, 0);
        }
    }
}
