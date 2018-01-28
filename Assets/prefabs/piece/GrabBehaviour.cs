using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBehaviour : MonoBehaviour {

    public static string STOP_GRABBING = "STOP_GRABBING";
    public static string SNAPPOINT_RAYCASTED_START = "SNAPPOINT_RAYCASTED_START";
    public static string SNAPPOINT_RAYCASTED_STOP = "SNAPPOINT_RAYCASTED_STOP";
    public static string SNAPPOINT_RAYCASTED_CLICK = "SNAPPOINT_RAYCASTED_CLICK";

    Piece selectedPiece;
    float selectedDistance;
    float hitDistance;
    Vector3 raycastHitPoint;
    Vector3 initialTargetPosition;
    Boolean lookingAtSnapPointMessageSent;
    Boolean isMovingPieces;

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

    SnapPoint getSnapPointByRaycast()
    {
        RaycastHit raycastHit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (!Physics.Raycast(ray, out raycastHit)) { return null; }
        if (!raycastHit.rigidbody) { return null; }

        SnapPoint snapPoint = raycastHit.rigidbody.GetComponentInParent<SnapPoint>();
        if (!snapPoint) { return null; }

        return snapPoint;
    }

    //I would not use OnGUI it was deprecated 6 years ago, but this is for now, so I can see the raycast
    void OnGUI()
    {
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), string.Empty);
    }


    void Update () {

        SnapPoint snapPointRaycasted = getSnapPointByRaycast();
        if (!isMovingPieces && snapPointRaycasted)
        {
            if (!lookingAtSnapPointMessageSent)
            {
                EventManager.TriggerEvent(SNAPPOINT_RAYCASTED_START, snapPointRaycasted);
                lookingAtSnapPointMessageSent = true;
            }

            //well, if we are looking at a snappoint
            //and the player clicks, it means he wants to
            //remove at the selected link
            if (Input.GetMouseButtonDown(0))
            {
                EventManager.TriggerEvent(SNAPPOINT_RAYCASTED_CLICK, snapPointRaycasted);
                return;
            }
        }
        else
        {
            if (lookingAtSnapPointMessageSent)
            {
                lookingAtSnapPointMessageSent = false;
                EventManager.TriggerEvent(SNAPPOINT_RAYCASTED_STOP, null);
            }
        }

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
        isMovingPieces = true;
        selectedPiece.StartManipulation();
        Vector3 difference = (Camera.main.transform.position + Camera.main.transform.forward * selectedDistance) - raycastHitPoint;
        selectedPiece.GetTargetTransform().position = initialTargetPosition + difference;
        UnityEditor.Selection.activeGameObject = selectedPiece.GetTargetTransform().gameObject;
    }

    private void CheckSelectPiece()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedPiece = getPiece();
            if (!selectedPiece) { return; }
            initialTargetPosition = selectedPiece.GetTargetTransform().position;
            selectedDistance = (Camera.main.transform.position - raycastHitPoint).magnitude;
        }
    }

    private void CheckDeselectPiece()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isMovingPieces = false;
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
