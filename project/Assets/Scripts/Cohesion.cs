using UnityEngine;
using System.Collections;

public class Cohesion : Capability
{
    public override Vector3 GetDelta()
    {
        base.GetDelta();

        // calculate the mass center of the visible objects
        Vector3 massCenter = Vector3.zero;
        for (int i = 0; i < hits.Length; i++)
        {
            massCenter += hits[i].transform.position;
        }
        massCenter /= hits.Length;

        // move towards that mass center
        Vector3 normalizedMovement = Vector3.Normalize(massCenter - currentTransform.position);

        return normalizedMovement;
    }
}