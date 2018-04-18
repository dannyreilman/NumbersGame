using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockHelper : MonoBehaviour {
	public void FinalizeLockInParent () {
		transform.parent.GetComponent<LockHandler>().FinalizeLock();
	}
}
