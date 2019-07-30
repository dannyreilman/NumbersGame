using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseWidth : UpgradeBehaviour 
{
	public float percentReturn;

	public ResourceStruct widthIncrease;

	public override void DoUpgrade()
	{
		upgrading.cost += widthIncrease;
		upgrading.returnValue = upgrading.cost * percentReturn;
	}
}
