using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    public float speed;
    public float baseRightSpeed;

    public GameObject basicLaser;
    private int shootCounter;
    public int shootActivate;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        speed = 0.02f;
        baseRightSpeed = 0.005f;

        shootCounter = 0;
        shootActivate = 20;

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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

    void Shoot()
    {
        GameObject thing = Instantiate(basicLaser);
        Laser behavior = thing.GetComponent<Laser>();

        behavior.transform.position = this.transform.position + new Vector3(1,0);

        Destroy(thing, 2f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject == GameObject.Find("Fast Upgrade"))
        {
            speed += 0.01f;
            Destroy(other.gameObject);
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
