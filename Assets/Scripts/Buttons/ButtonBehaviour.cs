using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewButton", menuName = "ScriptObjects/ButtonBehaviour", order = 1)]
public class ButtonBehaviour : ScriptableObject 
{
	public enum UpgradeType
	{
		None, Flat, Width
	}

	public string name;
	public UpgradeType upgradeType;
	public ResourceStruct buyCost;
	public ResourceStruct initCost;
	public ResourceStruct initReturn;

	public float initDelay;

	public bool startBought;

	public int maxLevel;
	
	/**
	 * A bunch of values to be used for different upgrade types
	 */
	
	//For flat
	public ResourceStruct flatUpgradeAmount;

	//For Width
	public ResourceStruct widthIncrease;

	public void Initialize(TransactionButton button)
	{
		button.SetName(name);
		button.buyCost = buyCost;
		button.cost = initCost;
		button.returnValue = initReturn;
		button.maxLevel = maxLevel;
		button.delay = initDelay;

		if(startBought)
		{
			button.level = 1;
		}

		switch(upgradeType)
		{
			case UpgradeType.Flat:
				FlatUpgrade fAdded = button.gameObject.AddComponent<FlatUpgrade>();
				fAdded.upgradeAmt = flatUpgradeAmount;
			break;
			case UpgradeType.Width:
				IncreaseWidth wAdded = button.gameObject.AddComponent<IncreaseWidth>();
				wAdded.widthIncrease = widthIncrease;
				wAdded.percentReturn = initReturn / initCost;
			break;
		}
	}
	
}
