using UnityEngine;
using System.Collections;

public class Cohesion : Capability
{
    public override Vector3 GetDelta()
    {
        base.GetDelta();

        // calculate the mass center of the visible objects
        Vector3 massCenter = Vector3.zero;
        for (int i = 0; i < hits.Count; i++)
        {
            massCenter += hits[i].transform.position;
        }

        if (hits.Count > 0)
        {
            massCenter /= hits.Count;
        }

        // move towards that mass center
        Vector3 normalizedMovement = Vector3.Normalize(massCenter - currentTransform.position);

        return normalizedMovement;
    }
}