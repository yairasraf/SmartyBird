using UnityEngine;

[RequireComponent(typeof(Bird))]
public class BirdPlayer : MonoBehaviour
{
    private Bird bird;
    // Use this for initialization
    void Start()
    {
        bird = GetComponent<Bird>();
    }

    // Update is called once per frame
    void Update()
    {
        // checking whether we pressed jump, cross-platform input
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    bird.Jump();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                bird.Jump();
            }
        }
    }
}
