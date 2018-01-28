using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour {

    public static string STOP_GRABBING = "STOP_GRABBING";

    Piece selectedPiece;
    float selectedDistance;
    float hitDistance;
    Vector3 raycastHitPoint;
    Vector3 initialTargetPosition;

    Piece getPiece()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if(!Physics.Raycast(ray, out raycastHit)) { return null; }
        if (!raycastHit.rigidbody) { return null; }

        Piece piece = raycastHit.rigidbody.GetComponentInParent<Piece>();
        if (!piece) { return null; }

        raycastHitPoint = raycastHit.point;
        hitDistance = (raycastHit.point - Camera.main.transform.position).magnitude;

        return piece;
    }

    //I would not use OnGUI it was deprecated 6 years ago, but this is for now, so I can see the raycast
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), string.Empty);
    }


    void Update () {

        if (!selectedPiece)
        {
            CheckSelectPiece();
        }
        else
        {
            CheckDeselectPiece();
            CheckRotatePiece();
        }

        // If piece is selected then update piece position.
        if (selectedPiece)
        {
            UpdatePiecePosition();
        }
    }

    private void UpdatePiecePosition()
    {
        //if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    selectedPiece.TransformConnectedNodes(Camera.main.transform.position + Camera.main.transform.forward * selectedPieceDistance);
        //}
        //else
        //{
        //    selectedPiece.StartManipulation();
        //    selectedPiece.transform.position = Camera.main.transform.position + Camera.main.transform.forward * selectedPieceDistance;
        //}
        selectedPiece.StartManipulation();
        //GetTargetTransformForPiece(selectedPiece).position = Camera.main.transform.position + Camera.main.transform.forward * selectedDistance;
        Vector3 difference = (Camera.main.transform.position + Camera.main.transform.forward * selectedDistance) - raycastHitPoint;
        GetTargetTransformForPiece(selectedPiece).position = initialTargetPosition + difference;
    }

    private Transform GetTargetTransformForPiece(Piece selectedPiece)
    {
        if (selectedPiece.SnapGroup)
        {
            return selectedPiece.SnapGroup.transform;
        }
        return selectedPiece.transform;
    }

    private void CheckSelectPiece()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedPiece = getPiece();
            if (!selectedPiece) { return; }
            initialTargetPosition = GetTargetTransformForPiece(selectedPiece).position;
            selectedDistance = (Camera.main.transform.position - raycastHitPoint).magnitude;
        }
    }

    private void CheckDeselectPiece()
    {
        if (Input.GetMouseButtonUp(0))
        {
            selectedPiece.StopManipulation();
            selectedPiece = null;
            EventManager.TriggerEvent(STOP_GRABBING, null);
        }
    }
    
    private void CheckRotatePiece()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                selectedPiece.Rotate(0, 90, 0);
            }
            else
            {
                selectedPiece.Rotate(0, -90, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                selectedPiece.Rotate(0, 0, 90);
            }
            else
            {
                selectedPiece.Rotate(0, 0, -90);
            }
        }
    }
}
