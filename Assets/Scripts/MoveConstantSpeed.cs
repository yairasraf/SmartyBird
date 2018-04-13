using UnityEngine;

public class MoveConstantSpeed : MonoBehaviour
{

    public float speed = 1;

    void Start()
    {

    }

    void FixedUpdate()
    {
        transform.position += Vector3.right * speed;
    }
}
