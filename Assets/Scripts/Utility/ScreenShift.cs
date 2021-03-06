﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScreenShift : MonoBehaviour {
	public static KeyCode shiftKey = KeyCode.Space;
	public static GraphicRaycaster raycaster;
	public static EventSystem system;
	private float shiftAmt = 0.0f;
	float speed = 10f;
	bool movingDown;
	private RectTransform paneTransform;

	float smoothstep(float t)
	{
		return (0.9f) * t*t * (2f -  t);
	}

	void Awake()
	{
		paneTransform = GetComponent<RectTransform>();
		raycaster = GetComponentInParent<GraphicRaycaster>();
		system = GetComponentInParent<EventSystem>();
	}

	void Update () 
	{
		paneTransform.anchorMin = new Vector2(paneTransform.anchorMin.x, 0.0f - smoothstep(shiftAmt));
		paneTransform.anchorMax = new Vector2(paneTransform.anchorMax.x, 1.0f - smoothstep(shiftAmt));
		
		if(Input.GetKeyDown(shiftKey))
		{
			movingDown = !movingDown;
		}
		
		if(movingDown)
		{
			shiftAmt = Mathf.Min(shiftAmt + speed * Time.deltaTime, 1);
		}
		else
		{
			shiftAmt = Mathf.Max(shiftAmt - speed * Time.deltaTime, 0);
		}
	}
}
