using UnityEngine;

public class AIScoreUIText : MonoBehaviour
{
    public UnityEngine.UI.Text aiScoreText;

    void Start()
    {

    }

    void Update()
    {
        aiScoreText.text = "AI Wins: " + ScoreManager.GetAIWins();
    }
}
