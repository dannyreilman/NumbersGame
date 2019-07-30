using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHandlerParent : MonoBehaviour {
	void Awake () {
		Transform[] children = new Transform[transform.childCount];

		for(int i = 0; i < transform.childCount; ++i)
		{
			children[i] = transform.GetChild(i);
			children[i].GetComponentInChildren<BarHandler>().Handle((ResourceEnum)(i+1));
		}
	}
}
