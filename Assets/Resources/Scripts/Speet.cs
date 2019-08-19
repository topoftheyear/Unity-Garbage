using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speet : Projectile
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        speed = -0.15f;

        life_death_max = 10;
        death_death_max = 10;

        xOffset = -1.447f;
        yOffset = 0.89f;

        animStart = "speetStart";
        animDefault = "speetDefault";
        animDestroy = "speetDestroy";

        parent = GameObject.Find("Vulture (Boss)");

        Physics2D.IgnoreCollision(coll, parent.GetComponent<Collider2D>());
        anim.Play(animStart);
    }
}
