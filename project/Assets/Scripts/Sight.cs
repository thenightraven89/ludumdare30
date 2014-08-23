using UnityEngine;
using System.Collections;

public class Sight : MonoBehaviour
{
    [Range(0, 5)]
    public float radius;

    [HideInInspector]
    public Collider[] hits;

    private Transform currentTransform;

    private void Awake()
    {
        currentTransform = transform;
    }
    
    public void Process()
    {
        hits = Physics.OverlapSphere(currentTransform.position, radius);
    }
}