using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusBehavior : MonoBehaviour
{
    private int health;
    private int iFrames;
    private int currentFrames;

    private float maxY;
    private float currentY;
    private float minY;
    private bool up;
    private float speed;

    Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        health = 10;
        iFrames = 10;
        currentFrames = 0;

        maxY = 0.2f;
        currentY = 0f;
        minY = -0.2f;
        up = true;

        speed = 0.02f;

        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rend.isVisible)
        {
            Vector3 move;

            Vector3 current = this.transform.position;

            if (up)
            {
                currentY += speed;

                if (currentY > maxY)
                {
                    up = !up;
                }
            }
            else
            {
                currentY -= speed;

                if (currentY < minY)
                {
                    up = !up;
                }
            }

            move = new Vector3(0, currentY, 0);

            this.transform.position = this.transform.position + move;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (rend.isVisible)
        {
            if (currentFrames > iFrames)
            {
                health -= 5;
                currentFrames = 0;
            }
        }
    }

    private void LateUpdate()
    {
        if (rend.isVisible)
        {
            currentFrames++;
            if (health <= 0)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
