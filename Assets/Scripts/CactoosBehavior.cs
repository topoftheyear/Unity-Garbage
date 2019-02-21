using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CactoosBehavior : MonoBehaviour {

    GameObject player;
    float speed;

    int maxHealth;
    int health;

    int i_frames;
    int current_frames;

    public Text textThing;

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        speed = 0.035f;

        maxHealth = 100;
        health = maxHealth;

        i_frames = 10;
        current_frames = i_frames;

        textThing = Instantiate(textThing);
        textThing.transform.SetParent(GameObject.Find("Canvas").transform);
	}
	
	// Update is called once per frame
	void Update () {
        var player_position = player.transform.position;
        var our_position = this.transform.position;

        float x = 0f;
        float y = 0f;

        if (player_position.x > our_position.x)
        {
            x += Mathf.Min(speed, Mathf.Abs(player_position.x - our_position.x));
        }
        else if (player_position.x < our_position.x)
        {
            x -= Mathf.Min(speed, Mathf.Abs(player_position.x - our_position.x));
        }

        if (player_position.y > our_position.y)
        {
            y += Mathf.Min(speed, Mathf.Abs(player_position.y - our_position.y));
        }
        else if (player_position.y < our_position.y)
        {
            y -= Mathf.Min(speed, Mathf.Abs(player_position.y - our_position.y));
        }

        this.transform.position = this.transform.position + new Vector3(x, y);

        textThing.rectTransform.position = Camera.main.WorldToScreenPoint(this.transform.position);
        textThing.text = health.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (current_frames > i_frames)
        {
            health -= 5;
            current_frames = 0;
        }
    }

    private void LateUpdate()
    {
        current_frames++;
        if (health <= 0)
        {
            DestroyObject(textThing);
            DestroyObject(gameObject);
        }
    }
}
