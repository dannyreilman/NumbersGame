using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class Unlockable : MonoBehaviour
{
	void Awake()
	{
		foreach(Transform children in transform)
		{
			Unlockable toUnlock = children.GetComponent<Unlockable>();
			if(toUnlock != null && !toUnlock.Equals(null))
			{
				toUnlock.Lock();
			}
		}
	}

	public abstract void Unlock();
	public abstract void Lock();
}