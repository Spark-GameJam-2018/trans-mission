using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraph<T> {

    void AddNode(GraphNode<T> node);

    void AddNode(T value);

    void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost);

    void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost);

    bool Contains(T value);

    bool Remove(T value);
}
