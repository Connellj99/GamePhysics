using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;


public struct cubeSpawnData : IComponentData
{
    public int NumXCubes;
    public int NumZCubes;
    public Vector3 positionData;
    public Vector3 velocityData;
    public Vector3 accelerationData;
    public Quaternion rotationData;
    public Vector3 angularAccelerationData;
    public Vector3 angularVelocityData;
    public Entity cubePrefabEntity;
}
[RequiresEntityConversion]
public class CubeSpawner : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public int NumXCubes;
    public int NumZCubes;
    public Vector3 position;
    public Vector3 velocity;
    public Vector3 acceleration;
    public Quaternion rotation;
    public Vector3 angularAcceleration;
    public Vector3 angularVelocity;
    public GameObject cubePrefab;

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(cubePrefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var varCubePrefabEntity = conversionSystem.GetPrimaryEntity(cubePrefab);
        var cubeSpawnerData = new cubeSpawnData
        {
            NumXCubes = NumXCubes,
            NumZCubes = NumZCubes,
            positionData = position,
            velocityData = velocity,
            accelerationData = acceleration,
            rotationData = rotation,
            angularAccelerationData = angularAcceleration,
            angularVelocityData = angularVelocity,
            cubePrefabEntity = varCubePrefabEntity,
        };
        dstManager.AddComponentData(entity, cubeSpawnerData);
    }
}