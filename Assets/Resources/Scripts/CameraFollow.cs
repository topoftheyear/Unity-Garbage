using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private GameObject player;
    public float closingAmount = 5f;
    private Camera cam;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        cam = Camera.main;
	}

    // Update is called once per frame
    void Update() {
        Vector3 starting = this.transform.position;

        float x = (player.transform.position.x - starting.x) / closingAmount;
        float y = (player.transform.position.y - starting.y) / closingAmount;

        // If slowdown look here first!

        Vector3 distance = new Vector3(x, y);

        // set camera location
        this.transform.position += distance;

        // temp
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //cam.orthographicSize -= 0.05f * cam.orthographicSize / 4;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //cam.orthographicSize += 0.05f * cam.orthographicSize / 4;
        }

	}
}
