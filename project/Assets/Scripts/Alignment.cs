using UnityEngine;
using System.Collections;

public class Alignment : Capability
{
    protected override Vector3 GetDelta()
    {
        Vector3 collectiveVelocity = Vector3.zero;

        for (int i = 0; i < agent.hits.Count; i++)
        {
            Vector3 distance = agent.hits[i].transform.position - currentTransform.position;

            if (distance.sqrMagnitude < actionRadius2)
            {
                Agent otherAgent = agent.hits[i].GetComponent<Agent>();

                // if what we see is an agent
                if (otherAgent != null)
                {
                    // have its velocity influence this one's
                    collectiveVelocity += otherAgent.CurrentVelocity;
                }
            }
        }

        if (agent.hits.Count > 0)
        {
            collectiveVelocity /= agent.hits.Count;
        }

        return collectiveVelocity - agent.CurrentVelocity;
    }
}