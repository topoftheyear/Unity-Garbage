using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int max_health;
    public int iFrames;
    public int currentFrames;

    public float speed;

    GameObject explosion;
    GameObject upgrade;
    Renderer rend;
    
    // Start is called before the first frame update
    public void Start()
    {
        health = 1;
        iFrames = 10;
        currentFrames = 0;

        speed = 0.01f;

        explosion = Resources.Load("Objects/Explosion") as GameObject;

        float num = Random.value;
        if (num < 0.5f)
        {
            upgrade = Resources.Load("Objects/Upgrades/TimeUpgrade Variant") as GameObject;
        }
        else
        {
            upgrade = Resources.Load("Objects/Upgrades/SpeedUpgrade") as GameObject;
        }


    }

    // Update is called once per frame
    void Update()
    {
        rend = GetComponent<Renderer>();
        if (rend.isVisible)
        {
            Move();
        }

        float health_ratio = (1 - health / max_health) * 45;
        print(health_ratio);
        rend.material.SetFloat("_HueShift", health_ratio);
    }

    public virtual void Move()
    {
        
    }

    public virtual void Attack()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rend.isVisible)
        {
            if (currentFrames > iFrames)
            {
                if (collision.gameObject.ToString().Contains("Laser"))
                {
                    Laser laser = collision.gameObject.GetComponent<Laser>();
                    health -= laser.damage;
                    currentFrames = 0;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (rend.isVisible)
        {
            currentFrames++;
            if (health <= 0)
            {
                GameObject thing = Instantiate(explosion);
                ExplosionBehavior behavior = thing.GetComponent<ExplosionBehavior>();

                behavior.transform.position = this.transform.position;

                // Spawn upgrade if there is one
                if (upgrade != null)
                {
                    GameObject up = Instantiate(upgrade);
                    Upgrade behave = up.GetComponent<Upgrade>();

                    behave.transform.position = this.transform.position;
                }

                Object.Destroy(gameObject);
            }
        }
    }
}
