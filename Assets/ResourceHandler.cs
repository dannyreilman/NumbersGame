using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHandler : MonoBehaviour 
{

	public static ResourceHandler instance = null;

	public Color redColor;
	public Color blueColor;
	public Color greenColor;
	public Color yellowColor;
	public Color blackColor;
	public Color whiteColor;
	public Color earthColor;

	public ResourceStruct resource = new ResourceStruct();

	public int startingGold;
	public int ambientMoney;
	public float ambientPeriod;

	private static ResourceStruct onedollar = new ResourceStruct(1, 0, 0, 0, 0, 0, 0, 0);

	public Text text;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);

		if(instance == null || instance.Equals(null))
		{
			instance = this;
			StartCoroutine(AmbientRoutine());
			resource = onedollar * startingGold;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void Update()
	{
		text.text = resource.ToString();

		Plate.BUY_ONE_PLATE = Input.GetKeyDown(KeyCode.R);
	}

	public Color getColor(ResourceEnum toGet)
	{
		switch(toGet)
		{
			case ResourceEnum.red:
				return redColor;
			case ResourceEnum.blue:
				return blueColor;
			case ResourceEnum.green:
				return greenColor;
			case ResourceEnum.yellow:
				return yellowColor;
			case ResourceEnum.black:
				return blackColor;
			case ResourceEnum.white:
				return whiteColor;
			case ResourceEnum.earth:
				return earthColor;
			default:
				Debug.Log(toGet + "Not implemented");
				return new Color(0,0,0,0);
		}
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
