using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Plate : Unlockable {
	public ResourceStruct unlockCost;

	private Color unBoughtColor;
	public Color boughtColor;

	public bool locked = true;
	private bool bought = false;
	private GameObject buyButton = null;
	
	void Start()
	{
			buyButton = transform.GetChild(0).gameObject;
			unBoughtColor = GetComponent<Image>().color;
	}

	void Update()
	{
		if(!locked && !bought)
		{
			buyButton.GetComponent<Button>().enabled = ResourceHandler.instance.resource >= unlockCost;
		}
	}

	public override void Unlock()
	{
		locked = false;
		buyButton.GetComponentInChildren<Text>().text = "$";
		buyButton.GetComponent<Button>().enabled = true;
	}

	public override void Lock()
	{
		if(buyButton == null || buyButton.Equals(null))
		{
			buyButton = transform.GetChild(0).gameObject;
		}

		locked = true;
		buyButton.GetComponentInChildren<Text>().text = "Loc";
		buyButton.GetComponent<Button>().enabled = false;
	}

	public void BuyPlate()
	{
		if(ResourceHandler.instance.resource >= unlockCost)
		{
			ResourceHandler.instance.resource -= unlockCost;
			GetComponent<Image>().color = boughtColor;
			bought = true;
			foreach(Transform children in transform)
			{
				Unlockable toUnlock = children.GetComponent<Unlockable>();
				if(toUnlock != null && !toUnlock.Equals(null))
				{
					toUnlock.Unlock();
				}
			}

			buyButton.SetActive(false);
		}
	}
}
