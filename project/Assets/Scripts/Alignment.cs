using UnityEngine;
using System.Collections;

public class Alignment : Capability
{
    [Range(0, 5)]
    public float radius;

    private float radius2;

    protected override void Awake()
    {
        base.Awake();
        radius2 = radius * radius;
    }

    public override Vector3 GetDelta()
    {
        base.GetDelta();

        Vector3 collectiveVelocity = Vector3.zero;

        for (int i = 0; i < hits.Count; i++)
        {
            Vector3 distance = hits[i].transform.position - currentTransform.position;

            if (distance.sqrMagnitude < radius2)

            collectiveVelocity += hits[i].GetComponent<Velocity>().CurrentVelocity;
        }

        if (hits.Count > 0)
        {
            collectiveVelocity /= hits.Count;
        }

        return collectiveVelocity - GetComponent<Velocity>().CurrentVelocity;
    }
}