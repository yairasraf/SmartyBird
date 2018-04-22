using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Bird))]
public class BirdPlayer : MonoBehaviour
{
    public static BirdPlayer singelton = null;
    private Bird bird;
    public Text scoreText;
    public GameObject whoWinnerPanelToEnable;
    public Text whoWinnerText;

    // Use this for initialization
    void Start()
    {
        // setting a new object to the singleton everytime we get a new object
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
        scoreText.text = "Distance: " + Mathf.Round(this.bird.Score());

        // handeling user input
        // checking whether we pressed jump, cross-platform input
        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // Setting the time scale to 1 just in case we start with a pause and we are sleeping meaning really at the start of a scene
                    if (bird.rigid.IsSleeping())
                    {
                        GameManager.instance.ResumeGame();
                    }
                    // actually jumping
                    bird.Jump();

                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                // Setting the time scale to 1 just in case we start with a pause and we are sleeping meaning really at the start of a scene
                if (bird.rigid.IsSleeping())
                {
                    GameManager.instance.ResumeGame();
                }
                // actually jumping
                bird.Jump();
            }
        }
    }

}
