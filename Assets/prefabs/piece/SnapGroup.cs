using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapGroup : MonoBehaviour {

	public List<Piece> Pieces
    {
        get
        {
            return new List<Piece>(GetComponentsInChildren<Piece>());
        }
    }

    public bool IsEmpty
    {
        get
        {
            return Pieces.Count == 0;
        }
    }
}
