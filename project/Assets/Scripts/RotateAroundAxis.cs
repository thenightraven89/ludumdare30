using UnityEngine;
using System.Collections;

public class RotateAroundAxis : MonoBehaviour
{
    public Vector3 axis;

    public float speed;

    private Transform currentTransform;

    void Awake()
    {
        currentTransform = transform;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        currentTransform.Rotate(axis, speed * Time.fixedDeltaTime);
    }
}
