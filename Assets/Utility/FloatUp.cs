using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatUp : MonoBehaviour {
	float velocity = 50f;
	float acceleration = -10f;
	float duration = 1.0f;
	float timePassed = 0.0f;
	Text text;
	void Awake()
	{
		text = GetComponent<Text>();
		transform.localPosition = new Vector2(
							transform.localPosition.x + (Random.value - 0.5f) * 7f, 
							transform.localPosition.y);
	}
	// Update is called once per frame
	void Update () {
		transform.localPosition = new Vector2(
							transform.localPosition.x, 
							transform.localPosition.y + velocity * Time.fixedDeltaTime);
		velocity += acceleration * Time.deltaTime;
		timePassed += Time.deltaTime;
		text.color = new Color(text.color.r,
							   text.color.g,
							   text.color.b,
							   1 - timePassed / duration);
		if(timePassed >= duration)
		{
			Destroy(gameObject);
		}
	}
}
