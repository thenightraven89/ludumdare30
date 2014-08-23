using UnityEngine;
using System.Collections;

public class Capability : MonoBehaviour
{
    protected Transform currentTransform;
    protected Sight sight;
    protected Collider[] hits;

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