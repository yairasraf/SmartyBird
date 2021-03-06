﻿/// <summary>
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
        // some random values
        this.Weight = Utils.randomGenerator.NextDouble();
    }

}