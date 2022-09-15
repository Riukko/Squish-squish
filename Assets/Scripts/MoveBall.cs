using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Linq;

public class MoveBall : MonoBehaviour
{
    public Rigidbody rb;
    public Transform sphereForward;
    public float moveForce;
    public float xMoveForce = 0;
    public float yMoveForce = 0;
    public float zMoveForce = 0;
    public bool moveBallWithArrowKeys = true;
    public float jumpForce;

    public bool hasJumped;

    // Start is called before the first frame update
    void Start()
    {
        TCPServerTest.OnIncomingMessage += ManageBallControllerInputs;
    }


    // Update is called once per frame
    void Update()
    {
        if (moveBallWithArrowKeys)
        {

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddForce(-sphereForward.right * moveForce * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                rb.AddForce(sphereForward.right * moveForce * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                rb.AddForce(-sphereForward.forward * moveForce * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb.AddForce(sphereForward.forward * moveForce * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                JumpBall();
            }
        }

        rb.AddForce(sphereForward.right * moveForce * yMoveForce);
        rb.AddForce(sphereForward.forward * moveForce * zMoveForce);
    }

    public void JumpBall()
    {
        if (!hasJumped)
        {
            rb.AddForce(sphereForward.up * jumpForce, ForceMode.Impulse);
            hasJumped = true;
            this.GetComponentInChildren<TrailRenderer>().enabled = true;
        }
            
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && hasJumped)
        {
            hasJumped = false;
            this.GetComponentInChildren<TrailRenderer>().enabled = false;
        }
    }

    #region TCP Messages managements
    void ManageBallControllerInputs(string tcpMessage)
    {
        string[] messages = tcpMessage.Split(';');
        messages = messages.Take(messages.Length - 1).ToArray();

        foreach(string message in messages)
        {
            if(!string.IsNullOrEmpty(message))
            {
                //Debug.Log(message);
                string[] inputs = message.Split(':');
                List<float> xInputs = new List<float>();
                List<float> yInputs = new List<float>();
                List<float> zInputs = new List<float>();
                if (inputs.Length > 1 && !string.IsNullOrEmpty(inputs[1]))
                {
                    switch (inputs[0])
                    {
                        case "X":
                            xInputs.Add(float.Parse(inputs[1], CultureInfo.InvariantCulture.NumberFormat));
                            break;

                        case "Y":
                            //Debug.Log(inputs[1]);
                            yInputs.Add(float.Parse(inputs[1], CultureInfo.InvariantCulture.NumberFormat));
                            break;

                        case "Z":
                            zInputs.Add(float.Parse(inputs[1], CultureInfo.InvariantCulture.NumberFormat));
                            break;
                    }
                    string debug = "";
                    for (int i = 0; i < yInputs.Count; i++)
                    {
                        debug += yInputs[i].ToString();
                        debug += "; ";
                    }
                    Debug.Log(debug);
                    zMoveForce = averageList(zInputs);
                    yMoveForce = averageList(yInputs);
                    //xMoveForce = averageList(xInputs);
                }
                else
                {

                }
                
            }
        }
    }
    

    float averageList(List<float> floatList)
    {
        if(floatList.Count != 0)
        {
            float sum = 0;
            for (int i = 0; i < floatList.Count; i++)
            {
                sum += floatList[i];
            }
            return sum / floatList.Count;
        }
        else
        {
            return 0;
        }
            
    }
    #endregion
}


