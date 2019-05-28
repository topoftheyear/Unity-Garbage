using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawner : MonoBehaviour {

    int timer;

    public GameObject enemy;

	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 400)
        {
            Instantiate(enemy);
            timer = 0;
        }
	}

    private void LateUpdate()
    {
        timer++;
    }
}
