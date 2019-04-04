using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        var x = this.transform.position.x;
        var y = this.transform.position.y;
        var z = this.transform.position.z;

        x = Mathf.Round(x * 100);
        y = Mathf.Round(y * 100);

        x /= 100;
        y /= 100;

        this.transform.position = new Vector3(x, y, z);
    }
}
