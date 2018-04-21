using UnityEngine;

/// <summary>
/// A simple class to destroy an object after specific time
/// </summary>
public class TimedDestroy : MonoBehaviour
{

    public float lifeTime = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
