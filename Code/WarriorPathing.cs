using Sandbox;

public sealed class WarriorPathing : Component
{
	[Property] TriumphController Controller { get; set; }
	protected override void OnFixedUpdate()
	{
		var targetDir = Controller.WorldPosition - LocalPosition;
		LocalPosition += (targetDir) * Time.Delta * 5 ;
		LocalRotation = Rotation.LookAt( targetDir );
	}
}
