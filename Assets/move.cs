using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class move : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 1;
    public float rspeed = 60;
    CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(transform.forward * speed * Time.deltaTime + Vector3.down * Time.deltaTime * 9.81f);


        int i = 0;

        if (Input.GetKey(KeyCode.D)) i++;
        if (Input.GetKey(KeyCode.A)) i--;
        transform.Rotate(Vector3.up, rspeed * i * Time.deltaTime);
    }
}
