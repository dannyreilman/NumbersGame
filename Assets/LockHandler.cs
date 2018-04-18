using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHandler : MonoBehaviour {
	public static Dictionary<ResourceEnum, LockHandler> locks = new Dictionary<ResourceEnum, LockHandler>();
	public ResourceEnum handling;
	Animator lockAnim;

	// Use this for initialization
	public void Handle ( ResourceEnum toHandle) {
		handling = toHandle;	
		GetComponentInChildren<BarHandler>().toHandle = handling;
		locks[handling] = this;

		lockAnim = GetComponentInChildren<Animator>();
	}
	
	public void SetLock(bool lockVal)
	{
		lockAnim.SetBool("Lock", lockVal);
	}

	public void FinalizeLock()
	{
		MarketManager.instance.FinalizeLock(handling);
	}
}
