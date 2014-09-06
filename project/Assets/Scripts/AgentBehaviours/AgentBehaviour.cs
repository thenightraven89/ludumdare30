using UnityEngine;
using System.Collections;

public class AgentBehaviour : MonoBehaviour
{
    // reference to the capability's current transform
    protected Transform currentTransform;

    // reference of the agent that this capability is parented to
    protected Agent agent;

    [Tooltip("how influential this capability is")]
    [Range(0f, 10f)]
    public float magnitude;

    [Tooltip("how far can this capability act versus the other agents")]
    [Range(0f, 10f)]
    public float actionRadius;

    protected float actionRadius2;

    protected virtual void Awake()
    {
        currentTransform = transform;
        agent = GetComponent<Agent>();
        actionRadius2 = actionRadius * actionRadius;
    }
}