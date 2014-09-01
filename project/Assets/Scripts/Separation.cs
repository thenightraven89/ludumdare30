using UnityEngine;
using System.Collections;

public class Separation : Capability
{
    protected override Vector3 GetDelta()
    {
        Vector3 normalizedMovement = Vector3.zero;

        // calculate the average run-away vector
        for (int i = 0; i < agent.hits.Count; i++)
        {
            Vector3 distance = agent.hits[i].transform.position - currentTransform.position;

            if (Vector3.SqrMagnitude(distance) < actionRadius2)
            {
                normalizedMovement -= distance;
            }
        }

        return Vector3.Normalize(normalizedMovement);
    }
}