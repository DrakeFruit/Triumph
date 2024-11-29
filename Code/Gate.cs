using System;
using Sandbox;

public sealed class Gate : Component
{
	[Property] public GameObject Prefab { get; set; }
	[Property] public CalcType CalculationType { get; set; }
	[Property] public int CalculationFactor { get; set; }
	public int GetAmount(List<WarriorPathing> warriors)
	{
		var amountNeeded = 0;
		switch ( CalculationType )
		{
			case CalcType.Add:
				amountNeeded = warriors.Count + CalculationFactor;
				break;
			case CalcType.Subtract:
				amountNeeded = warriors.Count - CalculationFactor;
				break;
			case CalcType.Multiply:
				amountNeeded = warriors.Count * CalculationFactor;
				break;
			case CalcType.Divide:
				amountNeeded = warriors.Count / CalculationFactor;
				break;
		}

		return amountNeeded;
	}
	public enum CalcType
	{
		Add,
		Subtract,
		Multiply,
		Divide,
	}
}
