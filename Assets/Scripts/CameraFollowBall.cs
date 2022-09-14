using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBall : MonoBehaviour
{
    Vector3 startOffsetWithBall;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        startOffsetWithBall = transform.position - ball.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ball.transform.position + startOffsetWithBall;
    }
}
