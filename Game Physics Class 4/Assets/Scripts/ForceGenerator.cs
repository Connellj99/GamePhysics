using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerator
{

    // f_gravity = mg;
    public static Vector2 GenerateForce_Gravity(float particleMass, float gravitationalConstant, Vector2 worldUp)
    {
        Vector2 f_gravity = particleMass * gravitationalConstant * worldUp;
        return f_gravity;
    }

    // f_normal = proj(f_gravity,surfaceNormal_unit)
    public static Vector2 GenerateForce_Normal(Vector2 f_gravity,Vector2 surfaceNormal_unit)
    {
        Vector2 f_normal = Vector3.Project(f_gravity, surfaceNormal_unit);
        return f_normal;
    }

    // f_sliding = f_gravity + f_normal
    public static Vector2 GenerateForce_Sliding(Vector2 f_gravity,Vector2 f_normal)
    {
        Vector2 f_sliding = f_gravity + f_normal;
        return f_sliding;
    }

    // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
    public static Vector2 GenerateForce_Friction_Static(Vector2 f_normal, Vector2 f_opposing, float frictionCoefficient_static)
    {
        Vector2 f_friction_s;

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
    public static Vector2 GenerateForce_Friction_Kinetic(Vector2 f_normal, Vector2 particleVelocity, float frictionCoefficient_kinetic)
    {
        Vector2 f_friction_k = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity;
        return f_friction_k;
    }

    // f_drag = (p * u^2 * area * coeff)/2    
    public static Vector2 GenerateForce_Drag(Vector2 particleVelocity,Vector2 fluidVelocity, float fluidDensity,float objectArea_crossSection, float objectDragCoefficient)
    {
        Vector2 f_drag = (particleVelocity * (fluidVelocity * fluidVelocity) * fluidDensity * objectArea_crossSection * objectDragCoefficient)*0.5f;
        return f_drag;
    }
    // f_spring = -coeff*(spring length - spring restingLength)
    public static Vector2 GenerateForce_Spring(Vector2 particlePosition, Vector2 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        float springLength = Vector2.Distance(anchorPosition,particlePosition);
        Vector2 f_spring = new Vector2(0.0f, -springStiffnessCoefficient * (springLength - springRestingLength));

        return f_spring;
    }
}
