﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VultureBoss : Enemy
{
    GameObject player;

    private int attackCounter;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        health = 50;
        max_health = health;

        player = GameObject.Find("Player");

        speed = 0.1f;

        upgrade = Resources.Load("Objects/Upgrades/PowerUpgrade Variant") as GameObject;

        attackCounter = 0;
    }

    public override void Move()
    {
        this.transform.position = new Vector3(365, player.transform.position.y - 0.8f);

        
    }

    public override void Attack()
    {
        // If the vulture can attack
        if (Mathf.Abs(player.transform.position.y - 0.8f - this.transform.position.y) < 0.25f && attackCounter == 30)
        {
            GameObject thing = Instantiate(Resources.Load("Objects/Weaponry/Speet") as GameObject);
            thing.transform.position = transform.position + new Vector3(-1.447f, 0.89f);

            attackCounter++;
        }
        // When the vulture has stopped attacking
        else if (attackCounter > 40)
        {
            GetComponent<Animator>().Play("vultureDefault");
            attackCounter = 0;
        }
        // While the vulture is attacking
        else if (attackCounter > 30)
        {
            GetComponent<Animator>().Play("vultureAttack");
            attackCounter++;
        }
        // When the vulture is doing nothing else
        else
        {
            attackCounter++;
            attackCounter = Mathf.Min(attackCounter, 30);
        }
    }
}
