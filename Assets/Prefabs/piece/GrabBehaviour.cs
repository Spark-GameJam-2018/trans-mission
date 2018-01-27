using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour {

    Piece selectedPiece;
    float selectedPieceDistance;

    Piece getPiece()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(!Physics.Raycast(ray, out raycastHit)) { return null; }
        if (!raycastHit.rigidbody) { return null; }

        Piece piece = raycastHit.rigidbody.GetComponentInParent<Piece>();
        if (!piece) { return null; }

        return piece;
    }

    //I would not use OnGUI it was deprecated 6 years ago, but this is for now, so I can see the raycast
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), string.Empty);
    }


    void Update () {
        if (Input.GetMouseButtonDown(0) && !selectedPiece)
        {
            selectedPiece = getPiece();
        }
        else if (Input.GetMouseButtonUp(0) && selectedPiece)
        {
            selectedPiece.isBeingManipulated = false;
            selectedPiece = null;
        }

        if (!selectedPiece) { return; }

        selectedPiece.isBeingManipulated = true;
        selectedPieceDistance = (Camera.main.transform.position - selectedPiece.transform.position).magnitude;
        selectedPiece.transform.position = Camera.main.transform.position + Camera.main.transform.forward * selectedPieceDistance;

    }
}
