using UnityEngine;

public class MoveConstantSpeed : MonoBehaviour
{

    public float speed = 1;

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

        transform.position += Vector3.right * speed;
    }
}
