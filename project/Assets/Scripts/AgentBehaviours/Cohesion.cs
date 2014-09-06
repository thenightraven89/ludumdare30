using UnityEngine;
using System.Collections;

public class Cohesion : SteeringBehaviour
{
    protected override Vector3 GetDelta()
    {
        // calculate the mass center of the visible objects
        Vector3 massCenter = Vector3.zero;
        for (int i = 0; i < agent.hits.Count; i++)
        {
            Agent otherAgent = agent.hits[i].GetComponent<Agent>();

            if (otherAgent != null)
            {
                massCenter += otherAgent.transform.position;
            }
        }

        // average the result
        if (agent.hits.Count > 0)
        {
            massCenter /= agent.hits.Count;
        }

        // return the normalized direction of moving towards the averaged mass point
        return massCenter - currentTransform.position;
    }
}