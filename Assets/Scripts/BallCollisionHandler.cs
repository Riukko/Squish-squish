using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GetComponentInParent<MoveBall>().OnCollisionEnter(collision);
    }
}
