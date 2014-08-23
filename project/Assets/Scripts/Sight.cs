using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sight : MonoBehaviour
{
    [Range(0, 5)]
    public float radius;

    [HideInInspector]
    public List<Collider> hits;

    private Transform currentTransform;

    private void Awake()
    {
        currentTransform = transform;
    }
    
    public void Process()
    {
        hits = new List<Collider>(Physics.OverlapSphere(currentTransform.position, radius));

        // remmove self from list
        hits.Remove(collider);
    }
}