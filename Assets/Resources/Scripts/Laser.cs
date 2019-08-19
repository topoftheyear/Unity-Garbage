using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Projectile
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        speed = 0.24f;
        damage = 1;

        life_death_max = 15;
        death_death_max = 15;

        xOffset = 1f;
        yOffset = -0.15f;

        animStart = "laserStart";
        animDefault = "laserDefault";
        animDestroy = "laserDestroy";

        parent = GameObject.Find("Player");

        Physics2D.IgnoreCollision(coll, parent.GetComponent<Collider2D>());
        anim.Play(animStart);
    }
}
