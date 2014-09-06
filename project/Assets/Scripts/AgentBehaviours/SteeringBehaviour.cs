using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SteeringBehaviour : AgentBehaviour
{
    protected virtual Vector3 GetDelta()
    {
        return Vector3.zero;
    }

    // fetch the velocity modifier that will be used by the agent
    public Vector3 Delta
    {
        get { return GetDelta() * magnitude; }
    }
}