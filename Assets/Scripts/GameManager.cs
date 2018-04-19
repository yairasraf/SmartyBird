using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    private List<BirdEvolutionAI> aiBirdsPool;

    private List<BirdEvolutionAI> choosedAIBirdsPool;

    private List<float> finalFitnessOfBirdsOrderer;


    public BirdEvolutionAI birdEvolutionAIToSpawn;

    private bool allBirdDead = false;

    // Use this for initialization
    void Start()
    {
        if (instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
            aiBirdsPool = new List<BirdEvolutionAI>(10);
            choosedAIBirdsPool = new List<BirdEvolutionAI>(4);
            finalFitnessOfBirdsOrderer = new List<float>(10);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// We call this function when we want to start a round,
    /// after each round we should compute the AI and improve it and this function does this as well.
    /// also it restarts the level
    /// </summary>
    public void Round()
    {
        // TODO ADD CALLING TO THE ALGORITHM ABOUT IMPROVING EACH BIRD AI HERE
        RestartLevel();
    }
    /// <summary>
    /// This function restarts the current level
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void AddAIBird(BirdEvolutionAI birdEvolutionAIToAdd)
    {
        aiBirdsPool.Add(birdEvolutionAIToAdd);
    }
    public void RemoveAIBird(BirdEvolutionAI birdEvolutionAIToAdd)
    {
        finalFitnessOfBirdsOrderer.Add(birdEvolutionAIToAdd.Fitness());
        aiBirdsPool.Remove(birdEvolutionAIToAdd);
        // TODO ADD SAVING FITNESS HERE THEN SELECTING, MUTATING, CROSS OVER
        if (aiBirdsPool.Count == 0)
        {
            allBirdDead = true;
            Round();
        }
    }

}
