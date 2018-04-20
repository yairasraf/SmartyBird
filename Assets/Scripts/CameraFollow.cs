using UnityEngine;

/// <summary>
/// This script is supposed to follow all birds on the screen
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{

    public GameObject[] targets = null;
    public Vector3 offset;
    public float smoothSpeed = 0.1f;
    private Vector3 currentCamVelocity = Vector3.zero;
    void Start()
    {
        if (targets == null)
        {
            targets = GameObject.FindGameObjectsWithTag("Player");
        }
    }

    void FixedUpdate()
    {
        if (targets == null || targets.Length == 0)
        {
            return;
        }
        // transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed);
        // following only the x of the target
        // calculating the middle x position of all the targets
        Vector3 targetsMiddlePosOnlyX = Vector3.zero;
        foreach (GameObject target in targets)
        {
            if (target)
            {
                targetsMiddlePosOnlyX.x += target.transform.position.x;
            }
        }
        targetsMiddlePosOnlyX /= targets.Length;
        // follow to the camera desired position
        transform.position = Vector3.SmoothDamp(transform.position, targetsMiddlePosOnlyX + offset, ref currentCamVelocity, smoothSpeed);
    }
}
