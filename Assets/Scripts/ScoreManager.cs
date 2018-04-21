using UnityEngine;

/// <summary>
/// This Class manages and tracks the scores of the whole game
/// </summary>
public static class ScoreManager
{

    private static int playerWins = 0;
    private static int AIWins = 0;

    public static void AddPointToPlayer()
    {
        playerWins++;
    }

    public static void AddPointToAI()
    {
        AIWins++;
    }

    public static void ResetScores()
    {
        playerWins = 0;
        AIWins = 0;
    }

    public static int GetPlayerWins()
    {
        return playerWins;
    }
    public static int GetAIWins()
    {
        return AIWins;
    }
}
