using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour 
{
	[HideInInspector]
	public ResourceEnum toHandle;
	public float otherAmount = 0.0f;
	
	private RectTransform friendlyBar;
	private RectTransform enemyBar;

	void Awake()
	{
		friendlyBar = transform.GetChild(0) as RectTransform;	
		enemyBar = transform.GetChild(1) as RectTransform;
	}

	void Start()
	{
		friendlyBar.GetComponent<Image>().color = ResourceHandler.instance.getColor(toHandle);
	}

	// Update is called once per frame
	void Update () 
	{
		float amount = ResourceHandler.instance.allyResource.GetResource(toHandle);
		if(amount + otherAmount < (MarketManager.LOCK_MIN/MarketManager.LOCK_PERCENT))
		{
			friendlyBar.anchorMax = new Vector2(1.0f, amount * MarketManager.LOCK_PERCENT / (float)MarketManager.LOCK_MIN);
			enemyBar.anchorMin = new Vector2(0f, 1 - (otherAmount / (float)MarketManager.LOCK_MIN));
		}
		else
		{
			friendlyBar.anchorMax = new Vector2(1.0f, amount / (amount + otherAmount));
			enemyBar.anchorMin = new Vector2(0f, 1 - (otherAmount / (amount + otherAmount)));
		}
	}
}
