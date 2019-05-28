using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float speed;
    public int damage;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.24f;
        player = GameObject.Find("Player");
        damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position + new Vector3(speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string name = collision.gameObject.name;
        print("Laser collided with: " + name);
        if (collision.gameObject != player && !name.Contains("Upgrade") && !name.Contains("Laser"))
        {
            Object.Destroy(gameObject);
        }
    }
}
