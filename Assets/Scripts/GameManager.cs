using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



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


public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    // private List<BirdDNA> aiBirdsPool;
    private int amountOfBirdsAlive;

    private List<Tuple<float, BirdDNA>> choosedBirdsDNAPool;

    // private List<float> finalFitnessOfBirdsOrderer;


    public BirdEvolutionAI birdEvolutionAIToSpawn;

    //private void Awake()
    //{

    //}
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
            // aiBirdsPool = new List<BirdDNA>(10);
            amountOfBirdsAlive = 0;
            choosedBirdsDNAPool = new List<Tuple<float, BirdDNA>>(2);
            // finalFitnessOfBirdsOrderer = new List<float>(10);

            // InstantiateAndImproveAndMutateAndCrossOverBirds();
            LoadNextLevel();

        }
    }

    // Update is called once per frame
    void Update()
    {
        print(amountOfBirdsAlive);
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
        // AddChosenBirdsToArrayAnRemoveAllFromOriginalArray();

        // RESTART LEVEL FOR NOW
        RestartLevel();
        // reset the amount of birds alive
        instance.amountOfBirdsAlive = 0;
        // instantiate new better birds
        // InstantiateAndImproveAndMutateAndCrossOverBirds();
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
    public void AddBird()
    {
        // aiBirdsPool.Add(birdEvolutionAIDNAToAdd);
        amountOfBirdsAlive++;
    }
    public void RemoveAIBird(BirdEvolutionAI birdEvolutionAIToRemove)
    {
        // finalFitnessOfBirdsOrderer.Add(birdEvolutionAIToAdd.Fitness());

        // INSERTING THE BIRDS DNA IF IT IS THE BEST BIRD VIA FITNESS FUNCTION
        // now looping the best birds array and comparing to check if we need to switch with some bird
        if (choosedBirdsDNAPool.Count < 2)
        {
            choosedBirdsDNAPool.Add(new Tuple<float, BirdDNA>(birdEvolutionAIToRemove.Fitness(), birdEvolutionAIToRemove.dna));
        }
        else
        {
            for (int bestBirdIndex = 0; bestBirdIndex < 2; bestBirdIndex++)
            {
                if (birdEvolutionAIToRemove.Fitness() > choosedBirdsDNAPool[bestBirdIndex].val1)
                {
                    choosedBirdsDNAPool[bestBirdIndex] = new Tuple<float, BirdDNA>(birdEvolutionAIToRemove.Fitness(), birdEvolutionAIToRemove.dna);
                    break;
                }
            }
        }



        // InsertBirdDNAToChoosenBirdsDNAArray(birdEvolutionAIToRemove);
        // aiBirdsPool.Remove(birdEvolutionAIToRemove.dna);
        amountOfBirdsAlive--;
        // TODO ADD SAVING FITNESS HERE THEN SELECTING, MUTATING, CROSS OVER
        // starting a new round if all birds are dead
        // if (aiBirdsPool.Count == 0)
        if (amountOfBirdsAlive == 0)
        {
            Round();
        }
    }

    public void RemovePlayerBird()
    {
        // if (aiBirdsPool.Count == 0)
        amountOfBirdsAlive--;
        if (amountOfBirdsAlive == 0)
        {
            Round();
        }
    }
    //private void InsertBirdDNAToChoosenBirdsDNAArray(BirdDNA birdAIToMaybeInsert)
    //{

    //}
    //private void AddChosenBirdsToArrayAnRemoveAllFromOriginalArray()
    //{
    //    // DONT FORGET THAT WE HAVE ONLY FITNESSES OF BIRD IN THE ARRAY OF FITNESSES
    //    // finalFitnessOfBirdsOrderer
    //    // adding 2 first birds
    //    for (int i = 0; i < 2; i++)
    //    {
    //        choosedBirdsDNAPool[i] = aiBirdsPool[i];
    //        aiBirdsPool.RemoveAt(i);
    //    }
    //    // now looping to check if we have bird bigger than some other bird
    //    for (int curBirdIndex = 0; curBirdIndex < aiBirdsPool.Count; curBirdIndex++)
    //    {
    //        BirdEvolutionAI curBird = aiBirdsPool[curBirdIndex];
    //        // now looping the best birds array and comparing to check if we need to switch with some bird
    //        for (int bestBirdIndex = 0; bestBirdIndex < choosedBirdsDNAPool.Count; bestBirdIndex++)
    //        {
    //            if (curBird.Fitness() > choosedBirdsDNAPool[bestBirdIndex].Fitness())
    //            {
    //                choosedBirdsDNAPool[bestBirdIndex] = curBird;
    //            }
    //        }
    //    }

    //    // now empty the not choosen birds array
    //    aiBirdsPool.RemoveRange(0, aiBirdsPool.Count);
    //}

    /// <summary>
    /// A Simple DNA Generator based on the best DNA's that were before it
    /// </summary>
    /// <returns></returns>
    public BirdDNA GetANewBirdDNA()
    {
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
    //private void InstantiateAndImproveAndMutateAndCrossOverBirds()
    //{
    //    // TODO ADD MUTATING HERE
    //    // assuming we have 2 birds in the best birds array
    //    print("reached mutating");
    //    for (int i = 0; i < 10; i++)
    //    {
    //        BirdEvolutionAI newBird = Instantiate(birdEvolutionAIToSpawn, birdEvolutionAIToSpawn.transform.position, birdEvolutionAIToSpawn.transform.rotation);
    //        if (this.choosedBirdsDNAPool.Count != 0)
    //        {
    //            // cross over the two dna's and loading the new bird's dna
    //            newBird.LoadNewBirdDNA(BirdDNA.CrossOver(choosedBirdsDNAPool[0].val2, choosedBirdsDNAPool[1].val2));
    //            // mutating the new dna a bit
    //            newBird.dna.Mutate(GameConstants.defaultMutationChance);
    //            // newBird.dna = choosedBirdsDNAPool;
    //        }
    //    }
    //}

}
