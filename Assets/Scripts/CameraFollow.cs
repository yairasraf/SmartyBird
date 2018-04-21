using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is supposed to follow all birds on the screen
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{

    public static List<GameObject> targets = new List<GameObject>();
    public Vector3 offset;
    public float smoothSpeed = 0.1f;
    private Vector3 currentCamVelocity = Vector3.zero;

    void Start()
    {

    }

    void FixedUpdate()
    {

        // if the game is paused we return and dont do anything
        if (Time.timeScale == 0)
        {
            return;
        }

        if (targets == null || targets.Count == 0)
        {
            return;
        }

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
        targetsMiddlePosOnlyX /= targets.Count;
        // follow to the camera desired position
        transform.position = Vector3.SmoothDamp(transform.position, targetsMiddlePosOnlyX + offset, ref currentCamVelocity, smoothSpeed);
    }
}
