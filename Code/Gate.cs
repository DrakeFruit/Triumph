using System;
using Sandbox;

public sealed class Gate : Component
{
	[Property] public GameObject Prefab { get; set; }
	[Property] public CalcType CalculationCalcType { get; set; }
	[Property] public float CalculationFactor { get; set; }
	public List<WarriorPathing> SpawnPrefabs(Vector3 position, List<WarriorPathing> warriors)
	{
		var amountNeeded = 0.0;
		switch ( CalculationCalcType )
		{
			case CalcType.Add:
				amountNeeded = CalculationFactor;
				break;
			case CalcType.Subtract:
				amountNeeded = -CalculationFactor;
				break;
			case CalcType.Multiply:
				amountNeeded = warriors.Count * CalculationFactor;
				break;
			case CalcType.Divide:
				amountNeeded = warriors.Count / CalculationFactor;
				break;
		}

		for ( var i = 0; i < amountNeeded; i++ )
		{
			if ( CalculationFactor >= 0 )
			{
				var warrior = Prefab.Clone( position - i );
				warriors.Add( warrior.Components.GetInChildrenOrSelf<WarriorPathing>() );
			}
			else
			{
				warriors.Remove( warriors.First() );
			}
		}

		return warriors;
	}
	public enum CalcType
	{
		Add,
		Subtract,
		Multiply,
		Divide,
	}
}
