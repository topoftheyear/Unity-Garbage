using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour {

    public Vector2 horizontal_direction = new Vector2(0, 0);
    public Vector2 vertical_direction = new Vector2(0, 0);

    float speed = 0.01f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = (horizontal_direction + vertical_direction) * speed + new Vector2(this.transform.position.x, this.transform.position.y);

        speed += 0.01f;
    }
}
