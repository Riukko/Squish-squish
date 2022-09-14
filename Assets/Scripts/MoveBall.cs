using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    public Rigidbody rb;
    public Transform sphereForward;
    public float moveForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(-sphereForward.right * moveForce * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(sphereForward.right * moveForce * Time.fixedDeltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddForce(-sphereForward.forward * moveForce * Time.fixedDeltaTime);
        }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForce(sphereForward.forward * moveForce * Time.fixedDeltaTime);
        }
        }
    }
