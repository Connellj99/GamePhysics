using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public struct CubeValues : IComponentData
{
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Quaternion rotation;
    public Vector3 angularAcceleration;
    public Vector3 angularVelocity;
}

public class CubeData : ComponentSystem
{
    protected override void OnUpdate()
    {

        Entities.ForEach((Entity entity, ref cubeSpawnData spawnerData) =>
        {
            for (int x = 0; x < spawnerData.NumXCubes; ++x)
            {
                float posX = x - (spawnerData.NumXCubes / 2);

                for (int z = 0; z < spawnerData.NumZCubes; ++z)
                {
                    float posZ = z - (spawnerData.NumZCubes / 2);

                    // Actually create a rotating cube entity from the prefab.
                    var cubeEntity = EntityManager.Instantiate(spawnerData.cubePrefabEntity);

                    // Set the position of the rotating cube.
                    spawnerData.positionData = new Vector3(posX, 0.0f, posZ);
                    EntityManager.SetComponentData(cubeEntity, new Translation { Value = new float3(posX, 0.0f, posZ) });
                    EntityManager.AddComponentData(cubeEntity, new CubeValues 
                    { 
                        position = spawnerData.positionData, 
                        velocity = spawnerData.velocityData,
                        acceleration = spawnerData.accelerationData,
                        rotation = spawnerData.rotationData,
                        angularAcceleration = spawnerData.angularAccelerationData,
                        angularVelocity = spawnerData.angularVelocityData
                    });                 
                }
            }

            EntityManager.DestroyEntity(entity);
        });
    }
}
