#include "PhysicsWorld.h"



void Particle3D::particleUpdate(float deltaTime)
{
	//update position based on velocity
	posX += velX * deltaTime;
	posY += velY * deltaTime;
	posZ += velZ * deltaTime;

	//update velocity based on acceleration
	velX += accelX * deltaTime;
	velY += accelY * deltaTime;
	velZ += accelZ * deltaTime;

	//update acceleration
	accelX = invMass * forceX;
	accelY = invMass * forceY;
	accelZ = invMass * forceZ;

	//zero out forces
	forceX = 0.0;
	forceY = 0.0;
	forceZ = 0.0;
}

//sets forces from PhysWorld to Particle3d
void Particle3D::setForce(float x, float y, float z)
{
	forceX = x;
	forceY = y;
	forceZ = z;
}

//clear the object pool
void PhysicsWorld::Reset()
{
	particle3DPool.clear();
	particle3DPool.shrink_to_fit();
}


PhysicsWorld::PhysicsWorld()
{
	
}

PhysicsWorld::~PhysicsWorld()
{
}

//update each individual object in the pool
void PhysicsWorld::Update(float deltaTime)
{
	for (int i = 0; i < particle3DPool.size(); ++i)
	{
		particle3DPool[i].particleUpdate(deltaTime);
	}

}

//creates object pool using Particle3Ds
void PhysicsWorld::AddParticle3D(float& mass, float& boxHeight, float& boxWidth, float& boxLength)
{
	//int particlePoolSize = 1024;
	for (int i = 0; i < particle3DPool.size(); ++i)
	{

		particle3DPool.push_back(Particle3D(mass, boxHeight, boxWidth, boxLength));

	}
}

//gives each object in pool additional forces
void PhysicsWorld::AddForce(float xForce, float yForce, float zForce)
{
	for (int i = 0; i < particle3DPool.size(); ++i)
	{
		particle3DPool[i].setForce(xForce, yForce, zForce);
	}
}


