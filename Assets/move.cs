using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1;
    public float rspeed = 60;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


        int i = 0;

        if (Input.GetKey(KeyCode.D)) i++;
        if (Input.GetKey(KeyCode.A)) i--;
        transform.Rotate(Vector3.up, rspeed * i * Time.deltaTime);
    }
}
