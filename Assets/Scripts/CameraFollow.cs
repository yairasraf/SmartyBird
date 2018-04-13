using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.1f;
    private Vector3 currentCamVelocity = Vector3.zero;
    void Start()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        // transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothSpeed);
        // following only the x of the target
        Vector3 targetPosOnlyX = target.position;
        targetPosOnlyX.y = 0;
        targetPosOnlyX.z = 0;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosOnlyX + offset, ref currentCamVelocity, smoothSpeed);
    }
}
