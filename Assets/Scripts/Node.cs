using System.Collections.Generic;


// defining the activation function delegate
public delegate double ActivationFunction(double val);

[System.Serializable]
public class Node
{
    private double value; // current value that the Node holds
    private double bias; // bias of the Node

    private List<Edge> inputEdges; // all edges connected to the Node
    private List<Edge> outputEdges; // all edges going out from the Node

    // an activation is per Node, for each node there is an activation, also for each layer there is probably the same activation
    private ActivationFunction activation;

    // empty constructor, default activation is ReLU
    public Node() : this(new ActivationFunction(Utils.ReLU)) { }

    // constructor with a specific activation fucntion
    public Node(ActivationFunction activation)
    {
        // creating the node variables
        this.value = 0;
        this.bias = Utils.randomGenerator.NextDouble();
        // creating the lists for the edges
        this.inputEdges = new List<Edge>();
        this.outputEdges = new List<Edge>();
        // setting the activation function of the node
        this.activation = activation;
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
        double biasedNodeValue = value + bias;
        // double activationOfBiasedNodeValue = Utils.Sigmoid(biasedNodeValue);
        double activationOfBiasedNodeValue = activation(biasedNodeValue);
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


}