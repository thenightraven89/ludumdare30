using UnityEngine;
using System.Collections;

public class VelocityController : Capability
{
    private Vector3 velocity;

    protected override void Awake()
    {
        base.Awake();
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalVelocity = Input.GetAxis("Horizontal");
        float verticalVelocity = Input.GetAxis("Vertical");
                
        ElasticCamera.instance.UpdatePosition();
            
        currentTransform.Translate((Vector3.right * horizontalVelocity + Vector3.forward * verticalVelocity) * magnitude * Time.fixedDeltaTime);
    }
}
