using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGroupManager : MonoBehaviour {
    
    public SnapGroup snapGroupPrefab;

    private List<Piece> pieceList = new List<Piece>();
    private List<Link> linkList = new List<Link>();
    private List<SnapGroup> snapGroupList = new List<SnapGroup>();

    private void Start()
    {
        //get all pieces
        pieceList.AddRange(GetComponentsInChildren<Piece>());
        EventManager.StartListening(Piece.LINK_CREATED, LinkCreatedHandler);
    }

    private void LinkCreatedHandler(object context)
    {
        Link link = (Link)context;
        linkList.Add(link);

        //move target group
        SnapGroup targetSnapGroup = link.target.ParentPiece.SnapGroup;
        if (targetSnapGroup)
        {
            targetSnapGroup.transform.position += link.origin.transform.position - link.target.transform.position;
        }
        //if the target is in no group, just move the piece then
        else
        {
            link.target.ParentPiece.transform.position += link.origin.transform.position - link.target.transform.position;
        }
        reDoGroups();
    }

    private void reDoGroups()
    {
        //unlink all pieces from their groups
        foreach(Piece piece in pieceList)
        {
            piece.transform.parent = transform;
        }

        //remove all groups
        foreach (SnapGroup group in snapGroupList)
        {
            GameObject.Destroy(group.gameObject);
        }

        //recreate groups based on links
        foreach(Link link in linkList)
        {
            //obtain group which each piece belongs to and merge them
            // or create them if no groups are found
            SnapGroup originSnapGroup = link.origin.ParentPiece.SnapGroup;
            SnapGroup targetSnapGroup = link.target.ParentPiece.SnapGroup;

            //if both pieces does not belong to any group, lets create one
            //and put both in that group
            if(originSnapGroup == null && targetSnapGroup == null)
            {
                SnapGroup snapGroup = CreateSnapGroup();
                link.origin.ParentPiece.transform.parent = snapGroup.transform;
                link.target.ParentPiece.transform.parent = snapGroup.transform;
            }
        }
    }

    private SnapGroup CreateSnapGroup()
    {
        SnapGroup snapGroup = Instantiate<SnapGroup>(snapGroupPrefab);
        snapGroup.transform.parent = transform;
        snapGroupList.Add(snapGroup);
        return snapGroup;
    }
}
