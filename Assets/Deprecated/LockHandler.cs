using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHandler : MonoBehaviour {
	public static Dictionary<ResourceEnum, LockHandler> locks = new Dictionary<ResourceEnum, LockHandler>();
	public ResourceEnum handling;
	public Animator lockAnim;

	// Use this for initialization
	public void Handle (ResourceEnum toHandle) {
		handling = toHandle;	
		GetComponentInChildren<BarHandler>().Handle(handling);
		foreach(MercSlot ms in GetComponentsInChildren<MercSlot>())
		{
			ms.row = handling;
		}
		locks[handling] = this;
	}

}
