using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour 
{
	public static int LOCK_MIN = 500;
	public static float LOCK_PERCENT = 0.75f;
	public ResourceEnum toHandle;
	public float otherAmount = 0.0f;
	
	private RectTransform friendlyBar;
	private RectTransform enemyBar;

	void Awake()
	{
		friendlyBar = transform.GetChild(0) as RectTransform;	
		enemyBar = transform.GetChild(1) as RectTransform;
		
		//Set top line
		(transform.GetChild(2) as RectTransform).anchorMin  = new Vector2(0f, LOCK_PERCENT);
		(transform.GetChild(2) as RectTransform).anchorMax = new Vector2(1.0f, LOCK_PERCENT);

		
		//Set bottom line
		(transform.GetChild(3) as RectTransform).anchorMin  = new Vector2(0f,  1 - LOCK_PERCENT);
		(transform.GetChild(3) as RectTransform).anchorMax = new Vector2(1.0f,  1 - LOCK_PERCENT);
	}

	void Start()
	{
		friendlyBar.GetComponent<Image>().color = ResourceHandler.instance.getColor(toHandle);
	}

	// Update is called once per frame
	void Update () 
	{
		float amount = ResourceHandler.instance.resource.GetResource(toHandle);
		if(amount + otherAmount < (LOCK_MIN/LOCK_PERCENT))
		{
			friendlyBar.anchorMax = new Vector2(1.0f, amount * LOCK_PERCENT / (float)LOCK_MIN);
			enemyBar.anchorMin = new Vector2(0f, 1 - (otherAmount / (float)LOCK_MIN));
		}
		else
		{
			friendlyBar.anchorMax = new Vector2(1.0f, amount / (amount + otherAmount));
			enemyBar.anchorMin = new Vector2(0f, 1 - (otherAmount / (amount + otherAmount)));
		}
	}
}
