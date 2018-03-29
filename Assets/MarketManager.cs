using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MarketManager : MonoBehaviour 
{
	public static MarketManager instance;

	public float timeLeft;
	
	// Use this for initialization
	void Awake () 
	{
		Assert.IsNull(instance);
		instance = this;

		timeLeft = 60.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeLeft -= Time.deltaTime;
	}
}
