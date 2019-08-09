using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : Enemy
{
    private GameObject player;
    private float rotateSpeed = 2f;
    private float radius = 4f;

    private Vector2 _center;
    private float _angle;

    public float test;
    
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        health = 2;
        max_health = health;
    }

    public override void Move()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            _center = player.transform.position;
        }

        _angle += rotateSpeed * Time.deltaTime;

        var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * radius;
        transform.position = _center + offset;
        transform.Rotate(new Vector3(0, 0, test));
        //transform.localEulerAngles = new Vector3(0, 0, 0);

        // Clamp the boss inside camera view
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.05f, 0.95f);
        pos.y = Mathf.Clamp(pos.y, 0.05f, 0.95f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

        if (health <= max_health / 2)
        {
            Camera.main.transform.localEulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
        }
    }
}
