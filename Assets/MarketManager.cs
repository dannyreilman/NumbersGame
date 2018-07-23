using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MarketManager : MonoBehaviour 
{
	public static int LOCK_MIN = 10;
	public static float LOCK_PERCENT = 0.75f;
	public static float timeOutOfMarket = 15.0f;
	public static float timeInMarket = 30.0f;
	public static MarketManager instance;
	public float timeLeft;
	public bool inMarket = false;
	public int[] locks = new int[ResourceStruct.resourceCount];
	public ResourceStruct GetAllyLockFactor()
	{
		ResourceStruct toReturn = new ResourceStruct((locks[0]==-1)?0:1,
								  (locks[1]==-1)?0:1,
								  (locks[2]==-1)?0:1,
								  (locks[3]==-1)?0:1,
								  (locks[4]==-1)?0:1,
								  (locks[5]==-1)?0:1,
								  (locks[6]==-1)?0:1);
		Debug.Log(toReturn);
		return toReturn;
	}
	public ResourceStruct GetEnemyLockFactor()
	{
		return new ResourceStruct((locks[0]==1)?0:1,
								  (locks[1]==1)?0:1,
								  (locks[2]==1)?0:1,
								  (locks[3]==1)?0:1,
								  (locks[4]==1)?0:1,
								  (locks[5]==1)?0:1,
								  (locks[6]==1)?0:1);
	}

	// Use this for initialization
	void Awake () 
	{
		Assert.IsNull(instance);
		instance = this;

		timeLeft = timeOutOfMarket;
		UnSetLocks();
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeLeft -= Time.deltaTime;
		if(timeLeft <= 0)
		{
			inMarket = !inMarket;
			transform.GetChild(0).gameObject.SetActive(!inMarket);
			if(inMarket)
			{
				UnSetLocks();
				timeLeft = timeInMarket;
			}
			else
			{
				timeLeft = timeOutOfMarket;
			}
		}		
	}

	public void UnSetLocks()
	{
		for(int i = 0; i < ResourceStruct.resourceCount; ++i)
		{
			locks[i] = 0;
		}
	}

	public void Lock(ResourceEnum toLock, bool side)
	{
		locks[(int)toLock] = side ? 1:-1;
		Debug.Log(side);
		LockHandler.locks[toLock].lockAnim.SetTrigger("go");
	}
}
