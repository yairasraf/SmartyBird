using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bird))]
public class BirdEvolutionAI : MonoBehaviour
{
    public static List<BirdEvolutionAI> instances = new List<BirdEvolutionAI>();
    private Bird bird;
    //public BirdDNA Dna
    //{
    //    get
    //    {
    //        return new BirdDNA(neuralNet.GetWeights(), neuralNet.GetBiases());
    //    }
    //}
    public BirdDNA dna;
    public NeuralNetwork neuralNet;
    private float eachWallSpriteHeight = 8;

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
        //if (this.Dna == null)
        //{
        //    this.dna = new BirdDNA(neuralNet.GetWeights(), neuralNet.GetBiases());
        //}
        //else
        //{
        //    print(dna.weightsOfNeuralNetwork);
        //    neuralNet.SetWeights(dna.weightsOfNeuralNetwork);
        //    neuralNet.SetBiases(dna.biasesOfNeuralNetwork);
        //    //neuralNet.SetWeights(0, dna.weightsOfNeuralNetwork[0]);
        //    //neuralNet.SetBiases(0, dna.biasesOfNeuralNetwork[0]);

        //    //neuralNet.SetWeights(1, dna.weightsOfNeuralNetwork[1]);
        //    //neuralNet.SetBiases(1, dna.biasesOfNeuralNetwork[1]);

        //    //neuralNet.SetWeights(2, dna.weightsOfNeuralNetwork[2]);
        //    //neuralNet.SetBiases(2, dna.biasesOfNeuralNetwork[2]);
        //}
        GameManager.instance.AddBird();
        instances.Add(this);
    }

    void Update()
    {

        // checking collisions
        // TODO ADD A BETTER COLLISION CHECK
        Rigidbody2D rigid = bird.GetRigid();
        if (rigid.position.y > bird.maxYBoundry || rigid.position.y < bird.minYBoundry)
        {
            KillEvolutionAIBird();
        }

        // now we pass the coordinates of what wall we hit
        //double distanceFromBirdToNearWall = hit.transform.position.x - transform.position.x;
        Transform upperWallObject = GameWorldGenerator.instance.oldestSpawnedUpperWall;
        Transform floorWallObject = GameWorldGenerator.instance.oldestSpawnedFloorWall;
        if (!(upperWallObject && floorWallObject))
        {
            // we are here if there is no wall near us
            return;
        }
        double distanceFromBirdToNearWall = upperWallObject.position.x - transform.position.x;
        // now we need the height difference between the y coordinate of the bird and the y coordinate of the entrance of the wall
        double heightScaleOfUpperWall = upperWallObject.root.localScale.y;
        double heightScaleOfFloorWall = floorWallObject.root.localScale.y;
        // using some hard-coded values, probably better to change this later
        double topOfTheEntranceY = upperWallObject.root.position.y - (heightScaleOfUpperWall * eachWallSpriteHeight);
        double bottomOfTheEntranceY = heightScaleOfFloorWall * eachWallSpriteHeight;
        double middleOfTheEntrance = (topOfTheEntranceY + bottomOfTheEntranceY) / 2;
        // hard-coded value of 2.0f, probably better to change this later
        double heightFromBirdToNearWallEntrance = transform.position.y - middleOfTheEntrance;

        // print("Passed to neural network: (" + distanceFromBirdToNearWall + "," + heightFromBirdToNearWallEntrance + ")");
        // print("Passed to neural network the closest wall pos is: (" + upperWallObject.position.x + "," + middleOfTheEntrance + ")");

        // passing the coordinates to the neural network

        // building the data set
        List<double> predictionData = new List<double>(2)
        {
            distanceFromBirdToNearWall,
            heightFromBirdToNearWallEntrance
        };


        // teaching the model what he should do from the player playing

        // trying to predict by itself, here we see it learned something
        double neuralNetPrediction = neuralNet.Predict(predictionData)[0];

        print(neuralNetPrediction);
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


    public void LoadNewBirdDNA(BirdDNA newDna)
    {
        this.dna = newDna;
    }
    // kills a bird and returns its fitness
    public void KillEvolutionAIBird()
    {
        // we are getting the score in remove AI bird
        GameManager.instance.RemoveAIBird(this);
        // only then killing it
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // basically losing
        KillEvolutionAIBird();
    }
}
