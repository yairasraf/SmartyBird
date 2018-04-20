using System.Collections.Generic;

// defining the loss function delegate
public delegate double LossFunction(double errorDelta);

[System.Serializable]
public class NeuralNetwork
{
    private List<List<Node>> layers;
    // TODO IMPORTANT ADD LEARNING RATE
    private double learningRate;
    private LossFunction lossFunction;


    public NeuralNetwork(double learningRate)
    {
        this.layers = new List<List<Node>>();
        this.learningRate = learningRate;
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

    public void FinishBuilding(LossFunction lossFunction)
    {
        this.lossFunction = lossFunction;
    }
    public List<double> Predict(List<double> data)
    {
        // resetting the neural network nodes value
        ResetAllNeuralNetworkNodesValue();
        // checking data dimensions
        List<Node> inputLayer = layers[0];
        if (data.Count != inputLayer.Count)
        {
            throw new DataDimensionsMismatchException();
        }

        // recieves the input data and put it in the first layer
        for (int i = 0; i < data.Count; i++)
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

    // TODO MAYBE - CHANGE THE NAME OF THE FUNCTION TO FIT, BUT MAYBE NOT
    public void Learn(List<double> data, List<double> neuralNetworkOutputValuesExpected)
    {
        // SUPER IMPORTANT - WE ARE ONLY TEACHING THE LAST LAYER FOR NOW FOR PROTOTYPING!!!!! IMPROVE AND CHANGE THIS
        // TODO FIX THIS LEARN FUNCTION, ALSO FIX BACK PROPAGATION


        // Validating input data dimensions
        if (neuralNetworkOutputValuesExpected.Count != layers[layers.Count - 1].Count)
        {
            throw new DataDimensionsMismatchException();
        }

        // TODO MAYBE DONT COPY THE ARRAY, BUT MAYBE YES BECAUSE WE DONT WANT TO CHANGE ITS VALUES, CHECK ABOUT THIS
        // DONT FORGET THAT THIS PREDICT RESETS THAT NEURAL NETWORK VALUES WHICH WE WANT FIRST
        Predict(data);
        // now we have an updated neural network with all activated values in place
        // now we need to update the weights
        List<double> layerValuesExpected = new List<double>(neuralNetworkOutputValuesExpected);

        for (int currentOutputNodeIndex = 0; currentOutputNodeIndex < layers[layers.Count - 1].Count; currentOutputNodeIndex++)
        {
            Node currentOutputNodeToBackPropagate = layers[layers.Count - 1][currentOutputNodeIndex];
            currentOutputNodeToBackPropagate.BackPropagate(layerValuesExpected[currentOutputNodeIndex]);
        }

        // THIS IS UNUSED, THIS IS FOR LOOPING AND BACKPROPAGATING EACH LAYER SEPERATLY
        //// looping each layer and getting its output values and output values expected and backpropagating it
        //for (int layerIndex = layers.Count - 1; layerIndex >= 1; layerIndex--)
        //{
        //    BackPropagateToPreviousLayer(layerIndex, outputValuesExpected);
        //    // getting the new UPDATED values expected of the previous layer
        //    layerValuesExpected=
        //}

        // THIS IS FOR BACKPROPAGATING LAST LAYER AND LETTING THE NODES BACKPROPAGATE BY THEMSELVES
        // BackPropagateToPreviousLayer(layerIndex, outputValuesExpected);

    }

    // recieves a layer index and back propagate it each node of that previous layer
    private void BackPropagateToPreviousLayer(int layerIndex, List<double> valuesExpected)
    {
        // Validating input data dimensions
        if (valuesExpected.Count != layers[layerIndex].Count)
        {
            throw new DataDimensionsMismatchException();
        }
        // TODO MAYBE - DO SOMETHING WITH THIS VARIABLE
        //double totalSumOfErrors = 0;

        // looping and backpropagating each node of the current layer
        for (int nodeToBackPropagateIndex = 0; nodeToBackPropagateIndex < layers[layerIndex].Count; nodeToBackPropagateIndex++)
        {
            Node nodeToBackPropagate = layers[layerIndex][nodeToBackPropagateIndex];
            nodeToBackPropagate.BackPropagate(valuesExpected[nodeToBackPropagateIndex]);
        }
    }




    // helper function for creating a nodes layer not connected to anything
    private List<Node> CreateLayer(int amountOfNodes, ActivationFunction layerActivation)
    {
        List<Node> layerCreated = new List<Node>(amountOfNodes);
        for (int i = 0; i < amountOfNodes; i++)
        {
            Node nodeToAddToLayer = new Node(layerActivation, learningRate);
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

    public double GetLearningRate()
    {
        return this.learningRate;
    }


    //public List<List<double>> GetLayerWeights(int layerIndex)
    //{
    //    List<List<double>> weightsToReturn = new List<List<double>>(layers[layerIndex].Count);

    //     looping and getting each layer's node weights
    //    for (int currentNodeIndex = 0; currentNodeIndex < layers[layerIndex].Count; currentNodeIndex++)
    //    {
    //        Node nodeToGetWeights = layers[layerIndex][currentNodeIndex];
    //        weightsToReturn.Add(nodeToGetWeights.GetWeights());
    //    }

    //    return weightsToReturn;
    //}


    // BIASES

    public void SetLayerBiases(int layerIndex, List<double> layerBiases)
    {
        // Validating data dimensions
        if (layerBiases.Count != layers[layerIndex].Count)
        {
            throw new DataDimensionsMismatchException();
        }
        // looping and changing each layer's node bias
        for (int currentNodeIndex = 0; currentNodeIndex < layers[layerIndex].Count; currentNodeIndex++)
        {
            Node nodeToChangeBias = layers[layerIndex][currentNodeIndex];
            nodeToChangeBias.SetBias(layerBiases[currentNodeIndex]);
        }
    }

    public void SetBiases(List<List<double>> biases)
    {
        // Validating data dimensions
        if (biases.Count != layers.Count)
        {
            throw new DataDimensionsMismatchException();
        }
        // looping and changing each layer's node bias
        for (int currentLayerIndex = 0; currentLayerIndex < layers.Count; currentLayerIndex++)
        {
            this.SetLayerBiases(currentLayerIndex, biases[currentLayerIndex]);
        }
    }


    public List<double> GetLayerBiases(int layerIndex)
    {
        List<double> layerBiasesToReturn = new List<double>(layers[layerIndex].Count);

        // looping and getting each layer's node bias
        for (int currentNodeIndex = 0; currentNodeIndex < layers[layerIndex].Count; currentNodeIndex++)
        {
            Node nodeToGetbias = layers[layerIndex][currentNodeIndex];
            layerBiasesToReturn.Add(nodeToGetbias.GetBias());
        }

        return layerBiasesToReturn;
    }

    public List<List<double>> GetBiases()
    {
        List<List<double>> biasesToReturn = new List<List<double>>(layers.Count);

        // looping and getting each layer's node biases
        for (int currentLayerIndex = 0; currentLayerIndex < layers.Count; currentLayerIndex++)
        {
            biasesToReturn.Add(this.GetLayerBiases(currentLayerIndex));
        }

        return biasesToReturn;
    }




    // WEIGHTS
    public void SetLayerWeights(int layerIndex, List<List<double>> layerWeights)
    {
        // Validating data dimensions
        if (layerWeights.Count != layers[layerIndex].Count)
        {
            throw new DataDimensionsMismatchException();
        }

        // looping and changing each layer's node weights
        for (int currentNodeIndex = 0; currentNodeIndex < layers[layerIndex].Count; currentNodeIndex++)
        {
            Node nodeToChangeWeight = layers[layerIndex][currentNodeIndex];
            nodeToChangeWeight.SetWeightsThatLeadToNextLayer(layerWeights[currentNodeIndex]);
        }
    }

    public void SetWeights(List<List<List<double>>> weights)
    {
        // Validating data dimensions
        if (weights.Count != layers.Count)
        {
            throw new DataDimensionsMismatchException();
        }

        // looping and changing each layer's node weights
        for (int currentLayerIndex = 0; currentLayerIndex < layers.Count; currentLayerIndex++)
        {
            this.SetLayerWeights(currentLayerIndex, weights[currentLayerIndex]);
        }
    }



    public List<double> GetNodeOutputWeights(int layerIndex, int nodeIndex)
    {
        // getting node weights
        return layers[layerIndex][nodeIndex].GetWeights();
    }


    public List<List<double>> GetLayerWeights(int layerIndex)
    {
        List<List<double>> layerWeightsToReturn = new List<List<double>>(layers[layerIndex].Count);
        // looping and getting each node weights
        for (int currentNodeIndex = 0; currentNodeIndex < layers[layerIndex].Count; currentNodeIndex++)
        {
            layerWeightsToReturn.Add(this.GetNodeOutputWeights(layerIndex, currentNodeIndex));
        }

        return layerWeightsToReturn;
    }

    public List<List<List<double>>> GetWeights()
    {
        List<List<List<double>>> weightsToReturn = new List<List<List<double>>>(layers.Count);
        // looping and getting each layer's node weights
        for (int currentLayerIndex = 0; currentLayerIndex < layers.Count; currentLayerIndex++)
        {
            weightsToReturn.Add(this.GetLayerWeights(currentLayerIndex));
        }

        return weightsToReturn;
    }

    // TODO IMPORTANT ADD SAVING AND LOADING OF THE NEURAL NETWORK
    // TODO IMPORTANT WE NEED TO SAVE THE WEIGHTS AND BIASES AND MAYBE THE MODEL ARCHITECTURE IN ORDER TO LOAD IT, PREFER MAYBE TO NOT SAVE MODEL FROM SECURITY PERSPECTIVE
    // TODO MAYBE - ALSO MAYBE SAVE WEIGHTS AND BIASES IN CSV FORMAT FOR IT TO BE BETTER CLEAR AND EASY TO UNDERSTAND AND EDIT IF NEEDED
    public void SaveNeuralNetworkData(string filepath)
    {
        // TODO IMPLEMENT THIS, USE THE GET BIASES AND WEIGHTS FUNCTIONS
        System.Console.WriteLine("Not implemented yet.");
    }

    public NeuralNetwork LoadNeuralNetworkData(string filepath)
    {
        // TODO IMPLEMENT THIS, USE THE GET BIASES AND WEIGHTS FUNCTIONS
        System.Console.WriteLine("Not implemented yet.");
        return null;
    }

    // GRADIENT DESCENT ALGORITHM
    // MAYBE TRY:
    // weight += learningRate * error
    // bias += learningRate * error


}
