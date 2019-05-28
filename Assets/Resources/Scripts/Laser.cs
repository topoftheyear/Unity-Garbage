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
        if (collision.gameObject != player)
        {
            Object.Destroy(gameObject);
        }
    }
}
