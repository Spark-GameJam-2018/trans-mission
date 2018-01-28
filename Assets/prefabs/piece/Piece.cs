using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public static string LINK_CREATED = "LINK_CREATED";

    public Collider[] snapPoints;
    private bool isBeingManipulated;

    private Link potentialLink;
    private GameObject lineGameObject;
    public Material snapLineMaterial;

    // Linked objects graph container.
    // Graph of linked pieces share the same linkedObjectParent.
    private PieceGraph pieceGraph;
    // Node representing this piece in the graph.
    private GraphNode<Piece> node;

    private SnapGroup snapGroup;

    public SnapGroup SnapGroup
    {
        get
        {
            return GetComponentInParent<SnapGroup>();
        }
    }

    void Start ()
    {
        //pieceGraph = GetComponentInParent<PieceGraph>();
        //RegisterNode();
        snapPoints = GetComponentsInChildren<Collider>();

        EventManager.StartListening(GrabBehaviour.STOP_GRABBING, StopGrabbingPieceHandler);
	}

    public Transform GetTargetTransform()
    {
        if (SnapGroup)
        {
            return SnapGroup.transform;
        }
        return transform;
    }

    public void addPotentialLink(SnapPoint origin, SnapPoint target)
    {
        if (isBeingManipulated) return;
        //Debug.Log("Piece(" + this + "): Adding link to:" + target.ParentPiece);
        potentialLink = new Link(origin, target);
        //Debug.Log("addPotentialSnapPoint:" + potentialLink);
    }

    public void removePotentialLink()
    {
        if (isBeingManipulated) return;
        //Debug.Log("Piece(" + this + "): Removing link:" + potentialLink);
        potentialLink = null;
        //Debug.Log("removePotentialSnapPoint:" + potentialLink);
    }

    public void StartManipulation()
    {
        if (!isBeingManipulated)
        {
            isBeingManipulated = true;
            //ResetNode();
        }
    }

    //internal void TransformConnectedNodes(Vector3 newLocation)
    //{
    //    HashSet<Piece> connectedPieces = this.pieceGraph.GetConnectedNodes(this);
    //    connectedPieces.Remove(this);
    //    foreach (var piece in connectedPieces)
    //    {
    //        piece.transform.parent = this.transform;
    //    }
    //    this.transform.position = newLocation;
    //    foreach (var piece in connectedPieces)
    //    {
    //        piece.transform.parent = this.transform.parent;
    //    }
    //}

    //internal GraphNode<Piece> GetNode()
    //{
    //    return this.node;
    //}

    public void StopManipulation()
    {
        if (isBeingManipulated)
        {
            isBeingManipulated = false;
        }
    }

    private void StopGrabbingPieceHandler(object context)
    {
        if (potentialLink == null) { return; }

        //send event to create the link
        EventManager.TriggerEvent(LINK_CREATED, potentialLink);

        //Piece targetPiece = potentialLink.target.ParentPiece;
        //at this point we have our child piece that needs to be snapped
        //I will do this in a tree structure in order to use Unity's gameobject
        //hierarchy

        //targetPiece.transform.parent = potentialLink.origin.transform;
        //targetPiece.transform.position += potentialLink.origin.transform.position - potentialLink.target.transform.position;

        //// Add the edge to the graph
        //Debug.Log("Adding edge for node in graph...");
        //this.pieceGraph.AddUndirectedEdge(this.node, targetPiece.node, 0);

        potentialLink = null;
    }

    void DrawLine(Vector3 start, Vector3 end)
    {
        if (lineGameObject) { RemoveLine(); }
        lineGameObject = new GameObject();
        lineGameObject.transform.position = start;
        LineRenderer lr = lineGameObject.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("k014/SnapLineShader"));
        lr.receiveShadows = false;
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }

    private void RemoveLine()
    {
        GameObject.Destroy(lineGameObject);
    }

    public void Rotate(float x, float y, float z)
    {
        transform.Rotate(x, y, z);
    }

    private void Update()
    {
        if (potentialLink != null)
        {
            DrawLine(potentialLink.origin.transform.position, potentialLink.target.transform.position);
        }
        else
        {
            RemoveLine();
        }
    }

    ///**
    // * Creates and registers this node in the graph of pieces.
    // */
    //private void RegisterNode()
    //{
    //    this.node = new GraphNode<Piece>(this);
    //    this.pieceGraph.AddNode(this.node);
    //}

    ///**
    // * Removes node from graph and adds it back.
    // * I know it's not optimal... I'm lazy.
    // */
    //private void ResetNode()
    //{
    //    Debug.Log("Resetting node in graph...");
    //    this.pieceGraph.Remove(this);
    //    this.node = new GraphNode<Piece>(this);
    //    this.pieceGraph.AddNode(this.node);
    //}
}
