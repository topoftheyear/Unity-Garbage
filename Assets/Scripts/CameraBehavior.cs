using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private GameObject player;
    private float rightSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            rightSpeed = player.GetComponent<PlayerBehavior>().baseRightSpeed;
        }
        catch (UnityEngine.UnityException)
        {
            print("player not found by camera and is likely dead :)");
        }
        transform.position = transform.position + new Vector3(rightSpeed, 0);
    }
}
