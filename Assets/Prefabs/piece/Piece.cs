using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    public Collider[] snapPoints;
    public bool isBeingManipulated;

    public List<Piece> potentialChildList;

	// Use this for initialization
	void Start ()
    {
        snapPoints = GetComponentsInChildren<Collider>();
	}

    public void addPotentialPiece(Piece piece)
    {
        if (isBeingManipulated) return;
        Debug.Log("Piece: Adding piece:" + piece);
        potentialChildList.Add(piece);
    }

    public void removePotentialPiece(Piece piece)
    {
        if (isBeingManipulated) return;
        Debug.Log("Piece: Removing piece:" + piece);
        potentialChildList.Remove(piece);
    }
}
