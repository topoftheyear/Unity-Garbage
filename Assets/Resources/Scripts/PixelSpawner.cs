using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelSpawner : MonoBehaviour
{
    GameObject player;
    public GameObject ztar;

    public Sprite one;
    public Sprite two;
    public Sprite three;
    public Sprite four;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + new Vector3(15f, 0);

        int num = (int)Mathf.Round(Random.value * 3);

        if (num == 2)
        {
            GameObject thing = Instantiate(ztar);
            int range = 60;
            float position = Random.value * range;

            thing.transform.position = this.transform.position + new Vector3(0, position - (range / 2), 100);

            int choice = (int)Mathf.Round(Random.value * 4);
            SpriteRenderer rend = thing.GetComponent<SpriteRenderer>();

            if (choice == 1)
            {
                rend.sprite = one;
            }
            else if (choice == 2)
            {
                rend.sprite = two;
            }
            else if (choice == 3)
            {
                rend.sprite = three;
            }
            else
            {
                rend.sprite = four;
            }
        }
    }
}
