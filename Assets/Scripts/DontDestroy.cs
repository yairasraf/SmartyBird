using UnityEngine;

/// <summary>
/// A simple script to not destroy an object at the loading of another scene
/// </summary>
public class DontDestroy : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
