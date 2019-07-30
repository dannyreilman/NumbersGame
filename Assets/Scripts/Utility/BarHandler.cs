using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarHandler : MonoBehaviour 
{
	static int maxAmount = -1;
	ResourceEnum toHandle;
	
	private RectTransform friendlyBar;

	public MercSlot enemy;
	public MercSlot ally;


	public void Handle(ResourceEnum toHandle_in)
	{
		toHandle = toHandle_in;
		ally.row = toHandle;
		enemy.row = toHandle;
	}

	void Awake()
	{
		friendlyBar = transform.GetChild(0) as RectTransform;	
	}

	void Start()
	{
		friendlyBar.GetComponent<Image>().color = ResourceHandler.instance.getColor(toHandle);
	}

	// Update is called once per frame
	void Update () 
	{
		if(maxAmount < 0)
		{
			for(int i = 1; i < ResourceStruct.resourceCount; ++i)
			{
				int foundResource = ResourceHandler.instance.allyResource.resourceArray[i];
				if(maxAmount < foundResource)
					maxAmount = foundResource;
			}
		}
		float amount = ResourceHandler.instance.allyResource.GetResource(toHandle);
		friendlyBar.anchorMax = new Vector2(1.0f, amount / maxAmount);
	}

	void LateUpdate()
	{
		maxAmount = -1;
	}
}
