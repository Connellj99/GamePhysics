using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle2D : MonoBehaviour
{
    private const float kGravity = -0.012f;

    private Vector2 surfaceNormalUnit = new Vector2(0.0f, 1.0f);
    private Vector2 f_opposing = new Vector2(1.0f, 0.0f);
    private const float frictionCoefficientStatic = 0.5f;
    private const float frictionCoefficientKinetic = 0.5f;
    private const float springRestingLength = 1.0f;
    private const float springStiffnessCoefficient = 0.5f;
    private Vector2 fluidVelocity = new Vector2(1.0f, 1.0f);
    private const float fluidDensity = 3.0f;
    private const float objectAreaCrossSection = 3.0f;
    private const float objectDragCoefficient = 0.5f;

    //private Vector2 anchorPosition;

    public Vector2 position;
    public Vector2 velocity;
    public Vector2 acceleration;
    public float startTime;

    public float rotation;
    public float angularVelocity;
    public float angularAcceleration;
    public dropDownPhysics phystype;
    public Oscillation circtype;
    public forcegen forcetype;

    [Range(0,Mathf.Infinity)]
    public float mass;

    private float invMass;

   
    private float Mass
    {
        set
        {
            mass = mass > 0.0f ? mass : 0.0f; //checks if mass isnt 0
            invMass = mass > 0.0f ? 1.0f / mass : 0.0f;
        }
        get
        {
            return mass;
        }
    }

    private Vector2 force;


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

    public enum forcegen
    {
        Gravity,
        Normal,
        Sliding,
        Friction_Static,
        Friction_Kinetic,
        Drag,
        Spring,
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

    private void AddForce(Vector2 newForce)
    {
        //forces can just be added
        force += newForce;
    }

    private void UpdateAcceleration()
    {
        //convert force to acceleration
        acceleration = force * invMass;

        force.Set(0.0f,0.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        Mass = mass;

        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(forcetype == forcegen.Gravity)
        {
            AddForce(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up));
        }
        if (forcetype == forcegen.Normal)
        {
            AddForce(ForceGenerator.GenerateForce_Normal(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up), surfaceNormalUnit));
        }
        if (forcetype == forcegen.Sliding)
        {
            AddForce(ForceGenerator.GenerateForce_Sliding(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up), ForceGenerator.GenerateForce_Normal(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up), surfaceNormalUnit)));
        }
        if (forcetype == forcegen.Friction_Static)
        {
            AddForce(ForceGenerator.GenerateForce_Friction_Static(ForceGenerator.GenerateForce_Normal(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up), surfaceNormalUnit), f_opposing, frictionCoefficientStatic));
        }
        if (forcetype == forcegen.Friction_Kinetic)
        {
            AddForce(ForceGenerator.GenerateForce_Friction_Kinetic(ForceGenerator.GenerateForce_Normal(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up), surfaceNormalUnit), velocity, frictionCoefficientKinetic));
        }
        if (forcetype == forcegen.Drag)
        {
            AddForce(ForceGenerator.GenerateForce_Sliding(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up), ForceGenerator.GenerateForce_Normal(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up), surfaceNormalUnit)));
            AddForce(ForceGenerator.GenerateForce_Drag(velocity, fluidVelocity, fluidDensity,objectAreaCrossSection,objectDragCoefficient));

        }
        if (forcetype == forcegen.Spring)
        {
            //AddForce(ForceGenerator.GenerateForce_Spring(position, anchorPosition, springRestingLength,springStiffnessCoefficient));
            AddForce(ForceGenerator.GenerateForce_Gravity(Mass, kGravity, Vector2.up));
            AddForce(ForceGenerator.GenerateForce_Spring(position, -position, springRestingLength, springStiffnessCoefficient));
        }
        if (forcetype == forcegen.None)
        {
            
        }
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


        UpdateAcceleration();

        transform.position = position;

        transform.eulerAngles = new Vector3(0f,0f,rotation);

        if (circtype == Oscillation.Oscillate)
        {
            //Save time when object instantiated
            acceleration.x = -Mathf.Sin(startTime);
            acceleration.y = Mathf.Sin(startTime);
            //subtract that time from current time
            //do sin of created time onto x and y pos

        }
        //acceleration.x = -Mathf.Sin(Time.time);
        //acceleration.y = Mathf.Sin(Time.time);
        //acceleration.y = -Mathf.Sin(startTime - Time.time);
    }
}
