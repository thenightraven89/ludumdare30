using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Velocity : Capability
{
    public float maxVelocity;

    private List<Capability> capabilities;
    private Rect area;

    protected override void Awake()
    {
        base.Awake();
        capabilities = new List<Capability>(GetComponents<Capability>());
        velocity = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up) * Vector3.forward;
        area = Spawner.instance.spawnArea;
    }

    private Vector3 velocity;
    
    private Vector3 angle;

    void FixedUpdate()
    {
        sight.Process();

        Vector3 capabilitiesVelocity = Vector3.zero;

        for (int i = 0; i < capabilities.Count; i++)
        {
            capabilitiesVelocity += capabilities[i].GetDelta() * capabilities[i].magnitude;
        }

        // translate agent
        velocity += capabilitiesVelocity;

        if (velocity.sqrMagnitude > maxVelocity * maxVelocity)
        {
            velocity = velocity.normalized * maxVelocity;
        }

        Vector3 translateValue = velocity * magnitude * Time.fixedDeltaTime;

        if ((currentTransform.position + translateValue).x < area.xMin ||
            (currentTransform.position + translateValue).x > area.xMax)
        {
            velocity += new Vector3(-2f * velocity.x, 0f, 0f);
        }

        if ((currentTransform.position + translateValue).z < area.yMin ||
            (currentTransform.position + translateValue).z > area.yMax)
        {
            velocity += new Vector3(0f, 0f, -2f * velocity.z);
            //velocity = new Vector3(velocity.x, velocity.y, -1f * velocity.z);
        }

        if (velocity.x * velocity.x > velocity.z * velocity.z)
        {
            velocity = new Vector3(velocity.x, 0f, 0f);
        }
        else
        {
            velocity = new Vector3(0f, 0f, velocity.z);
        }

        currentTransform.Translate(velocity * magnitude * Time.fixedDeltaTime);
    }

    public Vector3 CurrentVelocity { get { return velocity; } }
}