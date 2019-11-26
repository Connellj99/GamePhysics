#pragma once
#include <vector>

class Particle3D 
{
public:
	//values need to be references
	float& mass;
	float& posX;
	float& posY;
	float& posZ;

	float velX = 0.0;
	float velY = 0.0;
	float velZ = 0.0;

	float accelX = 0.0;
	float accelY = 0.0;
	float accelZ = 0.0;

	float forceX = 0.0;
	float forceY = 0.0;
	float forceZ = 0.0;
	float invMass = 0.0;

	Particle3D(float& mass, float& boxHeight, float& boxWidth, float& boxLength) : mass(mass), posX(boxHeight), posY(boxWidth), posZ(boxLength)
	{
		invMass = mass > 0.0f ? 1.0f / mass : 0.0f;
	}

	void particleUpdate(float deltaTime);
	void setForce(float x, float y, float z);
};

class PhysicsWorld
{
public:
	explicit PhysicsWorld();
	~PhysicsWorld();

	void Update(float deltaTime);

	void AddParticle3D(float& mass, float& boxHeight, float& boxWidth, float& boxLength);

	void AddForce(float forcex, float forcey, float forcez);

	void Reset();


private:

	std::vector<Particle3D> particle3DPool;

};