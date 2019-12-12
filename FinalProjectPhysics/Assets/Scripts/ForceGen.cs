using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGen : MonoBehaviour
{
    // f_gravity = mg;
    public static Vector3 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        Vector3 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }

    // f_normal = proj(f_gravity,surfaceNormal_unit)
    public static Vector3 GenerateForce_Normal(Vector3 f_gravity, Vector3 surfaceNormal_unit)
    {
        Vector3 f_normal = Vector4.Project(f_gravity, surfaceNormal_unit);
        return f_normal;
    }

    // f_sliding = f_gravity + f_normal
    public static Vector3 GenerateForce_Sliding(Vector3 f_gravity, Vector3 f_normal)
    {
        Vector3 f_sliding = f_gravity + f_normal;
        return f_sliding;
    }

    // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
    public static Vector3 GenerateForce_Friction_Static(Vector3 f_normal, Vector3 f_opposing, float frictionCoefficient_static)
    {
        Vector3 f_friction_s;

        if (f_opposing.magnitude < frictionCoefficient_static * f_normal.magnitude)
        {
            f_friction_s = -f_opposing;
        }
        else
        {
            f_friction_s = -frictionCoefficient_static * f_normal;
        }

        return f_friction_s;
    }

    // f_friction_k = -coeff*|f_normal| * unit(vel)
    public static Vector3 GenerateForce_Friction_Kinetic(Vector3 f_normal, Vector3 particleVelocity, float frictionCoefficient_kinetic)
    {
        Vector3 f_friction_k = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity;
        return f_friction_k;
    }

    // f_drag = (p * u^2 * area * coeff)/2    
    public static Vector3 GenerateForce_Drag(Vector3 f_drag, float objectDragCoefficient)
    {
        //Vector3 f_drag = (particleVelocity * Vector3.Dot(fluidVelocity,fluidVelocity) * fluidDensity * objectArea_crossSection * objectDragCoefficient) * 0.5f;
        objectDragCoefficient = 0.2f * objectDragCoefficient + 0.15f * (objectDragCoefficient * objectDragCoefficient);
        f_drag.Normalize();
        f_drag *= -objectDragCoefficient;
        return f_drag;
    }
    // f_spring = -coeff*(spring length - spring restingLength)
    public static Vector3 GenerateForce_Spring(Vector3 particlePosition, Vector3 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        float springLength = Vector3.Distance(anchorPosition, particlePosition);
        Vector3 f_spring = new Vector3(0.0f, -springStiffnessCoefficient * (springLength - springRestingLength));

        return f_spring;
    }
}
