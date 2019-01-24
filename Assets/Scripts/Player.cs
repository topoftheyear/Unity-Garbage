using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;

    public GameObject laserObject = null;

	// Use this for initialization
	void Start () {
        speed = 0.128f;
	}
	
	// Update is called once per frame
	void Update () {
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

        // transformo objectu
        this.transform.position += new Vector3(horizontalMove, verticalMove);

        // laser spawn
        if (Input.GetKey(KeyCode.F))
        {
            // Help
            GameObject bullshit = Instantiate(laserObject);
            LaserBehavior otherBullshit = bullshit.GetComponent<LaserBehavior>();

            List<Vector2> list = new List<Vector2>(new Vector2[] { Vector2.left, Vector2.right, Vector2.zero });
            otherBullshit.horizontal_direction = list[(int)Mathf.Floor(Random.Range(0, 2.9f))];

            List<Vector2> otherList = new List<Vector2>(new Vector2[] { Vector2.up, Vector2.down, Vector2.zero });
            otherBullshit.vertical_direction = otherList[(int)Mathf.Floor(Random.Range(0, 2.9f))];

            otherBullshit.transform.position = this.transform.position;
            otherBullshit.transform.rotation = Random.rotation;

            Destroy(bullshit, 2);
        }
	}

    private float Fuckyou(float number)
    {
        if (number > 0.01){
            return number - 0.01f;
        }
        else if (number < -0.01)
        {
            return number + 0.01f;
        }
        else
        {
            return 0;
        }
    }
}
