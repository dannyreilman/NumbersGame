using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueWellUpdate : MercenaryCall 
{
	private float amountAccumulated = 0.0f;
	
	private ResourceStruct oneBlue = new ResourceStruct(0,0,1,0,0,0,0);

	// Use this for initialization
	public void Call (float[] args) 
	{
		amountAccumulated += args[0] * Time.deltaTime;
		while(amountAccumulated > 1)
		{
			ResourceHandler.instance.allyResource += oneBlue;
			amountAccumulated -= 1;
		}
	}
}
