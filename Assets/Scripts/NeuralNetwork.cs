using System.Collections.Generic;

[System.Serializable]
public class NeuralNetwork
{
    private List<List<Node>> layers;
    // TODO IMPORTANT ADD LEARNING RATE

    public NeuralNetwork()
    {
        layers = new List<List<Node>>();
    }

    public void AddDenseLayer(int amountOfNodes, ActivationFunction layerActivation)
    {
        // creating an unconnected layer
        List<Node> layerToAdd = CreateLayer(amountOfNodes, layerActivation);
        layers.Add(layerToAdd);

        // if this is the first then we dont need to connect it
        if (layers.Count == 1)
        {

        }
        else
        {
            // if we are not the first layer then we should connect it to the previous layer
            // connect the last layer which is the layer that we just added
            ConnectLayerToTheLayerBeforeIt(layers.Count - 1);
        }
    }

    public List<double> Predict(params double[] data)
    {
        // resetting the neural network nodes value
        ResetAllNeuralNetworkNodesValue();
        // checking data dimensions
        List<Node> inputLayer = layers[0];
        if (data.Length != inputLayer.Count)
        {
            throw new DataDimensionsMismatchException();
        }

        // recieves the input data and put it in the first layer
        for (int i = 0; i < data.Length; i++)
        {
            inputLayer[i].Input(data[i]);
        }

        // starting from the first layer, also we already assigned the input data to the first layer
        // does NOT feed forward the last layer because it is the last layer and feed forward to another layer
        for (int layerIndex = 0; layerIndex < layers.Count - 1; layerIndex++)
        {
            FeedForwardToNextLayer(layerIndex);
        }

        // now we assume we have result in the nodes of the last layer
        List<double> resultOfOutputLayer = new List<double>();
        // Activating all neurons and adding the results to a list
        List<Node> outputLayer = layers[layers.Count - 1];
        foreach (Node outputNode in outputLayer)
        {
            resultOfOutputLayer.Add(outputNode.Activate());
        }
        // returning the results list

        return resultOfOutputLayer;
    }

    // recieves a layer index and feed forward each node of that layer
    private void FeedForwardToNextLayer(int layerIndex)
    {
        foreach (Node node in layers[layerIndex])
        {
            node.FeedForwardToNextLayer();
        }
    }

    // TODO IMPORTANT ADD SAVING AND LOADING OF THE NEURAL NETWORK
    // TODO IMPORTANT WE NEED TO SAVE THE WEIGHTS AND BIASES AND MAYBE THE MODEL ARCHITECTURE IN ORDER TO LOAD IT, PREFER MAYBE TO NOT SAVE MODEL FROM SECURITY PERSPECTIVE
    public void SaveNeuralNetworkData(string filepath)
    {
        // TODO IMPLEMENT THIS
        System.Console.WriteLine("Not implemented yet.");
    }
    public NeuralNetwork LoadNeuralNetworkData(string filepath)
    {
        // TODO IMPLEMENT THIS
        System.Console.WriteLine("Not implemented yet.");
        return null;
    }

    // helper function for creating a nodes layer not connected to anything
    private List<Node> CreateLayer(int amountOfNodes, ActivationFunction layerActivation)
    {
        List<Node> layerCreated = new List<Node>(amountOfNodes);
        for (int i = 0; i < amountOfNodes; i++)
        {
            Node nodeToAddToLayer = new Node(layerActivation);
            layerCreated.Add(nodeToAddToLayer);
        }
        return layerCreated;
    }

    // Helper function - This function recieves a layer index and connect that layer to the previous layer before
    private void ConnectLayerToTheLayerBeforeIt(int layerIndex)
    {
        // we look at the previous layer which is at layer index - 1
        List<Node> previousLayer = layers[layerIndex - 1];
        List<Node> nextLayer = layers[layerIndex];
        foreach (Node nodeToConnect in previousLayer)
        {
            foreach (Node nodeToBeConnectedTo in nextLayer)
            {

                nodeToConnect.ConnectToNode(nodeToBeConnectedTo);
            }
        }
    }

    // Helper function - This function resets the values of all of the nodes of the neural network
    private void ResetAllNeuralNetworkNodesValue()
    {
        foreach (List<Node> layer in layers)
        {
            foreach (Node neuralNetworkNode in layer)
            {
                neuralNetworkNode.ResetValue();
            }
        }
    }
    // GRADIENT DESCENT ALGORITHM
    // MAYBE TRY:
    // weight += learningRate * error
    // bias += learningRate * error
}
