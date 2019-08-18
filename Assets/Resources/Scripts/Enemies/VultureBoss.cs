using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureBoss : Enemy
{
    GameObject player;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        health = 50;
        max_health = health;

        player = GameObject.Find("Player");

        speed = 0.1f;

        upgrade = Resources.Load("Objects/Upgrades/PowerUpgrade Variant") as GameObject;
    }

    public override void Move()
    {
        Vector3 move = new Vector3(0, Random.Range(0, speed) - speed / 2);
        
        this.transform.position = this.transform.position + move;
        this.transform.position = new Vector3(365, this.transform.position.y);
    }
}
