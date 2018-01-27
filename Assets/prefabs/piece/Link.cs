using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link {
    //I now, getters and settters right?, since we dont have lombok in c#
    //(and is not possible to implement in c#), I dont want to autogenerate getters
    //and setters because it generates a lot of boilerplate code, longer than this comment
    //I can still talk about my life and those getters and setter still will be longer
    public SnapPoint origin;
    public SnapPoint target;

    public Link(SnapPoint origin, SnapPoint target)
    {
        this.origin = origin;
        this.target = target;
    }

    public float Distance()
    {
        return (origin.transform.position - target.transform.position).magnitude;
    }

    public override string ToString()
    {
        return base.ToString() + ": origin=" + origin + ", target=" + target;
    }
}
