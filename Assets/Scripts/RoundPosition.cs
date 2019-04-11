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

        int num = 1000;

        x = Mathf.Round(x * num);
        y = Mathf.Round(y * num);

        x /= num;
        y /= num;

        this.transform.position = new Vector3(x, y, z);
    }
}
