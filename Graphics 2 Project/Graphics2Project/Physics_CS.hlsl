struct particle
{
	float posX;
	float posY;
	float posZ;
	float radius;
	float velX;
	float velY;
	float velZ;
	float mass;
	float deltaTime;
	float3 buffer;
};

RWStructuredBuffer<particle> structBuffer: register(u0);

[numthreads(1, 1, 1)]
void main(uint3 input : SV_DispatchThreadID)
{
	particle ball = structBuffer[input.x];

	float3 position, velocity, gravity;

	position.x = ball.posX;
	position.y = ball.posY;
	position.z = ball.posZ;

	velocity.x = ball.velX;
	velocity.y = ball.velY;
	velocity.z = ball.velZ;

	gravity.x = 0.0f;
	gravity.y = -9.8f;
	gravity.z = 0.0f;

	velocity = velocity + (gravity * ball.deltaTime);
	position += (velocity * ball.deltaTime) + (0.5f * gravity * (ball.deltaTime * ball.deltaTime));

	if (position.x + ball.radius > 0.0f)
	{
		position.x = 0.0f - ball.radius;
		velocity = reflect(velocity * 0.7f, float3(-1.0f, 0.0f, 0.0f));
	}
	if (position.x - ball.radius < -10.0f)
	{
		position.x = -10.0f + ball.radius;
		velocity = reflect(velocity * 0.7f, float3(1.0f, 0.0f, 0.0f));
	}
	//if (position.y + ball.radius >= 8.5f)
	//{
	//	position.y = 8.5f - ball.radius;
	//	velocity = reflect(velocity * 0.9f, float3(0.0f, -1.0f, 0.0f));
	//}
	if (position.y - ball.radius <= -1.5f)
	{
		position.y = -1.5f + ball.radius;
		velocity = reflect(velocity * 0.7f, float3(0.0f, 1.0f, 0.0f));
	}
	if (position.z + ball.radius > 10.0f)
	{
		position.z = 10.0f - ball.radius;
		velocity = reflect(velocity * 0.7f, float3(0.0f, 0.0f, 1.0f));
	}
	if (position.z - ball.radius < 0.0f)
	{
		position.z = 0.0f + ball.radius;
		velocity = reflect(velocity * 0.7f, float3(0.0f, 0.0f, -1.0f));
	}

	structBuffer[input.x].posX = position.x;
	structBuffer[input.x].posY = position.y;
	structBuffer[input.x].posZ = position.z;

	structBuffer[input.x].velX = velocity.x;
	structBuffer[input.x].velY = velocity.y;
	structBuffer[input.x].velZ = velocity.z;
}