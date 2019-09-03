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
    GameMasterBehavior gm;
    
    // Start is called before the first frame update
    public void Start()
    {
        speed = 0f;
        speedScaling = 0.5f;
        maxSpeed = 23.52f;

        wait = 0;
        maxWait = 60;

        player = GameObject.Find("Player");
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.paused)
        {
            return;
        }

        if (wait < maxWait)
        {
            wait++;
        }
        else
        {
            speed += speedScaling * Time.deltaTime;
            speedScaling *= 0.98f;
            speed = Mathf.Clamp(speed, -maxSpeed * Time.deltaTime, maxSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Object.Destroy(this.gameObject);
        }
    }
}
