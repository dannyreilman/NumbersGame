using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceHandler : MonoBehaviour 
{

	public static ResourceHandler instance = null;

	public Color moneyColor;
	public Color redColor;
	public Color blueColor;
	public Color greenColor;
	public Color yellowColor;
	public Color blackColor;
	public Color whiteColor;
	public Color earthColor;
	private ResourceStruct resource_internal = new ResourceStruct();
	public ResourceStruct allyResource
	{
		get
		{
			return resource_internal;
		}
		set
		{
			resource_internal = value * MarketManager.instance.GetAllyLockFactor();
		}
	}
	private ResourceStruct enemyResource_internal = new ResourceStruct();
	public ResourceStruct enemyResource
	{
		get
		{
			return enemyResource_internal;
		}
		set
		{
			enemyResource_internal = value * MarketManager.instance.GetEnemyLockFactor();
		}
	}

	public ResourceStruct startingResource;
	public int ambientMoney;
	public float ambientPeriod;

	private static ResourceStruct onedollar = ResourceStruct.GetOne(ResourceEnum.money);

	public RepresentStructHandler representer;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);

		representer = GetComponentInChildren<RepresentStructHandler>();

		if(instance == null || instance.Equals(null))
		{
			instance = this;
			StartCoroutine(AmbientRoutine());
			resource_internal = startingResource;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void Update()
	{
		representer.Represent(resource_internal, true);
		Plate.BUY_ONE_PLATE = Input.GetKeyDown(Plate.buyKey);
	}

	public Color getColor(ResourceEnum toGet)
	{
		switch(toGet)
		{
			case ResourceEnum.money:
				return moneyColor;
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
			allyResource += onedollar * ambientMoney;
		}
	}
}
