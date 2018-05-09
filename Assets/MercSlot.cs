using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercSlot : MonoBehaviour 
{
	const int DIRECT_FACTOR = 5;

	public bool general;
	public MercSlot opposite;

	[HideInInspector]	
	public ResourceEnum row;

	[HideInInspector]
	public bool occupied = false;

	public bool ally;
	void Awake()
	{
		if(!general)
		{
			for(int i = 0; i < transform.parent.childCount; ++i)
			{
				MercSlot comp = transform.parent.GetChild(i).GetComponent<MercSlot>();
				if(comp != null && comp != this)
				{
					opposite = comp;
				}
			}
		}
		else
		{
			opposite = null;
		}
	}

	public bool doAttack(int attack)
	{
		if(opposite == null || opposite.Equals(null))
		{
			return false;
		}

		if(opposite.occupied)
		{
			Debug.Log("Not implemented");
		}
		else
		{
			ResourceHandler.instance.resource.resourceArray[(int)row] += DIRECT_FACTOR * attack;
		}
	
		return true;
	}
}
