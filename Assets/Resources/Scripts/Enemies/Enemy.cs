using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public int max_health;

    public float speed;

    GameObject explosion;
    public GameObject upgrade;
    Renderer rend;

    ArrayList collisions;
    
    // Start is called before the first frame update
    public void Start()
    {
        health = 1;

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

        rend = this.GetComponent<Renderer>();

        Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), GameObject.Find("GameMaster").GetComponent<Collider2D>());

        foreach (Enemy thing in GameObject.FindObjectsOfType<Enemy>())
        {
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), thing.gameObject.GetComponent<Collider2D>());
        }

        collisions = new ArrayList();
    }

    // Update is called once per frame
    void Update()
    {
        if (rend.isVisible)
        {
            Move();
            Attack();
        }

        float health_ratio = (1 - ((float)health / (float)max_health)) * 90;
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
            if (collision.gameObject.ToString().Contains("Laser"))
            {
                // If object has already been collided with, don't bother
                if (!collisions.Contains(collision.gameObject))
                {
                    collisions.Add(collision.gameObject);
                    Laser laser = collision.gameObject.GetComponent<Laser>();
                    health -= laser.damage;
                    StartCoroutine(FlashSprites(rend, 2, 0.04f));
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (rend.isVisible)
        {
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

    /**
     * Coroutine to create a flash effect on all sprite renderers passed in to the function.
     *
     * @param sprites   a sprite renderer array
     * @param numTimes  how many times to flash
     * @param delay     how long in between each flash
     * @param disable   if you want to disable the renderer instead of change alpha
     */
    IEnumerator FlashSprites(Renderer sprite, int numTimes, float delay, bool disable = false)
    {
        // number of times to loop
        for (int loop = 0; loop < numTimes; loop++)
        {
            // cycle through all sprites
            if (disable)
            {
                // for disabling
                sprite.enabled = false;
            }
            else
            {
                // for changing the saturation
                rend.material.SetFloat("_Sat", 0f);
            }

            // delay specified amount
            yield return new WaitForSeconds(delay);

            if (disable)
            {
                // for disabling
                sprite.enabled = true;
            }
            else
            {
                // for changing the saturation
                rend.material.SetFloat("_Sat", 1f);
            }

            // delay specified amount
            yield return new WaitForSeconds(delay);
        }
    }
}
