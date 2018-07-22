using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class AppendTimeLeft : MonoBehaviour {
	private Text textObj;

	void Awake()
	{
		textObj = GetComponent<Text>();
	}

	string GetString()
	{
		if(MarketManager.instance.inMarket)
		{
			return " seconds until market closes.";
		}
		else
		{
			return " seconds until market opens.";
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		textObj.text = Mathf.CeilToInt(MarketManager.instance.timeLeft) + GetString();
	}
}
