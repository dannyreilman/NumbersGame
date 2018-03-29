using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarHandler : MonoBehaviour 
{
	public static int LOCK_MIN = 1000;
	public ResourceEnum toHandle;
	
	private RectTransform trans;

	void Awake()
	{
		trans = transform as RectTransform;	
	}

	// Update is called once per frame
	void Update () 
	{
		int amount = ResourceHandler.instance.resource.GetResource(toHandle);
		if(amount < LOCK_MIN)
		{
			trans.anchorMax = new Vector2(1.0f,amount / (float)LOCK_MIN);
		}
		else
		{
			//TODO: Incorporate multiplayer here
			trans.anchorMax = new Vector2(1.0f, 1.0f);
		}
	}
}
