using UnityEngine;
using System.Collections;

public class Velocity : MonoBehaviour
{
    public Vector3 velocity;
        
    void FixedUpdate()
    {
        transform.Translate(velocity * Time.fixedDeltaTime);
    }
}
