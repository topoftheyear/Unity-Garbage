using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public string upgradeName;
    public float speed;
    public float speedScaling;
    public float maxSpeed;

    public int wait;
    public int maxWait;

    public GameObject player;
    
    // Start is called before the first frame update
    public void Start()
    {
        speed = 0f;
        speedScaling = 0.01f;
        maxSpeed = 0.4f;

        wait = 0;
        maxWait = 60;

        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (wait < maxWait)
        {
            wait++;
        }
        else
        {
            speed += speedScaling;
            speedScaling *= 0.98f;
            speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.gameObject.name);
        if (collision.gameObject.name == "Player")
        {
            Object.Destroy(this.gameObject);
        }
    }
}
