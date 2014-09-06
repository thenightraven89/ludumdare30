using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    [Tooltip("fizzy particle system")]
    public ParticleSystem particles;

    [Tooltip("tween agent color")]
    public TweenMaterialColor colorTweener;

    [Tooltip("tween fizzy particle system color")]
    public TweenMaterialColor fizzTweener;

    [Tooltip("how far will the agent scan to apply capabilities")]
    [Range(0, 5)]
    public float sightRadius;

    [Tooltip("cap velocity to this value")]
    [Range(1, 10)]
    public float maxVelocity;

    // stored list of all hit colliders in specified sight radius
    [HideInInspector]
    public List<Collider> hits;

    // stored agent transform component
    protected Transform currentTransform;

    // the agent's type
    protected EntityTypes entityType;

    // the agent's color
    protected Color mainColor;

    // the memory amount stored in the entity
    protected int memory;

    // stored list of this agent's capabilities
    protected List<Capability> capabilities;

    // area on which agent is able to move
    protected Rect movementArea;

    // computed velocity of the agent (from capabilities + default)
    protected Vector3 velocity;

    protected virtual void Awake()
    {
        currentTransform = transform;

        // fetch list of this agent's capabilities
        capabilities = new List<Capability>(GetComponents<Capability>());

        // set movement area to initial spawning area
        movementArea = Spawner.instance.spawnArea;
    }   

    protected virtual void Update()
    {
        // scan the horizon
        Scan();
    }

    /// <summary>
    /// update sight
    /// </summary>
    public void Scan()
    {
        hits = new List<Collider>(Physics.OverlapSphere(currentTransform.position, sightRadius));

        // remmove self from list
        hits.Remove(collider);
    }

    /// <summary>
    /// Returns current velocity outside agent class
    /// </summary>
    public Vector3 CurrentVelocity { get { return velocity; } }

    public EntityTypes EntityType
    {
        get { return entityType; }
    }

    public enum EntityTypes
    {
        Citizen = 0,
        Security = 1,
        Friendly = 2,
        Corrupted = 3
    }
}