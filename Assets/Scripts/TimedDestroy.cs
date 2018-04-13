using UnityEngine;

public class TimedDestroy : MonoBehaviour
{

    public float lifeTime = 1;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
