#include "PhysicsPlugin.h"
#include "PhysicsWorld.h"

PhysicsWorld* PhysicsWorldInstance = nullptr;
 void CreatePhysicsWorld()
{
	if (PhysicsWorldInstance == nullptr)
	{
		PhysicsWorldInstance = new PhysicsWorld;
	}
	PhysicsWorldInstance->Reset();
}

 void DestroyPhysicsWorld()
{
	if (PhysicsWorldInstance != nullptr)
	{
		delete PhysicsWorldInstance;
		PhysicsWorldInstance = nullptr;
	}
}

 void UpdatePhysicsWorld(float deltaTime)
{
	if (PhysicsWorldInstance != nullptr)
	{
		PhysicsWorldInstance->Update(deltaTime);
	}
}

 void AddForce(float xForce, float yForce, float zForce)
 {
	 if (PhysicsWorldInstance != nullptr)
	 {
		 PhysicsWorldInstance->AddForce(xForce, yForce, zForce);
	 }
 }

 void AddParticle3D(float& mass, float& boxHeight, float& boxWidth, float& boxLength)
 {
	 if (PhysicsWorldInstance != nullptr)
	 {
		 PhysicsWorldInstance->AddParticle3D(mass, boxHeight, boxWidth, boxLength);
	 }
 }


