using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueWellUpdate : MercenaryCall 
{
	private float amountAccumulated = 0.0f;
	
	private ResourceStruct oneBlue = new ResourceStruct(0,0,1,0,0,0,0);
	const float speed = 2.0f;

	// Use this for initialization
	public void Call () 
	{
		amountAccumulated += speed * Time.deltaTime;
		while(amountAccumulated > 1)
		{
			ResourceHandler.instance.resource += oneBlue;
			amountAccumulated -= 1;
		}
	}
}
