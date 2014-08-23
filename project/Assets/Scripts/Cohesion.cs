using UnityEngine;
using System.Collections;

public class Cohesion : MonoBehaviour
{
    [Range(0, 2)]
    public float radius;

    private Transform currentTransform;

    void Awake()
    {
        currentTransform = transform;
    }

    void FixedUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(currentTransform.position, radius);

        // calculate the mass center of the visible objects
        Vector3 massCenter = Vector3.zero;
        for (int i = 0; i < hits.Length; i++)
        {
            massCenter += hits[i].transform.position;
        }
        massCenter /= hits.Length;

        // move towards that mass center
        Vector3 normalizedMovement = Vector3.Normalize(currentTransform.position - massCenter);

        currentTransform.Translate(normalizedMovement);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}