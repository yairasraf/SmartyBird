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
            choosedAIBirdsPool = new List<BirdEvolutionAI>(2);
            finalFitnessOfBirdsOrderer = new List<float>(10);
            LoadNextLevel();
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

        // choosing 4 best birds via fitness function
        AddChosenBirdsToArrayAnRemoveAllFromOriginalArray();

        // RESTART LEVEL FOR NOW
        RestartLevel();

        // instantiate new better birds
        InstantiateAndImproveAndMutateAndCrossOverBirds();
    }
    /// <summary>
    /// This function loads the next level from all of the possible scenes
    /// </summary>
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
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
            Round();
        }
    }

    private void AddChosenBirdsToArrayAnRemoveAllFromOriginalArray()
    {
        // DONT FORGET THAT WE HAVE ONLY FITNESSES OF BIRD IN THE ARRAY OF FITNESSES
        // finalFitnessOfBirdsOrderer
        // adding 2 first birds
        for (int i = 0; i < 2; i++)
        {
            choosedAIBirdsPool[i] = aiBirdsPool[i];
            aiBirdsPool.RemoveAt(i);
        }
        // now looping to check if we have bird bigger than some other bird
        for (int curBirdIndex = 0; curBirdIndex < aiBirdsPool.Count; curBirdIndex++)
        {
            BirdEvolutionAI curBird = aiBirdsPool[curBirdIndex];
            // now looping the best birds array and comparing to check if we need to switch with some bird
            for (int bestBirdIndex = 0; bestBirdIndex < choosedAIBirdsPool.Count; bestBirdIndex++)
            {
                if (curBird.Fitness() > choosedAIBirdsPool[bestBirdIndex].Fitness())
                {
                    choosedAIBirdsPool[bestBirdIndex] = curBird;
                }
            }
        }

        // now empty the not choosen birds array
        aiBirdsPool.RemoveRange(0, aiBirdsPool.Count);
    }
    private void InstantiateAndImproveAndMutateAndCrossOverBirds()
    {
        // TODO ADD MUTATING HERE
        // assuming we have 2 birds in the best birds array
        print("reached mutating");
        for (int i = 0; i < 10; i++)
        {
            BirdEvolutionAI newBird = Instantiate(birdEvolutionAIToSpawn, birdEvolutionAIToSpawn.transform.position, birdEvolutionAIToSpawn.transform.rotation);
            // cross over the two dna's
            newBird.dna = BirdDNA.CrossOver(choosedAIBirdsPool[0].dna, choosedAIBirdsPool[1].dna);
            // mutating the new dna a bit
            newBird.dna.Mutate(GameConstants.defaultMutationChance);
            // newBird.dna = choosedAIBirdsPool;
        }
    }

}
