using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShift : MonoBehaviour {

	private float shiftAmt = 0.0f;
	float speed = 10f;
	
	private RectTransform paneTransform;

	float smoothstep(float t)
	{
		return (0.9f) * t*t * (2f -  t);
	}

	void Awake()
	{
		paneTransform = GetComponent<RectTransform>();
	}

	void Update () 
	{
		paneTransform.anchorMin = new Vector2(paneTransform.anchorMin.x, 0.0f - smoothstep(shiftAmt));
		paneTransform.anchorMax = new Vector2(paneTransform.anchorMax.x, 1.0f - smoothstep(shiftAmt));
		
		if(Input.GetKey(KeyCode.Space))
		{
			shiftAmt = Mathf.Min(shiftAmt + speed * Time.deltaTime, 1);
		}
		else
		{
			shiftAmt = Mathf.Max(shiftAmt - speed * Time.deltaTime, 0);
		}
	}
}
