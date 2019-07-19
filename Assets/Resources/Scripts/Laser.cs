﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float speed;
    public int damage;

    public GameObject player;

    private Animator anim;
    private Renderer rend;

    private int life_counter;
    private int death_counter;
    private int life_death_max;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.24f;
        damage = 1;
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();
        rend = GetComponent<Renderer>();

        life_counter = 0;
        death_counter = -1;
        life_death_max = 15;

        anim.Play("laserStart");

        Collider2D coll = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(coll, player.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(coll, GameObject.Find("GameMaster").GetComponent<Collider2D>());
        foreach (Laser thing in GameObject.FindObjectsOfType<Laser>())
        {
            Physics2D.IgnoreCollision(coll, thing.gameObject.GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (life_counter < life_death_max)
        {
            life_counter++;
            this.transform.position = player.transform.position + new Vector3(1f, -0.15f);
            anim.Play("laserStart");
        }
        else if (death_counter > -1 && death_counter < life_death_max)
        {
            death_counter++;
            anim.Play("laserDestroy");
        }
        else if (death_counter >= life_death_max || !rend.isVisible)
        {
            Object.Destroy(gameObject);
        }
        else
        {
            anim.Play("laserDefault");
            this.transform.position = this.transform.position + new Vector3(speed, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string name = collision.gameObject.name;
        if (collision.gameObject != player && !name.Contains("Upgrade") && !name.Contains("Laser"))
        {
            if (death_counter < 0)
            {
                death_counter++;
            }
        }
    }
}
