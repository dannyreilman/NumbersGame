using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchColor : MonoBehaviour {
	public ResourceEnum toMatch;
	// Use this for initialization
	void Start () {
		GetComponent<Image>().color = ResourceHandler.instance.getColor(toMatch);
	}
}
