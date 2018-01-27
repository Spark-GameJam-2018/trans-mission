using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour {

    public Piece parentPiece;

    private void OnTriggerEnter(Collider other)
    {
        Piece piece = getPieceInCollision(other);
        parentPiece.addPotentialPiece(piece);
    }

    void OnTriggerExit(Collider other)
    {
        Piece piece = getPieceInCollision(other);
        parentPiece.removePotentialPiece(piece);
    }

    private Piece getPieceInCollision(Collider other)
    {
        Piece piece = other.GetComponentInParent<Piece>();
        if (piece)
        {
            return piece;
        }
        return null;
    }
}
