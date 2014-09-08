using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Support : SteeringBehaviour
{
    public static List<int> informedAgents;

    private const float ERROR = 0.05f;
    private Vector3 delta = Vector3.zero;

    protected override Vector3 GetDelta()
    {
        if (hasTarget)
        {
            if (Vector3.Magnitude(currentTransform.position - targetPosition) < ERROR)
            {
                hasTarget = false;
                targetPosition = Vector3.zero;
                delta = Vector3.zero;

                for (int i = 0; i < agent.hits.Count; i++)
                {
                    if (agent.hits[i].gameObject.name == "MainCube")
                    {
                        Debug.Log(GetInstanceID() + " has reached you!");
                        StageManager.instance.AnnounceIntercept();
                    }
                }

                agent.SwitchType((int)Agent.EntityTypes.Security);

            }
            else
            {
                delta = Vector3.Normalize(targetPosition - currentTransform.position);
            }
        }
        else
        {
            delta = Vector3.zero;
        }

        return delta;
    }    
    
    internal void SetDestination(Vector3 position)
    {
        hasTarget = true;

        targetPosition = position;
    }

    private bool hasTarget = false;
    private Vector3 targetPosition = Vector3.zero;
}