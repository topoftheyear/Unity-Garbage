using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class PlayerBehavior : MonoBehaviour
{
    public float speed;
    public float baseRightSpeed;

    public GameObject basicLaser;
    public int damage;
    private int shootCounter;
    public float shootActivate;

    private Animator anim;
    private AudioSource audi;

    public AudioClip pain;

    private int dead;
    public GameObject explosion;

    private GameMasterBehavior gm;

    // Use this for initialization
    void Start()
    {
        Application.targetFrameRate = 60;

        speed = 0.05f;
        baseRightSpeed = 0.02f;

        damage = 1;
        shootCounter = 0;
        shootActivate = 30f;

        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();

        dead = 0;

        gm = GameObject.Find("GameMaster").GetComponent<GameMasterBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == 0)
        {
            // Player Movement check
            float verticalMove = 0f;
            float horizontalMove = 0f;

            if (Input.GetKey(KeyCode.W))
            {
                verticalMove += speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                verticalMove -= speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                horizontalMove -= speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                horizontalMove += speed;
            }

            if (verticalMove != 0 && horizontalMove != 0)
            {
                verticalMove /= Mathf.Sqrt(2);
                horizontalMove /= Mathf.Sqrt(2);
            }

            if (verticalMove > 0)
            {
                anim.Play("shipUp");
            }
            else if (verticalMove < 0)
            {
                anim.Play("shipDown");
            }
            else
            {
                anim.Play("shipBasic");
            }

            horizontalMove += baseRightSpeed;

            // transformo objectu
            this.transform.position += new Vector3(horizontalMove, verticalMove);
            // Fix rotation
            this.transform.rotation = new Quaternion(0, 0, 0, 0);

            if (Input.GetKey(KeyCode.Space))
            {
                if (shootCounter >= shootActivate)
                {
                    Shoot();
                    shootCounter = 0;
                }
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                shootCounter += 2;
            }
        }
        else if (dead == 1)
        {
            dead++;
            GameObject thing = Instantiate(explosion);
            ExplosionBehavior behavior = thing.GetComponent<ExplosionBehavior>();

            behavior.transform.position = this.transform.position;
            GetComponent<Renderer>().enabled = false;
        }
        else if (dead < 100)
        {
            dead += 1;
        }
        else
        {
            gm.PlayerDied();
        }
    }

    void Shoot()
    {
        GameObject thing = Instantiate(basicLaser);
        Laser behavior = thing.GetComponent<Laser>();

        behavior.transform.position = this.transform.position + new Vector3(1f, -0.15f);
        behavior.damage = damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (dead == 0)
        {
            if (other.gameObject.name.Contains("Upgrade"))
            {
                Upgrade upgrade = other.gameObject.GetComponent<Upgrade>();
                string type = upgrade.upgradeName;

                if (type == "Speed")
                {
                    SpeedUpgrade();
                }
                if (type == "Time")
                {
                    TimeUpgrade();
                }
                if (type == "Power")
                {
                    PowerUpgrade();
                }
            }
            else
            {
                audi.clip = pain;
                audi.Play();
                dead = 1;
            }
        }
    }

    private void LateUpdate()
    {
        shootCounter++;
        if (shootCounter > 1000)
        {
            shootCounter = 1000;
        }

        // Clamp the ship inside camera view
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.05f, 0.95f);
        pos.y = Mathf.Clamp(pos.y, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void SpeedUpgrade()
    {
        speed += 0.005f;
    }

    void TimeUpgrade()
    {
        shootActivate -= 1f;
    }

    void PowerUpgrade()
    {
        damage += 1;
    }
}
