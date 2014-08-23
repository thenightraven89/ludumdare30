using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Velocity : Capability
{
    public Vector3 defaultVelocity;

    private List<Capability> capabilities;

    protected override void Awake()
    {
        base.Awake();
        capabilities = new List<Capability>(GetComponents<Capability>());
    }
        
    void FixedUpdate()
    {
        sight.Process();

        Vector3 capabilitiesVelocity = Vector3.zero;
        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < capabilities.Count; i++)
        {
            capabilitiesVelocity += capabilities[i].GetDelta();
        }

        velocity = capabilitiesVelocity + defaultVelocity;
        //velocity.Normalize();

        currentTransform.Translate(velocity * Time.fixedDeltaTime);
    }
}