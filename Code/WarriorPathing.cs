using Sandbox;
using Sandbox.Citizen;

public sealed class WarriorPathing : Component
{
	[RequireComponent] public NavMeshAgent Agent { get; set; }
	[RequireComponent] public CitizenAnimationHelper Helper { get; set; }
	protected override void OnFixedUpdate()
	{
		Helper.WithVelocity( new Vector3(Agent.Velocity.x, 0, 0 ) );
	}
}
