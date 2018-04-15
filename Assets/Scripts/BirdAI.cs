using UnityEngine;

[RequireComponent(typeof(Bird))]
public class BirdAI : MonoBehaviour
{
    private Bird bird;
    public NeuralNetwork neuralNet;
    public LayerMask wallsLayerMask;

    void Start()
    {
        bird = GetComponent<Bird>();

        neuralNet = new NeuralNetwork(GameConstants.defaultLearningRate);
        neuralNet.AddDenseLayer(2, new ActivationFunction(Utils.ReLU));
        neuralNet.AddDenseLayer(6, new ActivationFunction(Utils.ReLU));
        neuralNet.AddDenseLayer(1, new ActivationFunction(Utils.Sigmoid));


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
        if (!upperWallObject)
        {
            // we are here if there is no wall near us
            return;
        }
        double distanceFromBirdToNearWall = upperWallObject.position.x - transform.position.x;
        // now we need the height difference between the y coordinate of the bird and the y coordinate of the entrance of the wall
        double heightScaleOfHittedWall = upperWallObject.root.localScale.y;
        // using some hard-coded values, probably better to change this later
        double topOfTheEntrance = Mathf.Abs((float)(((8 * heightScaleOfHittedWall) - 8 * (2 - heightScaleOfHittedWall)) / 2));
        // hard-coded value of 2.0f, probably better to change this later
        double heightFromBirdToNearWallEntrance = transform.position.y - (topOfTheEntrance - 2.0);

        print("Passed to neural network: (" + distanceFromBirdToNearWall + "," + heightFromBirdToNearWallEntrance + ")");
        // passing the coordinates to the neural network
        double neuralNetPrediction = neuralNet.Predict(distanceFromBirdToNearWall, heightFromBirdToNearWallEntrance)[0];

        print(neuralNetPrediction);
        if (neuralNetPrediction > 0.5)
        {
            bird.Jump();
        }
    }

    public float Fitness()
    {
        // a simple fitness function
        return transform.position.x;
    }

}
