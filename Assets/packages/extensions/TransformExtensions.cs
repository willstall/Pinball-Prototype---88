using UnityEngine;
using System.Collections;


public static class TransformExtensions  {

    public static Bounds GetBoundsWithChildren( this Transform transform )
    {
        Bounds newBounds = new Bounds();
        foreach (Transform child in transform)
        {
            newBounds.Encapsulate(child.position);
        }
        return newBounds;
    }
}
