using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMove : MonoBehaviour
{
    public float speed;
    public float baseRightSpeed;

    public GameObject basicLaser;

    // Use this for initialization
    void Start()
    {
        speed = 0.08f;
        baseRightSpeed = 0.04f;
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

        horizontalMove += baseRightSpeed;

        // transformo objectu
        this.transform.position += new Vector3(horizontalMove, verticalMove);

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject thing = Instantiate(basicLaser);
        Laser behavior = thing.GetComponent<Laser>();

        behavior.transform.position = this.transform.position + new Vector3(1,0);

        Destroy(thing, 2f);
    }
}
