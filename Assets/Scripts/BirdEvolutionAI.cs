using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bird))]
public class BirdEvolutionAI : MonoBehaviour
{
    // A list to hold all of the Evo birds instances
    public static List<BirdEvolutionAI> instances = new List<BirdEvolutionAI>();

    private Bird bird;
    public float ceilingDistance = 10;
    public Vector2 rayStartPosDirDown;
    public LayerMask wallsLayer;

    private BirdDNA dna;
    public BirdDNA Dna
    {
        get
        {
            return dna;
        }
        set
        {
            // This setter makes the dna value also update the neural network values everytime it is set
            dna = value;
            LoadNewBirdDNA(this.dna);
        }
    }

    public NeuralNetwork neuralNet;
    private float eachWallSpriteHeight = 3.3f;

    private Vector2 lastSeenGapPosition;

    private Transform upperWallObject;
    private Transform floorWallObject;

    void Start()
    {
        bird = GetComponent<Bird>();

        neuralNet = new NeuralNetwork(GameConstants.defaultLearningRate);
        neuralNet.AddDenseLayer(2, new ActivationFunction(Utils.ReLU));
        neuralNet.AddDenseLayer(6, new ActivationFunction(Utils.ReLU));
        neuralNet.AddDenseLayer(1, new ActivationFunction(Utils.Sigmoid));
        neuralNet.FinishBuilding(Utils.LinearError);


        this.dna = GameManager.instance.GetANewBirdDNA();
        // if we dont have a valid DNA from the DNA Generator
        if (dna == null)
        {
            this.dna = new BirdDNA(neuralNet.GetWeights(), neuralNet.GetBiases());
        }
        else
        {
            // we are here if we DID got a new DNA from the DNA Generator so we should actually apply it to the neural network
            this.neuralNet.SetWeights(this.dna.weightsOfNeuralNetwork);
            this.neuralNet.SetBiases(this.dna.biasesOfNeuralNetwork);
        }

        // we are adding this Evo bird to the static array that holds all Evo birds
        instances.Add(this);
    }

    void Update()
    {

        // if the game is paused we should NOT do the calculations
        if (Time.timeScale == 0)
        {
            return;
        }

        // at the start of the game we dont see that walls so we should assign them via the game world generator
        if (!(upperWallObject && floorWallObject))
        {
            upperWallObject = GameWorldGenerator.instance.oldestSpawnedUpperWall;
            floorWallObject = GameWorldGenerator.instance.oldestSpawnedFloorWall;
        }
        else
        {
            // checking for walls near this bird
            // raycast to check for walls in front of the bird
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayStartPosDirDown + (transform.position.x * Vector2.right), Vector2.down, ceilingDistance, wallsLayer);
            if (hits.Length == 2)
            {
                // now we pass the coordinates of what wall we hit
                upperWallObject = hits[0].transform.root;
                floorWallObject = hits[1].transform.root;
            }
            else
            {
                // if we dont hit anything we keep the walls objects as they are without changing them
            }
        }
        if (!(upperWallObject && floorWallObject))
        {
            // we are here if there is no wall near us
            // we return, meaning we dont jump
            return;
        }
        double distanceFromBirdToNearWall = upperWallObject.position.x - transform.position.x;
        // now we need the height difference between the y coordinate of the bird and the y coordinate of the entrance of the wall
        double heightScaleOfUpperWall = upperWallObject.localScale.y;
        double heightScaleOfFloorWall = floorWallObject.localScale.y;
        // using some hard-coded values, probably better to change this later
        double topOfTheEntranceY = upperWallObject.position.y - (heightScaleOfUpperWall * eachWallSpriteHeight);
        double bottomOfTheEntranceY = heightScaleOfFloorWall * eachWallSpriteHeight;
        double middleYOfTheEntrance = (topOfTheEntranceY + bottomOfTheEntranceY) / 2;
        double heightFromBirdToNearWallEntrance = transform.position.y - middleYOfTheEntrance;

        lastSeenGapPosition.x = (float)distanceFromBirdToNearWall;
        lastSeenGapPosition.y = (float)heightFromBirdToNearWallEntrance;
        // print("Passed to neural network the closest wall pos is: (" + upperWallObject.position.x + "," + middleYOfTheEntrance + ")");

        // print("Passed to neural network: (" + distanceFromBirdToNearWall + "," + heightFromBirdToNearWallEntrance + ")");
        // print("Passed to neural network the closest wall pos is: (" + upperWallObject.position.x + "," + middleOfTheEntrance + ")");






        // passing the coordinates to the neural network
        List<double> predictionData = new List<double>(2)
        {
            lastSeenGapPosition.x,
            lastSeenGapPosition.y
        };
        // print("Passed to neural network: " + lastSeenGapPosition);

        // trying to predict by itself, here we see it learned something and if the current Bird's DNA is any good
        double neuralNetPrediction = neuralNet.Predict(predictionData)[0];

        // print(neuralNetPrediction);
        if (neuralNetPrediction > 0.5)
        {
            bird.Jump();
        }
    }

    public float Fitness()
    {
        // a simple fitness function, based on the score of the bird
        return bird.Score();
    }

    // This function recieves a new dna and loads it to bird and its the neural network
    private void LoadNewBirdDNA(BirdDNA newDna)
    {
        this.dna = newDna;
        this.neuralNet.SetWeights(this.dna.weightsOfNeuralNetwork);
        this.neuralNet.SetBiases(this.dna.biasesOfNeuralNetwork);
    }

    // kills an Evolutional AI Bird 
    public void KillEvolutionAIBird()
    {
        // we are getting actually making the game manager here track this Bird for calculations before it is killed
        GameManager.instance.RemoveAIBirdAndCalculateTheNewBestBirdsDna(this);

        // remove this Evo bird from the Evo birds array
        instances.Remove(this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        // transform.position + rayStartPosDirDown.localPosition, Vector2.down, lookDistance, wallsLayer);
        Gizmos.DrawLine((transform.position.x * Vector2.right) + rayStartPosDirDown, (transform.position.x * Vector2.right) + rayStartPosDirDown + (ceilingDistance * Vector2.down));
    }
}
