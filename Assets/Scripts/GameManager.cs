using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// IMPORTANT NOTES:
// INSPIRATION GOT FROM THIS PROJECT: 
// https://youtu.be/aeWmdojEJf0
// https://github.com/ssusnic/Machine-Learning-Flappy-Bird

/// <summary>
/// Helper class for holding two values in an object
/// </summary>
/// <typeparam name="T">First value in the tuple</typeparam>
/// <typeparam name="K">Second value in the tuple</typeparam>
public class Tuple<T, K>
{
    public T val1;
    public K val2;

    public Tuple(T val1, K val2)
    {
        this.val1 = val1;
        this.val2 = val2;
    }
}

/// <summary>
/// A class to represent the game manager
/// Should have only 1 at any given time, it controls some game features
/// </summary>
public class GameManager : MonoBehaviour
{
    // singleton of the game manager
    public static GameManager instance;

    public int amountOfBirdsAlive;

    private List<Tuple<float, BirdDNA>> choosedBirdsDNAPool;



    public BirdEvolutionAI birdEvolutionAIToSpawn;

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
            amountOfBirdsAlive = 0;
            choosedBirdsDNAPool = new List<Tuple<float, BirdDNA>>(2);
            // loading the next level because we start the game manager in the pre-loader level
            LoadNextLevel();

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// We call this function when we want to start a round,
    /// it reset the birds counter, also it restarts the level
    /// </summary>
    public void Round()
    {
        // RESTART LEVEL MEANING WE GET A NEW SET OF BIRDS
        RestartLevel();
        // reset the amount of birds alive
        instance.amountOfBirdsAlive = 0;
    }
    /// <summary>
    /// This function loads the next level from all of the possible scenes
    /// </summary>
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    /// <summary>
    /// This function restarts the current level
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void IncCurrentlyAliveBirdsCounter()
    {
        amountOfBirdsAlive++;
    }
    public void DecCurrentlyAliveBirdsCounterAndCheckForEndRound()
    {
        amountOfBirdsAlive--;
        if (amountOfBirdsAlive == 0)
        {
            Round();
        }
    }
    public void RemoveAIBirdAndCalculateTheNewBestBirdsDna(BirdEvolutionAI birdEvolutionAIToRemove)
    {

        // INSERTING THE BIRDS DNA TO THE CHOOSEN DNA'S ARRAY IF IT IS THE BEST BIRD VIA FITNESS FUNCTION
        // now looping the best birds array and comparing to check if we need to switch with some bird
        if (choosedBirdsDNAPool.Count < 2)
        {
            choosedBirdsDNAPool.Add(new Tuple<float, BirdDNA>(birdEvolutionAIToRemove.Fitness(), birdEvolutionAIToRemove.Dna));
        }
        else
        {
            for (int bestBirdIndex = 0; bestBirdIndex < 2; bestBirdIndex++)
            {
                if (birdEvolutionAIToRemove.Fitness() > choosedBirdsDNAPool[bestBirdIndex].val1)
                {
                    choosedBirdsDNAPool[bestBirdIndex] = new Tuple<float, BirdDNA>(birdEvolutionAIToRemove.Fitness(), birdEvolutionAIToRemove.Dna);
                    break;
                }
            }
        }

    }

    /// <summary>
    /// A Simple DNA Generator based on the best DNA's that were before it
    /// </summary>
    /// <returns>A new Bird's DNA to use, at first will be null, then will generate better ones</returns>
    public BirdDNA GetANewBirdDNA()
    {
        // MAIN LEARNING IS HERE AT GENERATING A BETTER BIRD'S DNA EACH GENERATION
        BirdDNA birdDNAToReturn = null;
        if (choosedBirdsDNAPool.Count < 2)
        {
            birdDNAToReturn = null;
        }
        else
        {
            // cross over the two dna's and loading the new bird's dna
            birdDNAToReturn = BirdDNA.CrossOver(choosedBirdsDNAPool[0].val2, choosedBirdsDNAPool[1].val2);
            // mutating the new dna a bit
            birdDNAToReturn.Mutate(GameConstants.defaultMutationChance);
        }
        return birdDNAToReturn;
    }

    public void SaveBestBirdsDNAs()
    {
        // TODO IMPLEMENT THIS, USE THE GET BIASES AND WEIGHTS FUNCTIONS
        System.Console.WriteLine("Not implemented yet.");
    }

    // Game manager utils function - mostly for UX/UI

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void FastForwardGame()
    {
        // maybe could be heavy on CPU/GPU, because more calculations
        Time.timeScale = 2;
    }

    public void ReallyFastForwardGame()
    {
        // maybe could be really heavy on CPU/GPU, because more calculations
        Time.timeScale = 3;
    }

    public void LoadLevel(string levelName)
    {
        // when we load a level we should reset some values
        ScoreManager.ResetScores();
        BirdEvolutionAI.instances.RemoveRange(0, BirdEvolutionAI.instances.Count);
        CameraFollow.targets.RemoveRange(0, CameraFollow.targets.Count);

        SceneManager.LoadScene(levelName);
    }

}
