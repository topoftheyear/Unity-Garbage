using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class BasicMove : MonoBehaviour
{
    public float speed;
    public float baseRightSpeed;

    public GameObject basicLaser;
    private int shootCounter;
    public int shootActivate;

    private Animator anim;
    private AudioSource audi;

    public AudioClip pain;

    private int dead;

    // Use this for initialization
    void Start()
    {
        speed = 0.10f;
        baseRightSpeed = 0.02f;

        shootCounter = 0;
        shootActivate = 20;

        anim = GetComponent<Animator>();
        audi = GetComponent<AudioSource>();

        dead = 0;
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
        else if (dead < 6000)
        {
            dead += 1;
        }
        else
        {
            Application.LoadLevel(0); // SpaceTest
        }
    }

    void Shoot()
    {
        GameObject thing = Instantiate(basicLaser);
        Laser behavior = thing.GetComponent<Laser>();

        behavior.transform.position = this.transform.position + new Vector3(1.3f, -0.2f);

        Destroy(thing, 2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (dead == 0)
        {
            print(other.gameObject);

            if (other.gameObject == GameObject.Find("Fast Upgrade"))
            {
                speed += 0.01f;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.ToString().Contains("Laser"))
            {
                // This space left intentionally blank
            }
            else
            {
                audi.clip = pain;
                audi.Play();
                dead = 1;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print(other.gameObject);

        if (other.gameObject.ToString().Contains("Scene"))
        {
            Application.LoadLevel(other.gameObject.GetComponent<SceneTransfer>().scene);
        }
    }

    private void LateUpdate()
    {
        shootCounter++;
        if (shootCounter > 1000)
        {
            shootCounter = 1000;
        }
    }
}
