using UnityEngine;

[RequireComponent(typeof(Bird))]
public class BirdAI : MonoBehaviour
{
    private Bird bird;
    public NeuralNetwork neuralNet;

    void Start()
    {
        bird = GetComponent<Bird>();

        neuralNet = new NeuralNetwork();
        neuralNet.AddDenseLayer(2, new ActivationFunction(Utils.ReLU));
        neuralNet.AddDenseLayer(6, new ActivationFunction(Utils.ReLU));
        neuralNet.AddDenseLayer(1, new ActivationFunction(Utils.Sigmoid));


    }

    void Update()
    {

        // TOOD IMPORTANT - ADD A DIFFERENT INPUT, PROBABLY SOMETHING HERE THAT WILL MAKE THE BIRD SORT OF SEE THE TERRAIN, THE WORLD
        double neuralNetPrediction = neuralNet.Predict(transform.position.x, transform.position.y)[0];
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
