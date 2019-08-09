using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sply : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        health = 2;
        max_health = health;

        speed = 0.02f;
    }

    public override void Move()
    {
        GameObject player = GameObject.Find("Player");
        float distance = Mathf.Abs(Vector3.Distance(this.transform.position, player.transform.position));

        if (distance < 6f)
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
