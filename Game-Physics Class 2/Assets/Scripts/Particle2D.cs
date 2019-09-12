using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;

    private float startTime;

    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;
    public dropDownPhysics phystype;
    public Oscillation circtype;

    public enum dropDownPhysics
    {
        Kinematic,
        Euler
    }

    public enum Oscillation
    {
        Oscillate,
        None
    }

    void updatePositionEulerExplicit(float deltaTime)
    {
        position += velocity * deltaTime;

        velocity += acceleration * deltaTime;
    }

    void updatePositionKinematic(float deltaTime)
    {
        position += velocity * deltaTime + acceleration * deltaTime * deltaTime * 0.5f;

        velocity += acceleration * deltaTime;
    }

    void updateRotationEulerExplicit(float deltaTime)
    {
        rotation += angularVelocity * deltaTime;

        angularVelocity += angularAcceleration * deltaTime;
    }

    void updateRotationKinematic(float deltaTime)
    {
        rotation += angularVelocity * deltaTime + angularAcceleration * deltaTime * deltaTime * 0.5f;

        angularVelocity += angularAcceleration * deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (phystype == dropDownPhysics.Euler)
        {
            updatePositionEulerExplicit(Time.fixedDeltaTime);

            updateRotationEulerExplicit(Time.fixedDeltaTime);
        }

        if (phystype == dropDownPhysics.Kinematic)
        {
            updatePositionKinematic(Time.fixedDeltaTime);

            updateRotationKinematic(Time.fixedDeltaTime);
        }

        if(circtype == Oscillation.Oscillate)
        {
            //Save time when object instantiated
            //subtract that time from current time
            //do sin of created time onto x and y pos
            acceleration.x = -Mathf.Sin(Time.time - startTime);
            acceleration.y = Mathf.Cos(Time.time - startTime);
        }

        transform.position = position;

        transform.eulerAngles = new Vector3(0f,0f,rotation);

        
    }
}
