using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    public int lifetime;
    public int currentLife;

    // Start is called before the first frame update
    void Start()
    {
        lifetime = 44;
        currentLife = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentLife++;
        if (currentLife >= lifetime)
        {
            Object.Destroy(gameObject);
        }
    }
}
