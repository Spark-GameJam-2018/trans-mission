using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public Collider[] snapPoints;
    private bool isBeingManipulated;

    private Link potentialLink;
    private GameObject lineGameObject;
    public Material snapLineMaterial;

    void Start ()
    {
        snapPoints = GetComponentsInChildren<Collider>();

        EventManager.StartListening(GrabBehaviour.STOP_GRABBING, StopGrabbingPieceHandler);
	}

    public void addPotentialLink(SnapPoint origin, SnapPoint target)
    {
        if (isBeingManipulated) return;
        Debug.Log("Piece(" + this + "): Adding link to:" + target.ParentPiece);
        potentialLink = new Link(origin, target);
        Debug.Log("addPotentialSnapPoint:" + potentialLink);
    }

    public void removePotentialLink()
    {
        if (isBeingManipulated) return;
        Debug.Log("Piece(" + this + "): Removing link:" + potentialLink);
        potentialLink = null;
        Debug.Log("removePotentialSnapPoint:" + potentialLink);
    }

    public void StartManipulation()
    {
        isBeingManipulated = true;
    }

    public void StopManipulation()
    {
        isBeingManipulated = false;
    }

    private void StopGrabbingPieceHandler(object context)
    {
        if (potentialLink == null) { return; }

        Piece targetPiece = potentialLink.target.ParentPiece;
        //at this point we have our child piece that needs to be snapped
        //I will do this in a tree structure in order to use Unity's gameobject
        //hierarchy
        //        targetPiece.transform.parent = potentialLink.origin.transform;
        //        targetPiece.transform.rotation = Quaternion.Inverse(potentialLink.origin.transform.rotation);
        //        targetPiece.transform.position += potentialLink.origin.transform.position - potentialLink.target.transform.position;

        Debug.Log("Liam: " + potentialLink.origin.transform);
        targetPiece.transform.parent = potentialLink.origin.transform;
        targetPiece.transform.position += potentialLink.origin.transform.position - potentialLink.target.transform.position;

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
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
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
}
