using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnAwake : MonoBehaviour {
	public int toLoad;
	// Use this for initialization
	void Awake () {
		StartCoroutine(Load());
	}
	IEnumerator Load()
	{
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene(toLoad, LoadSceneMode.Single);
	}
}
