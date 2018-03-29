using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class AppendTimeLeft : MonoBehaviour {
	private Text textObj;
	private string oldString;

	void Awake()
	{
		textObj = GetComponent<Text>();
		if(textObj == null || textObj.Equals(null))
		{
			Debug.Log("AppendTimeLeft requires a text object on the attached object");
			Assert.AreNotEqual(textObj, null);
		}

		oldString  = textObj.text;
	}
	
	// Update is called once per frame
	void Update () 
	{
		textObj.text = oldString + (int)MarketManager.instance.timeLeft;
	}
}
