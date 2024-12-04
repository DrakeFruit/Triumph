using System.Numerics;
using Sandbox;
using Sandbox.Citizen;

public sealed class TriumphController : Component, Component.ITriggerListener
{
	[RequireComponent] public BoxCollider Collider { get; set; }
	[Property] public float StrafeSpeed { get; set; } = 250;
	[Property] public float RunSpeed { get; set; } = 200;
	[Property] public GameObject WarriorPrefab { get; set; }
	[Property] public SoundEvent GateSound { get; set; }
	[Property] public SoundEvent WinSound { get; set; }
	public WinScreen WinPanel { get; set; }
	public LoseScreen LosePanel { get; set; }
	public List<WarriorPathing> Warriors { get; set; } = new();
	protected override void OnStart()
	{
		WinPanel = Components.GetInChildrenOrSelf<WinScreen>( true );
		LosePanel = Components.GetInChildrenOrSelf<LoseScreen>( true );
		WarriorPrefab.Clone( LocalPosition );
		foreach ( var i in Scene.GetAllComponents<WarriorPathing>() )
		{
			Warriors.Add( i );
		}
	}
	protected override void OnFixedUpdate()
	{
		if ( !Warriors.Any() ) Lose();
		LocalPosition += new Vector3( RunSpeed, Input.AnalogMove.y * StrafeSpeed, 0 ) * Time.Delta;
		foreach ( var i in Warriors )
		{
			var moveDir = LocalPosition - i.LocalPosition;
			i.Agent.MoveTo( LocalPosition );
		}
	}

	public void OnTriggerEnter( GameObject other )
	{
		if ( other.Tags.Has( "finish" ) )
		{
			Win();
		}
		if ( !other.Tags.Has( "gate" ) ) return;
		
		Sound.Play( GateSound );
		var gate = other.GetComponent<Gate>();
		var amount = gate.GetAmount( Warriors );
		while ( amount != Warriors.Count )
		{
			if ( Warriors.Count > amount )
			{
				var w = Warriors.First();
				w.DestroyGameObject();
				Warriors.Remove( w );
			}
			if ( Warriors.Count < amount )
			{
				Warriors.Add( gate.Prefab.Clone( LocalPosition ).Components.GetInChildrenOrSelf<WarriorPathing>() );
			}
		}
	}

	public void Win()
	{
		WinPanel.Enabled = true;
		Scene.TimeScale = 0;
		Sound.Play( WinSound );
		WinPanel.FinalCount = Warriors.Count();
	}
	public void Lose()
	{
		LosePanel.Enabled = true;
		Scene.TimeScale = 0;
	}
}
