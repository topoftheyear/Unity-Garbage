using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float speed;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.24f;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position + new Vector3(speed, 0);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject != player)
        {
            Object.Destroy(gameObject);
        }
    }
}
