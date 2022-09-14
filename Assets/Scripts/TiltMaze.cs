using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltMaze : MonoBehaviour
{
    public float TiltXAmount;
    public float TiltZAmount;

    public float maxXRotation;
    public float maxZRotation;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            TiltX(-TiltXAmount);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            TiltX(TiltXAmount);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            TiltZ(-TiltZAmount);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            TiltZ(TiltZAmount);
        }
    }

    void TiltX(float amount)
    {
        Debug.Log(Mathf.Abs(Mathf.Repeat(transform.eulerAngles.x + 180, 360) - 180) + amount);
        transform.Rotate(amount, 0, 0);
        transform.rotation = Quaternion.Euler(Mathf.Clamp(Mathf.Repeat(transform.eulerAngles.x + 180, 360) - 180, -maxXRotation, maxXRotation), transform.eulerAngles.y, transform.eulerAngles.z);
        //GetComponent<BoxCollider>().enabled = false;
        //GetComponent<BoxCollider>().enabled = true;

    }

    void TiltZ(float amount)
    {
        transform.Rotate(0, 0, amount);
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x , transform.eulerAngles.y, Mathf.Clamp(Mathf.Repeat(transform.eulerAngles.z + 180, 360) - 180, -maxZRotation, maxZRotation));
        //GetComponent<BoxCollider>().enabled = false;
        //GetComponent<BoxCollider>().enabled = true;
    }

}
