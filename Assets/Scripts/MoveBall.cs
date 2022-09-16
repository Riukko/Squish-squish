using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Linq;
using System.Threading;

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
    public bool jump = false;

    public GameObject vfxJump;

    // Start is called before the first frame update
    void Start()
    {
        TCPServerTest.OnIncomingMessage += ManageBallControllerInputs;
    }


    // Update is called once per frame
    void Update()
    {
        ////Arrow keys movements
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
        ////
        ////Arduino movements
        if (jump)
        {
            JumpBall();
        }
        rb.AddForce(sphereForward.right * moveForce * zMoveForce);
        rb.AddForce(sphereForward.forward * moveForce * yMoveForce);
    }

    public void JumpBall()
    {
        if (!hasJumped)
        {
            rb.AddForce(sphereForward.up * jumpForce, ForceMode.Impulse);
            hasJumped = true;
            this.GetComponentInChildren<TrailRenderer>().enabled = true;
            SoundManager.Instance.PlayJumpSound();
            jump = false;
        }
            
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && hasJumped)
        {
            hasJumped = false;
            this.GetComponentInChildren<TrailRenderer>().enabled = false;
            var vfx = Instantiate(vfxJump, collision.contacts[0].point, Quaternion.identity);
            vfx.transform.parent = transform;
            vfx.transform.rotation = Quaternion.identity;
            vfx.transform.localScale = Vector3.one * 0.02f;
            StartCoroutine(DestroyVFX(vfx, vfx.GetComponent<ParticleSystem>().main.duration));
        }
    }

    private IEnumerator DestroyVFX(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }

    #region TCP Messages managements
    void ManageBallControllerInputs(string tcpMessage)
    {
        List<float> xInputs = new List<float>();
        List<float> yInputs = new List<float>();
        List<float> zInputs = new List<float>();
        string[] messages = tcpMessage.Split(';');
        messages = messages.Take(messages.Length - 1).ToArray();

        foreach(string message in messages)
        {
            if(!string.IsNullOrEmpty(message))
            {
                //Debug.Log(message);
                string[] inputs = message.Split(':');
                if (inputs.Length > 1 && !string.IsNullOrEmpty(inputs[1]))
                {
                    switch (inputs[0])
                    {
                        case "X":
                            xInputs.Add(float.Parse(inputs[1], CultureInfo.InvariantCulture.NumberFormat));
                            break;

                        case "Y":
                            yInputs.Add(float.Parse(inputs[1], CultureInfo.InvariantCulture.NumberFormat));
                            break;

                        case "Z":
                            zInputs.Add(float.Parse(inputs[1], CultureInfo.InvariantCulture.NumberFormat));
                            break;
                    }
                    string debugY = "Y inputs : ";
                    string debugZ = "Z inputs : ";
                    for (int i = 0; i < yInputs.Count; i++)
                    {
                        debugY += yInputs[i].ToString();
                        debugY += "; ";
                    }
                    for (int i = 0; i < zInputs.Count; i++)
                    {
                        debugZ += zInputs[i].ToString();
                        debugZ += "; ";
                    }
                    zMoveForce = averageList(zInputs, zMoveForce);
                    yMoveForce = averageList(yInputs, yMoveForce);
                    xMoveForce = averageList(xInputs, xMoveForce);
                }
                else if (inputs[0] == "JUMP")
                {
                    jump = true;
                }
                
            }
        }
    }
    

    float averageList(List<float> floatList, float moveForce = 0)
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
            return moveForce;
        }
            
    }
    #endregion
}


