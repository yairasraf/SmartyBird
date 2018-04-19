using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bird))]
public class BirdEvolutionAI : MonoBehaviour
{
    private Bird bird;
    private BirdDNA dna;
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

        GameManager.instance.AddAIBird(this);
    }

    void Update()
    {

        // TOOD IMPORTANT - ADD A DIFFERENT INPUT, PROBABLY SOMETHING HERE THAT WILL MAKE THE BIRD SORT OF SEE THE TERRAIN, THE WORLD
        // TODO MAYBE - USE RAY CASTING INSTEAD BECAUSE IT IS MAYBE MORE REALISTIC
        // Ray2D ray;
        //RaycastHit2D hit;
        //// now we need to get the nearest wall position in ordre to put it as data to the neural network

        //hit = Physics2D.Raycast(transform.position, Vector2.right, 1000, wallsLayerMask);

        //// if we did not hit anything with the raycast
        //if (!hit.transform)
        //{
        //    return;
        //}

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
        List<double> predictionData = new List<double>(2);
        predictionData.Add(distanceFromBirdToNearWall);
        predictionData.Add(heightFromBirdToNearWallEntrance);


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

    public void KillEvolutionAIBird()
    {
        // we are getting the score in remove AI bird
        GameManager.instance.RemoveAIBird(this);
        // only then killing it
        bird.Kill();
    }

}
