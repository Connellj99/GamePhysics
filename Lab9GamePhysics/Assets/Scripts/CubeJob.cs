using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;

public class CubeJob : JobComponentSystem
{

    public struct cubeJob : IJobForEach<Rotation, CubeValues, Translation>
    {
        public float deltaTime;
        public void Execute(ref Rotation rotation, ref CubeValues cubevals, ref Translation translation)
        {
            Quaternion angularVelQuat = new Quaternion(cubevals.angularVelocity.x, cubevals.angularVelocity.y, cubevals.angularVelocity.z, 0.0f);
            Quaternion angularRotQuat = angularVelQuat * cubevals.rotation;
            angularRotQuat.x = angularRotQuat.x * deltaTime * 0.5f;
            angularRotQuat.y = angularRotQuat.y * deltaTime * 0.5f;
            angularRotQuat.z = angularRotQuat.z * deltaTime * 0.5f;
            angularRotQuat.w = angularRotQuat.w * deltaTime * 0.5f;

            cubevals.rotation = 
                new Quaternion(
                (cubevals.rotation.x + angularRotQuat.x), 
                (cubevals.rotation.y + angularRotQuat.y), 
                (cubevals.rotation.z + angularRotQuat.z), 
                (cubevals.rotation.w + angularRotQuat.w)).normalized;
            cubevals.position += cubevals.velocity * deltaTime;
            cubevals.velocity += cubevals.acceleration * deltaTime;
            cubevals.angularVelocity = cubevals.angularAcceleration * deltaTime;
            translation.Value = cubevals.position;
            rotation.Value = cubevals.rotation;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return new cubeJob { deltaTime = Time.deltaTime }.Schedule(this, inputDeps);
    }
}
