using UnityEngine;
using System.Collections;

public class VelocityController : Capability
{
    private Vector3 velocity;

    public Hacking hacking;
    private const float VELOCITY_CANCELHACK = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalVelocity = Input.GetAxis("Horizontal");
        float verticalVelocity = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalVelocity) > Mathf.Abs(verticalVelocity))
        {
            currentTransform.Translate(Vector3.right * horizontalVelocity * magnitude * Time.deltaTime);
        }
        else
        {
            currentTransform.Translate(Vector3.forward * verticalVelocity * magnitude * Time.deltaTime);
        }

        if (Mathf.Abs(horizontalVelocity) + Mathf.Abs(verticalVelocity) > VELOCITY_CANCELHACK)
        {
            hacking.Cancel();
        }
                
        ElasticCamera.instance.UpdatePosition();
            
        //currentTransform.Translate((Vector3.right * horizontalVelocity + Vector3.forward * verticalVelocity) * magnitude * Time.fixedDeltaTime);

        if (Input.GetButtonDown("Hack"))
        {
            hacking.Activate();
        }

        if (Input.GetButtonUp("Hack"))
        {
            hacking.Cancel();
        }
    }
}
