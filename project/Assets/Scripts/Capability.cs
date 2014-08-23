using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Capability : MonoBehaviour
{
    protected Transform currentTransform;
    protected Sight sight;
    protected List<Collider> hits;

    [Range(0f, 5f)]
    public float magnitude;

    protected virtual void Awake()
    {
        currentTransform = transform;
        sight = GetComponent<Sight>();
    }

    public virtual Vector3 GetDelta()
    {
        hits = sight.hits;

        return Vector3.zero;
    }
}