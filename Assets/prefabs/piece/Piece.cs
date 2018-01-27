using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public Collider[] snapPoints;
    private bool isBeingManipulated;

    private Link potentialLink;

    // Use this for initialization
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
        targetPiece.transform.parent = potentialLink.origin.transform;
        targetPiece.transform.localPosition = Vector3.zero;

        potentialLink = null;
    }
}
