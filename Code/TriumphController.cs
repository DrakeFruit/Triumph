using Sandbox;

public sealed class TriumphController : Component
{
	[Property] private float StrafeSpeed { get; set; } = 250;
	[Property] private float RunSpeed { get; set; } = 200;
	protected override void OnFixedUpdate()
	{
		LocalPosition += new Vector3( RunSpeed, Input.AnalogMove.y * StrafeSpeed, 0 ) * Time.Delta;
	}
}
