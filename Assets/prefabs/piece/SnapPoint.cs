using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour {

    private Piece parentPiece;

    public Piece ParentPiece
    {
        get
        {
            return parentPiece;
        }

        set
        {
            parentPiece = value;
        }
    }

    private void Start()
    {
        parentPiece = GetComponentInParent<Piece>();
    }

    private void OnTriggerEnter(Collider other)
    {
        SnapPoint snapPoint = getSnapPointInCollision(other);
        if(!snapPoint || snapPoint.ParentPiece == parentPiece) { return; }
        ParentPiece.addPotentialLink(this, snapPoint);
    }

    void OnTriggerExit(Collider other)
    {
        SnapPoint snapPoint = getSnapPointInCollision(other);
        if (!snapPoint || snapPoint.ParentPiece == parentPiece) { return; }
        ParentPiece.removePotentialLink();
    }

    private SnapPoint getSnapPointInCollision(Collider other)
    {
        SnapPoint snapPoint = other.GetComponent<SnapPoint>();
        if (snapPoint)
        {
            return snapPoint;
        }
        return null;
    }
}
