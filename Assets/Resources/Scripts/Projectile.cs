using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;

    public GameObject parent;

    public Animator anim;
    public Renderer rend;
    public Collider2D coll;

    private int life_counter;
    private int death_counter;
    public int life_death_max;
    public int death_death_max;

    public string animStart;
    public string animDefault;
    public string animDestroy;

    public float xOffset;
    public float yOffset;

    // Start is called before the first frame update
    public void Start()
    {
        speed = 0.24f;
        damage = 1;
        anim = this.GetComponent<Animator>();
        rend = this.GetComponent<Renderer>();

        life_counter = 0;
        death_counter = -1;
        life_death_max = 15;

        coll = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(coll, GameObject.Find("GameMaster").GetComponent<Collider2D>());
        foreach (Projectile thing in GameObject.FindObjectsOfType<Projectile>())
        {
            Physics2D.IgnoreCollision(coll, thing.gameObject.GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    public void Update()
    {
        if (life_counter < life_death_max)
        {
            life_counter++;
            this.transform.position = parent.transform.position + new Vector3(xOffset, yOffset);
            anim.Play(animStart);
        }
        else if (death_counter > -1 && death_counter < death_death_max)
        {
            death_counter++;
            anim.Play(animDestroy);
        }
        else if (death_counter >= death_death_max || !rend.isVisible)
        {
            Object.Destroy(gameObject);
        }
        else
        {
            anim.Play(animDefault);
            this.transform.position = this.transform.position + new Vector3(speed, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string name = collision.gameObject.name;
        if (!name.Contains("Upgrade"))
        {
            if (death_counter < 0)
            {
                death_counter++;
            }
        }
    }
}
