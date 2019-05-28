using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBehavior : MonoBehaviour
{
    private int ticker;
    private int maxTick;
    private AudioSource[] audi;
    private GameObject player;
    private string part;

    private AudioClip clip0;
    private AudioClip clip1;
    private AudioClip clip2;

    public AudioClip bassVerse1;
    public AudioClip bassVerse2;
    public AudioClip bassVerse3;
    public AudioClip leadVerse1;
    public AudioClip leadVerse2;
    public AudioClip leadVerse3;
    public AudioClip drumVerse1;
    public AudioClip drumVerse2;
    public AudioClip drumVerse3;

    // Start is called before the first frame update
    void Start()
    {
        ticker = 0;
        maxTick = 60;
        audi = GetComponents<AudioSource>();
        player = GameObject.Find("Player");
        part = "verse";
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

        if (ticker == 0)
        {
            if (!Playing())
            {
                Generate();
                PlayAll();
            }
            else
            {
                ticker = -1;
            }
        }

        ticker++;
        if (ticker >= maxTick)
        {
            ticker = 0;
        }
    }

    void Generate()
    {
        if (part == "verse")
        {
            int bass = (int)Random.Range(0f, 3f);
            int lead = (int)Random.Range(0f, 3f);
            int drum = (int)Random.Range(0f, 3f);

            if (bass == 0)
            {
                clip0 = bassVerse1;
            }
            else if (bass == 1)
            {
                clip0 = bassVerse2;
            }
            else if (bass == 2)
            {
                clip0 = bassVerse3;
            }

            if (lead == 0)
            {
                clip1 = leadVerse1;
            }
            else if (lead == 1)
            {
                clip1 = leadVerse2;
            }
            else if (lead == 2)
            {
                clip1 = leadVerse3;
            }

            if (drum == 0)
            {
                clip2 = drumVerse1;
            }
            else if (drum == 1)
            {
                clip2 = drumVerse2;
            }
            else if (drum == 2)
            {
                clip2 = drumVerse3;
            }
        }


        //float max = Mathf.Max((float)clip0.length, (float)clip1.length, (float)clip2.length);
        //print("max " + max);
        //float frames = 1.0f / Time.deltaTime;
        //print("framerate " + frames);
        //maxTick = (int)(max * frames);
        //print(maxTick);
    }

    private bool Playing()
    {
        foreach (AudioSource audio in audi)
        {
            if (audio.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    void PlayAll()
    {
        foreach (AudioSource audio in audi)
        {
            audio.Stop();
        }

        audi[0].PlayOneShot(clip0);
        audi[1].PlayOneShot(clip1);
        audi[2].PlayOneShot(clip2);
    }
}
