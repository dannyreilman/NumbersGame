using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberPanel : MonoBehaviour {
	public float displayNum;
	Text txt;

	void Start () 
	{
		txt = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		txt.text = displayNum.ToString();
	}
}
