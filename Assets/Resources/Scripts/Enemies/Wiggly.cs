using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggly : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        health = 6;
        max_health = health;

        speed = 0.008f;
    }

    public override void Move()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            float x = 0;
            float y = 0;
            if (player.transform.position.x > transform.position.x)
            {
                x = Mathf.Min(player.transform.position.x - transform.position.x, speed);
            }
            else if (player.transform.position.x < transform.position.x)
            {
                x = Mathf.Max(player.transform.position.x - transform.position.x, -speed);
            }

            if (player.transform.position.y > transform.position.y)
            {
                y = Mathf.Min(player.transform.position.y - transform.position.y, speed);
            }
            else if (player.transform.position.y < transform.position.y)
            {
                y = Mathf.Max(player.transform.position.y - transform.position.y, -speed);
            }

            this.transform.position = this.transform.position + new Vector3(x, y, 0);
        }
    }
}
