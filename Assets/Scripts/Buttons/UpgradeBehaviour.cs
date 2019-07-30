using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UpgradeBehaviour : MonoBehaviour 
{
	protected TransactionButton upgrading;
	protected Button upgradeObj;

	void Awake()
	{
		upgrading = GetComponent<TransactionButton>();
	}

	public abstract void DoUpgrade();
}
