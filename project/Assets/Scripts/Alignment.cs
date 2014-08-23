using UnityEngine;
using System.Collections;

public class Alignment : Capability
{
    public override Vector3 GetDelta()
    {
        base.GetDelta();

        Vector3 collectiveVelocity = Vector3.zero;

        for (int i = 0; i < hits.Count; i++)
        {
            collectiveVelocity += hits[i].GetComponent<Velocity>().CurrentVelocity;
        }

        if (hits.Count > 0)
        {
            collectiveVelocity /= hits.Count;
        }

        return collectiveVelocity - GetComponent<Velocity>().CurrentVelocity;
    }
}