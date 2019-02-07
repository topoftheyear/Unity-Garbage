using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangBehavior : MonoBehaviour {

    public Vector3 destination;
    bool reached;
    float speed;

	// Use this for initialization
	void Start () {
        speed = 0.3f;
        reached = false;
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, 10));

        if (!(reached))
        {
            // Approach outDestination
            transform.position = Vector3.MoveTowards(transform.position, destination, speed);
            if (transform.position == destination)
            {
                reached = true;
            }
        }
        else
        {
            GameObject player = GameObject.Find("/Player");
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed);
            if (transform.position == player.transform.position)
            {
                Object.Destroy(gameObject);
            }
        }
	}

    void giveDestination(Vector3 des)
    {
        destination = des;
    }
}
