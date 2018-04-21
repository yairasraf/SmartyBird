using System.Collections.Generic;


// defining the activation function delegate
public delegate double ActivationFunction(double val, bool derivate);

[System.Serializable]
public class Node
{
    private double value; // current value that the Node holds
    private double bias; // bias of the Node
    private double learningRate; // the learning rate of the Node

    private List<Edge> inputEdges; // all edges connected to the Node
    private List<Edge> outputEdges; // all edges going out from the Node

    // an activation is per Node, for each node there is an activation, also for each layer there is probably the same activation
    private ActivationFunction activation;

    // empty constructor, default activation is ReLU
    public Node(double learningRate) : this(new ActivationFunction(Utils.ReLU), learningRate) { }

    // constructor with a specific activation fucntion
    public Node(ActivationFunction activation, double learningRate)
    {
        // creating the node variables
        this.value = 0;
        // SMALL random values
        this.bias = (0.00001 - 0.000001) * Utils.randomGenerator.NextDouble() + 0.000001;
        // creating the lists for the edges
        this.inputEdges = new List<Edge>();
        this.outputEdges = new List<Edge>();
        // setting the activation function of the node
        this.activation = activation;
        // setting the learning rate of the node
        this.learningRate = learningRate;
    }

    // This function add to the current value of the node
    public void Input(double val)
    {
        this.value += val;
    }

    // This function connect the node to another node using the Edge class
    // not writted unconnect code because unneeded
    public void ConnectToNode(Node nodeToConnectTo)
    {
        // creating the edge connecting the two nodes
        Edge edgeConnectingTheTwoNodes = new Edge(this, nodeToConnectTo);

        // assigning the edge to each node input or output edges
        this.outputEdges.Add(edgeConnectingTheTwoNodes);
        nodeToConnectTo.inputEdges.Add(edgeConnectingTheTwoNodes);
    }

    // This function returns the activation of the node based on its value without changing the value
    public double Activate()
    {
        // WHEN WE ARE HERE WE ASSUME WE ALREADY HAVE VALUE ALL SUMMED UP BY PREVIOUS NODES AND EDGES
        // does NOT change the original value of the node
        // we should return here activationFunc(w0a0 + w1a1 + w2a2 + ... + wnan + b0)
        // reLU function here

        // adding the bias
        double biasedNodeValue = ValueWithBias();
        // double activationOfBiasedNodeValue = Utils.Sigmoid(biasedNodeValue);
        double activationOfBiasedNodeValue = activation(biasedNodeValue, false);
        // double activationOfBiasedNodeValue = Utils.ReLU(biasedNodeValue);
        return activationOfBiasedNodeValue;
    }

    public void FeedForwardToNextLayer()
    {
        double activatedValue = Activate();

        foreach (Edge outputEdge in outputEdges)
        {
            Node nodeOfNextLayer = outputEdge.Next;
            nodeOfNextLayer.Input(activatedValue * outputEdge.Weight);
        }
    }

    // This function resets the current value of the node to 0
    public void ResetValue()
    {
        this.value = 0;
    }

    public double ValueWithBias()
    {
        return this.value + this.bias;
    }
    public void BackPropagate(double valueExpected)
    {
        // THIS IS OLD VERSION
        //// now we back propagate to the input edges to tweak their weights and biases
        //foreach (Edge backEdge in inputEdges)
        //{
        //    // calculating the error and cost and loss and some values
        //    // CORE OF LEARNING IS HERE
        //    double valueCurrent = (backEdge.Weight * this.value) + this.bias;
        //    double error = valueExpected - valueCurrent;
        //    // loss function we define as error squared
        //    // TODO ADD HERE A REFERENCE TO THE LOSS FUNCTION
        //    double loss = error * error;

        //    double weightGradient = -(2) * (this.value * error);
        //    double biasGradient = -(2) * (error);

        //    // this lines are a must
        //    backEdge.Weight = backEdge.Weight - (learningRate * weightGradient);
        //    this.bias = this.bias - (learningRate * biasGradient);

        //    // backEdge.Previous.BackPropagate(
        //}


        // UPDATED VERSION

        // CORE OF LEARNING IS HERE
        // calculating values about this current node
        double valueCurrentWithoutActivation = ValueWithBias();
        double valueCurrentWithActivation = Activate();


        // calculating the sum of the errors
        double outputSumMarginOfError = valueExpected - valueCurrentWithActivation;
        // loss function we define as error squared
        // TODO ADD HERE A REFERENCE TO THE LOSS FUNCTION
        double derivactivationOfCurrentValue = this.activation(valueCurrentWithoutActivation, true);

        // this is the important value, specific to each node
        double deltaOutputSum = derivactivationOfCurrentValue * outputSumMarginOfError;

        // now we back propagate to the input edges to tweak their weights and biases
        foreach (Edge backEdge in inputEdges)
        {
            // calculating the error and cost and loss and some values

            // double loss = error * error;

            // double weightGradient = -(2) * (this.value * error);
            // double biasGradient = -(2) * (error);

            double resOfPreviousNode = backEdge.Previous.Activate();
            double deltaWeight = deltaOutputSum / resOfPreviousNode;

            // these lines for prototyping
            // double oldw = backEdge.Weight;
            // double newW = oldw - deltaWeight;

            // TODO CHECK IF WE REALLY NEED TO MULTIPLY BY LEARNING RATE HERE
            backEdge.Weight = backEdge.Weight + (learningRate * deltaWeight);
            // this lines are maybe important
            // backEdge.Weight = backEdge.Weight - (learningRate * weightGradient);
            // this.bias = this.bias - (learningRate * biasGradient);

            // backEdge.Previous.BackPropagate(
        }

        // now calling backpropagate on the other hidden layers
        // TODO ADD THIS HERE
        foreach (Edge inputEdge in inputEdges)
        {

        }

    }

    public void SetBias(double newBias)
    {
        this.bias = newBias;
    }

    public double GetBias()
    {
        return this.bias;
    }
    public void SetWeightsThatLeadToNextLayer(List<double> weights)
    {
        // validating data dimensions
        if (outputEdges.Count != weights.Count)
        {
            throw new DataDimensionsMismatchException();
        }
        // looping and changing all of the output edges weights
        for (int currentEdgeIndex = 0; currentEdgeIndex < outputEdges.Count; currentEdgeIndex++)
        {
            // getting the connecting edge
            Edge weightThatLeadToNextLayer = outputEdges[currentEdgeIndex];
            // setting its weight
            weightThatLeadToNextLayer.Weight = weights[currentEdgeIndex];
        }
    }
    public List<double> GetWeights()
    {
        List<double> weightsToReturn = new List<double>(outputEdges.Count);

        // looping and adding the output edges weights
        foreach (Edge outputEdgeToGetWeight in outputEdges)
        {
            weightsToReturn.Add(outputEdgeToGetWeight.Weight);
        }

        return weightsToReturn;
    }

}