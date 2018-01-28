using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGroupManager : MonoBehaviour {

    public GameObject snapGroupPrefab;
    public GameObject linkSpherePrefab;

    private List<Piece> pieceList = new List<Piece>();
    private List<Link> linkList = new List<Link>();
    public List<SnapGroup> snapGroupList = new List<SnapGroup>();

    private Link targetedLink;
    private GameObject linkSphere;

    private void Start()
    {
        //get all pieces
        pieceList.AddRange(GetComponentsInChildren<Piece>());
        EventManager.StartListening(Piece.LINK_CREATED, LinkCreatedHandler);
        EventManager.StartListening(GrabBehaviour.SNAPPOINT_RAYCASTED_START, SnapPointRaycastedStartHandler);
        EventManager.StartListening(GrabBehaviour.SNAPPOINT_RAYCASTED_STOP, SnapPointRaycastedStopHandler);
        EventManager.StartListening(GrabBehaviour.SNAPPOINT_RAYCASTED_CLICK, SnapPointRaycastedClickHandler);
    }

    private void SnapPointRaycastedClickHandler(object context)
    {
        SnapPoint snapPoint = (SnapPoint)context;
        targetedLink = getLinkBySnapPoint(snapPoint);
        if (targetedLink == null) { return; }

        RemoveLink(targetedLink);
    }

    private void RemoveLink(Link link)
    {
        Debug.Log("RemoveLink");
        linkList.Remove(link);
        ReDoGroups();
    }

    private void SnapPointRaycastedStopHandler(object context)
    {
        if (linkSphere){ GameObject.Destroy(linkSphere); }
        targetedLink = null;
    }

    private void SnapPointRaycastedStartHandler(object context)
    {
        SnapPoint snapPoint = (SnapPoint)context;
        targetedLink = getLinkBySnapPoint(snapPoint);
        if (targetedLink == null) { return; }

        //instantiate the link sphere for visual guide
        if (!linkSphere)
        {
            linkSphere = Instantiate(linkSpherePrefab);
            linkSphere.transform.position = snapPoint.transform.position;
        }
    }

    private void LinkCreatedHandler(object context)
    {
        Link link = (Link)context;
        linkList.Add(link);

        //move target
        link.target.ParentPiece.GetTargetTransform().position += link.origin.transform.position - link.target.transform.position;
        ReDoGroups();
    }

    private void ReDoGroups()
    {
        //orphan all pieces :(
        foreach(Piece piece in pieceList)
        {
            piece.transform.parent = transform;
        }

        //recreate groups based on links
        foreach (Link link in linkList)
        {
            MergeGroupsOfPieces(link.origin.ParentPiece, link.target.ParentPiece);
        }

        //remove all empty groups
        snapGroupList.RemoveAll(snapGroup => {
            if(!snapGroup.IsEmpty) { return false; }
            GameObject.Destroy(snapGroup.gameObject);
            return true;
        });
    }

    private void MergeGroupsOfPieces(Piece a, Piece b)
    {
        //create the new group
        SnapGroup snapGroup = CreateSnapGroup();

        //lets make a list of pieces and it's related (connected) pieces;
        List<Piece> pieces = new List<Piece>();
        pieces.AddRange(GetPiecesInSameGroup(a));
        pieces.AddRange(GetPiecesInSameGroup(b));

        //move pieces into the new and shiny group :O
        foreach (Piece piece in pieces)
        {
            piece.transform.parent = snapGroup.transform;
        }
    }

    private void DestroySnapGroup(SnapGroup snapGroup)
    {
        if (!snapGroup) { return; }
        snapGroupList.Remove(snapGroup);
        GameObject.Destroy(snapGroup.gameObject);
    }

    private List<Piece> GetPiecesInSameGroup(Piece piece)
    {
        
        if (piece.SnapGroup)
        {
            return piece.SnapGroup.Pieces;
        }
        List<Piece> pieces = new List<Piece>();
        pieces.Add(piece);
        return pieces;
    }

    private SnapGroup CreateSnapGroup()
    {
        GameObject snapGroupGameObject = Instantiate(snapGroupPrefab);
        SnapGroup snapGroup = snapGroupGameObject.GetComponent<SnapGroup>();
        snapGroup.transform.parent = transform;
        snapGroupList.Add(snapGroup);
        return snapGroup;
    }

    private Link getLinkBySnapPoint(SnapPoint snapPoint)
    {
        foreach(Link link in linkList)
        {
            if(link.origin == snapPoint || link.target == snapPoint)
            {
                return link;
            }
        }
        return null;
    }

    private void Update()
    {
        //Debug.Log(linkList.Count);
    }
}
