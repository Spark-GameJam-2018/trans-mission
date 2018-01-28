using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Adapter for a Graph of Pieces.
 */
public class PieceGraph : MonoBehaviour, IGraph<Piece> {

    public List<Piece> pieces;
    private Graph<Piece> pieceGraph;

    public void AddDirectedEdge(GraphNode<Piece> from, GraphNode<Piece> to, int cost)
    {
        pieceGraph.AddDirectedEdge(from, to, cost);
    }

    public void AddNode(GraphNode<Piece> node)
    {
        pieceGraph.AddNode(node);
    }

    public void AddNode(Piece value)
    {
        pieceGraph.AddNode(value);
    }

    public void AddUndirectedEdge(GraphNode<Piece> from, GraphNode<Piece> to, int cost)
    {
        pieceGraph.AddUndirectedEdge(from, to, cost);
    }

    public bool Contains(Piece value)
    {
        return pieceGraph.Contains(value);
    }

    public bool Remove(Piece value)
    {
        return pieceGraph.Remove(value);
    }

    /**
     * Time for some BFS Bois!!
     */
    public HashSet<Piece> GetConnectedNodes(Piece start)
    {
        var visited = new HashSet<Piece>();

        if (!Contains(start))
            return visited;

        var stack = new Stack<Piece>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            var vertex = stack.Pop();

            if (visited.Contains(vertex))
                continue;

            visited.Add(vertex);

            foreach (var neighbor in vertex.GetNode().Neighbors)
                if (!visited.Contains(neighbor.Value))
                    stack.Push(neighbor.Value);
        }

        return visited;
    }

    // Use this for initialization
    void Start () {
        pieceGraph = new Graph<Piece>();
	}

}
