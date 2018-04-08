using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepresentStructHandler : MonoBehaviour 
{
	private Transform[] children;

	// Use this for initialization
	void Awake () {
		children = new Transform[transform.childCount];
		for(int i = 0; i < transform.childCount; ++i)
		{
			children[i] = transform.GetChild(i);
			children[i].GetComponentInChildren<MatchColor>().toMatch = (ResourceEnum)i;
		}
	}
	
	public void Represent(ResourceStruct rs, bool showAll = false)
	{
		for(int i = 0; i < children.Length; ++i)
		{
			children[i].GetComponentInChildren<Text>().text = rs.resourceArray[i].ToString();

			if(showAll || rs.resourceArray[i] > 0)
			{
				children[i].gameObject.SetActive(true);
			}
			else
			{
				children[i].gameObject.SetActive(false);
			}
		}
	}
}
