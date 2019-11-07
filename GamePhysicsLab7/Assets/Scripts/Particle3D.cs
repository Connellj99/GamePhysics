using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle3D : MonoBehaviour
{
    public GameObject shape;
    //private const float kGravity = -0.012f;

    //torque vectors
    private Vector2 localSpace = new Vector2(2.0f, 2.0f);
    private Vector2 otherSpace = new Vector2(0, -2);

    //private Vector2 anchorPosition;

    public Vector3 position;// = shape.transform.position;
    public Vector3 velocity;
    public Vector3 acceleration;
    private Vector3 force;

    public float startTime;
    public float restitution;

    public Quaternion rotation;

    public Vector3 angularVelocity;
    public Vector3 angularAcceleration;
    public Vector3 torque;
    public Matrix4x4 worldTransformationMatrix; //object to world
    public Matrix4x4 invworldTransformationMatrix; // object to local
    public Vector3 localCenterOfMass;
    public Vector3 worldCenterOfMass; 
    public Matrix4x4 localInertiaTensor;  // each frame, chage this to world after inverting
    public Matrix4x4 invlocalInertiaTensor;
    public Matrix4x4 worldInertiaTensor; 


    public dropDownPhysics phystype;
    public Oscillation circtype;

    public particleShape shapetype;

    [Range(0,Mathf.Infinity)]
    public float mass;


    public float invMass;


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


    public enum particleShape
    {
        SolidSphere,
        HollowSphere,
        SolidBox,
        HollowBox,
        SolidCylinder,
        SolidCone
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
        //quat + angularvel*quat *deltatime/2

        Quaternion qangularVel = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0.0f);
        Quaternion qangularVelRot = qangularVel * rotation;
        qangularVel.x = (0.5f) * deltaTime * qangularVelRot.x;
        qangularVel.y = (0.5f) * deltaTime * qangularVelRot.y;
        qangularVel.z = (0.5f) * deltaTime * qangularVelRot.z;
        qangularVel.w = (0.5f) * deltaTime * qangularVelRot.w;

        rotation = new Quaternion(rotation.x + qangularVel.x, rotation.y + qangularVel.y, rotation.z + qangularVel.z, rotation.w + qangularVel.w).normalized;

        angularVelocity += angularAcceleration * deltaTime;
    }

    public void updateRotationKinematic(float deltaTime)
    {
        //rotation += angularVelocity * deltaTime + angularAcceleration * deltaTime * deltaTime * 0.5f;

        Quaternion qangularVel = new Quaternion(angularVelocity.x, angularVelocity.y, angularVelocity.z, 0.0f);
        Quaternion qangularVelRot = qangularVel * rotation;
        qangularVel.x = (0.5f) * deltaTime * qangularVelRot.x;
        qangularVel.y = (0.5f) * deltaTime * qangularVelRot.y;
        qangularVel.z = (0.5f) * deltaTime * qangularVelRot.z;
        qangularVel.w = (0.5f) * deltaTime * qangularVelRot.w;

        rotation = new Quaternion(rotation.x + qangularVel.x, rotation.y + qangularVel.y, rotation.z + qangularVel.z, rotation.w + qangularVel.w);

        Quaternion newAngularAccel = new Quaternion(angularAcceleration.x, angularAcceleration.y, angularAcceleration.z, 0.0f);

        newAngularAccel.x = newAngularAccel.x * deltaTime * deltaTime * 0.5f;
        newAngularAccel.y = newAngularAccel.y * deltaTime * deltaTime * 0.5f;
        newAngularAccel.z = newAngularAccel.z * deltaTime * deltaTime * 0.5f;
        newAngularAccel.w = newAngularAccel.w * deltaTime * deltaTime * 0.5f;

        rotation = new Quaternion(rotation.x + newAngularAccel.x, rotation.y + newAngularAccel.y, rotation.z + newAngularAccel.z, rotation.w + newAngularAccel.w).normalized;


        angularVelocity += angularAcceleration * deltaTime;
    }

    public void AddForce(Vector3 newForce)
    {
        //forces can just be added
        force += newForce;

    }
       
    //make an addforceatpoint
    //get center of mass, calc vector from center to point, then do an add force

    public void AddForceAtPoint(Vector3 force, Vector3 point)
    {
        Vector3 forceVector = point - localCenterOfMass;

    }

    private void UpdateAcceleration()
    {
        //convert force to acceleration
        acceleration = force * invMass;

        force.Set(0.0f,0.0f,0.0f);
    }

    private void updateAngularAcceleration()
    {
        //convers torque to angular acceleration before resetting torque
        angularAcceleration =  localInertiaTensor * torque;
        torque = new Vector3(0.0f,0.0f,0.0f);
    }

    private void applyTorque()
    {
        //adds an amount of torque to the total torque acting on a particle
        //torque applied is calculated using 2D equivalent of T = pf x F: T is torque, pf is moment arm (point of applied force relative to center of mass)F is applied force at pf.  
        //It is important to note that the center of mass may not be the center of the object, so it might help to add a separate member for center of mass in local and world space.
        //torque = pf * f
        torque = Vector3.Cross(localSpace, otherSpace);
    }

    // Start is called before the first frame update
    void Start()
    {
        worldInertiaTensor = Matrix4x4.zero;
        force = Vector3.zero;
        torque = Vector3.zero;
        worldCenterOfMass = Vector3.zero;
        acceleration = Vector3.zero;
        worldTransformationMatrix = Matrix4x4.zero;
        invworldTransformationMatrix = Matrix4x4.zero;

        position = transform.position;

        
        //localInertiaTensor = shape.GetComponent<InterialTensor>().findInertia();
        localInertiaTensor = findInertia();
        Mass = mass;
        localCenterOfMass = shape.transform.position;
        startTime = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        applyTorque();


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

        

        transform.position = position;

        transform.rotation = rotation;  //new Vector3(0f,0f,rotation);

        if (circtype == Oscillation.Oscillate)
        {
            //Save time when object instantiated
            acceleration.x = -Mathf.Sin(startTime);
            acceleration.y = Mathf.Sin(startTime);
            //subtract that time from current time
            //do sin of created time onto x and y pos

        }


        
        worldTransformationMatrix = Matrix4x4.TRS(position, rotation, new Vector3(1, 1, 1));
        //world to object transform = worldTransformationMatrix.transpose
        invworldTransformationMatrix = worldTransformationMatrix.transpose;
        invlocalInertiaTensor = localInertiaTensor.transpose;
        worldInertiaTensor = worldTransformationMatrix * localInertiaTensor * invworldTransformationMatrix;

        updateAngularAcceleration();
        UpdateAcceleration();

        transform.SetPositionAndRotation(position, rotation);
        worldCenterOfMass = worldTransformationMatrix * localCenterOfMass;
        //set positiion and rotation
    }


    public float radius = 0;
    public float width = 0;
    public float height = 0;
    public float depth = 0;

    public Matrix4x4 findInertia()
    {
        Matrix4x4 inertia = new Matrix4x4();
        //Particle3D.particleShape newPartShape = shape.GetComponent<Particle3D>().shapetype;
        particleShape newPartShape = shapetype;
        if (newPartShape == Particle3D.particleShape.SolidSphere)
        {
            //float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(0.4f * mass * (radius * radius), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 0.4f * mass * (radius * radius), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.4f * mass * (radius * radius), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));

        }

        if (newPartShape == Particle3D.particleShape.HollowSphere)
        {
            //float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(0.66f * mass * (radius * radius), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 0.66f * mass * (radius * radius), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.66f * mass * (radius * radius), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));

        }

        if (newPartShape == Particle3D.particleShape.SolidBox)
        {
            //float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(0.083f * mass * ((height * height) + (depth * depth)), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 0.083f * mass * ((depth * depth) + (width * width)), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.083f * mass * ((height * height) + (width * width)), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));
        }
        if (newPartShape == Particle3D.particleShape.HollowBox)
        {
            //float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(1.66f * mass * ((height * height) + (depth * depth)), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 1.66f * mass * ((depth * depth) + (width * width)), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 1.66f * mass * ((height * height) + (width * width)), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));
        }
        if (newPartShape == Particle3D.particleShape.SolidCylinder)
        {
            //float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(0.083f * mass * (3 * (radius * radius) + (height * height)), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, 0.083f * mass * (3 * (radius * radius) + (height * height)), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.5f * mass * (radius * radius), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));
        }
        if (newPartShape == Particle3D.particleShape.SolidCone)
        {
            //float mass = shape.GetComponent<Particle3D>().mass;
            inertia.SetColumn(0, new Vector4(((0.6f * mass * (height * height)) + (0.15f * mass * (radius * radius))), 0, 0, 0));
            inertia.SetColumn(1, new Vector4(0, ((0.6f * mass * (height * height)) + (0.15f * mass * (radius * radius))), 0, 0));
            inertia.SetColumn(2, new Vector4(0, 0, 0.3f * mass * (radius * radius), 0));
            inertia.SetColumn(3, new Vector4(0, 0, 0, 1));
        }

        return inertia.inverse;
    }
}
