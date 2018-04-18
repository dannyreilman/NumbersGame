using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MarketManager : MonoBehaviour 
{
	public static int LOCK_MIN = 10;
	public static float LOCK_PERCENT = 0.75f;
	public static MarketManager instance;

	public float timeLeft;
	
	bool inMarket = false;

	// Use this for initialization
	void Awake () 
	{
		Assert.IsNull(instance);
		instance = this;

		timeLeft = 10.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(inMarket)
		{
			for(int i = 1; i < ResourceStruct.resourceCount; ++i)
			{
				ResourceEnum e = (ResourceEnum) i;

				int val = ResourceHandler.instance.resource.GetResource(e);
				int otherVal = ResourceHandler.instance.enemyResource.GetResource(e);

				LockHandler.locks[e].SetLock(val >= LOCK_MIN && val / (val + otherVal) >= LOCK_PERCENT);
			}
		}
		else
		{
			timeLeft -= Time.deltaTime;
			if(timeLeft <= 0)
			{
				timeLeft = 0;
				inMarket = true;
			}
		}		
	}

	public void FinalizeLock(ResourceEnum toFinalize)
	{
		Debug.Log(toFinalize);
	}
}
