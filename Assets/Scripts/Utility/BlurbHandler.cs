using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class BlurbHandler : MonoBehaviour {
	public static string delimiter = " | ";
	public static float charEstimate = 5.0f;
	public static float startingSpeed = 4.0f;
	public static int minLen;

	private Text text;

	private float scrollDelay;
	private float totalTimeLeft;

	public static BlurbHandler instance = null;
	// Use this for initialization
	void Awake () {
		if(instance == null || instance.Equals(null))
		{
			instance = this;
			text = GetComponent<Text>();
			minLen = (int)(Screen.width / charEstimate);

			SetScrollSpeed(startingSpeed);

			while(text.text.Length < minLen)
			{
				GenerateRandomBlurb();
			}

			StartCoroutine(TickBy());
		}	
		else
		{
			Assert.AreEqual(instance, this);
		}
	}

	public static void SetScrollSpeed(float charPerSecond)
	{
		instance.scrollDelay = 1.0f / charPerSecond;
	}
	
	public static void TickMessage(string message)
	{
		instance.text.text += delimiter + message;
	}

	public static void GenerateRandomBlurb()
	{
		TickMessage("This is a random blurb which is being sent");
	}

	private IEnumerator TickBy()
	{
		while(true)
		{
			yield return new WaitForSeconds(scrollDelay);
			totalTimeLeft -= scrollDelay;
			if(text.text.Length > 0)
			{
				text.text = text.text.Substring(1);
			}

			while(text.text.Length < minLen)
			{
				GenerateRandomBlurb();
			}
		}
	}
}
