using UnityEngine;


public class GamePausedUIText : MonoBehaviour
{

    public UnityEngine.UI.Text startGameText;

    void Start()
    {

        // checking the running platform and showing start text based on that platform
        if (Application.isMobilePlatform)
        {
            startGameText.text = "Touch screen to start";
        }
        else
        {
            startGameText.text = "Press 'Space' to start";
        }
    }

    void Update()
    {
        // This script only does its work when the game is paused
        // we should destroy this object if the game is not paused
        if (Time.timeScale != 0)
        {
            Destroy(this.gameObject);
        }
    }
}
