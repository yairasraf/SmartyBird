using UnityEngine;

public class PlayerScoreUIText : MonoBehaviour
{

    public UnityEngine.UI.Text playerScoreText;

    void Start()
    {

    }

    void Update()
    {
        playerScoreText.text = "Player Wins: " + ScoreManager.GetPlayerWins();
    }
}
