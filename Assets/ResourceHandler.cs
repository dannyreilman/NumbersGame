using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResourceHandler : MonoBehaviour {

	public static ResourceHandler instance = null;
	public ResourceStruct resource = new ResourceStruct();

	public ResourceStruct startingGold;
	public int ambientMoney;
	public float ambientPeriod;

	private static ResourceStruct onedollar = new ResourceStruct(1, 0, 0);

	public Text text;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);

		if(instance == null || instance.Equals(null))
		{
			instance = this;
			StartCoroutine(AmbientRoutine());
			resource = startingGold;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void Update()
	{
		text.text = resource.ToString();
	}

	private IEnumerator AmbientRoutine()
	{
		while(true)
		{
			yield return new WaitForSeconds(ambientPeriod);
			resource += onedollar * ambientMoney;
		}
	}
}
