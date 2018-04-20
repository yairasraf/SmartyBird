/// <summary>
/// This class represents an Edge,Link,Line,Arc in a Undirected Graph
/// </summary>

[System.Serializable]
public class Edge
{
    public Node Previous { get; set; }
    public Node Next { get; set; }
    public double Weight { get; set; }

    public Edge(Node previous, Node next)
    {
        this.Previous = previous;
        this.Next = next;
        // SMALL random values
        this.Weight = (0.001 - 0.0001) * Utils.randomGenerator.NextDouble() + 0.0001;
        //this.Weight = Utils.randomGenerator.NextDouble();

    }

}