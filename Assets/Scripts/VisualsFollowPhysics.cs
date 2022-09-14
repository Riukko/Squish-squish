using UnityEngine;

/// <summary>
/// Modifies a transform's position and rotation to maintain a constant offset with a target transform.
/// Useful for syncing the position/rotation of two objects which are siblings within the hierarchy.
/// </summary>
public class VisualsFollowPhysics : MonoBehaviour
{
    [Tooltip("Transform of the rigidbody to follow.")]
    public Transform target;
    public float moveTowardsDelta;
    Vector3 offset;

    void Start()
    {
        offset = transform.localPosition - target.localPosition;
    }

    void Update()
    {

            Vector3 rotatedOffset = target.localRotation * offset;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition + rotatedOffset, moveTowardsDelta);
            //transform.rotation = target.rotation;
    }
}
