using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splop : Enemy
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        health = 4;
        max_health = health;

        speed = 0.02f;
    }

    // Update is called once per frame
    public override void Move()
    {
        this.transform.position += new Vector3(-speed, 0);
    }
}
