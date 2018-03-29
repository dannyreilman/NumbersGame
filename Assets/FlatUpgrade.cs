using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatUpgrade : UpgradeBehaviour {

	public ResourceStruct upgradeAmt;

	public override void DoUpgrade()
	{
		upgrading.returnValue += upgradeAmt;
	}
}
