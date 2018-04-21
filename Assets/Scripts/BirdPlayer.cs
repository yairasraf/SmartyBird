using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Bird))]
public class BirdPlayer : MonoBehaviour
{
    public static BirdPlayer singelton = null;
    private Bird bird;
    public Text scoreText;

    // Use this for initialization
    void Start()
    {
        if (singelton)
        {
            Destroy(singelton.gameObject);
            singelton = this;

        }
        else
        {
            singelton = this;
            bird = GetComponent<Bird>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if the game is paused we do not want to return because we want to check for player input, if he jumps we should resume the game

        // updating the score of the player every frame
        scoreText.text = "Fitness: " + Mathf.Round(this.bird.Score());

        // handeling user input
        // checking whether we pressed jump, cross-platform input
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    bird.Jump();
                    // Setting the time scale to 1 just in case we start with a pause
                    GameManager.instance.ResumeGame();

                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                bird.Jump();
                // Setting the time scale to 1 just in case we start with a pause
                GameManager.instance.ResumeGame();
            }

        }
    }

}
