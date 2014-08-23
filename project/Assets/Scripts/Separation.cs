using UnityEngine;
using System.Collections;

public class Separation : Capability
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

        // calculate the average run-away vector
        Vector3 normalizedMovement = Vector3.zero;
        for (int i = 0; i < hits.Length; i++)
        {
            Vector3 distance = hits[i].transform.position - currentTransform.position;

            if (Vector3.SqrMagnitude(distance) < radius2)
            {
                normalizedMovement -= distance;
            }
        }

        normalizedMovement = Vector3.Normalize(normalizedMovement);

        return normalizedMovement;               
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}