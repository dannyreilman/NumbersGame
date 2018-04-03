using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


[System.Serializable]
public enum ResourceEnum
{
	money, red, blue, green, yellow, black, white, earth
}

[System.Serializable]
public class ResourceStruct 
{
	private const int resourceCount = 8;

	[NamedArray(typeof(ResourceEnum))] 
	public int[] resourceArray = new int[resourceCount];

	public int GetResource(ResourceEnum e)
	{
		return resourceArray[(int)e];
	}

	public ResourceStruct(int money, int red, int blue, int green, int yellow, int black, int white, int earth)
	{
		resourceArray[0] = money;
		resourceArray[1] = red;
		resourceArray[2] = blue;
		resourceArray[3] = green;
		resourceArray[4] = yellow;
		resourceArray[5] = black;
		resourceArray[6] = white;
		resourceArray[7] = earth;
	}

	public ResourceStruct()
	{
		resourceArray[0] = 0;
		resourceArray[1] = 0;
		resourceArray[2] = 0;
		resourceArray[3] = 0;
		resourceArray[4] = 0;
		resourceArray[5] = 0;
		resourceArray[6] = 0;
		resourceArray[7] = 0;
	}


	public override string ToString()
	{
		string toReturn = "";
		for(int i = 0; i < resourceCount; ++i)
		{
			toReturn += Enum.GetName(typeof(ResourceEnum), i) + ": " + resourceArray[i] + " \t";
		}
		return toReturn;
	}

	public static ResourceStruct operator+ (ResourceStruct a, ResourceStruct b)
	{
		ResourceStruct returnVal = new ResourceStruct();

		for(int i = 0; i < resourceCount; ++i)
		{
			returnVal.resourceArray[i] = a.resourceArray[i] + b.resourceArray[i];
		}

		return returnVal;
	}

	public static ResourceStruct operator* (ResourceStruct a, float b)
	{
		ResourceStruct returnVal = new ResourceStruct();
				
		for(int i = 0; i < resourceCount; ++i)
		{
			returnVal.resourceArray[i] = (int)(a.resourceArray[i] * b);
		}

		return returnVal;
	}

	public static ResourceStruct operator* (ResourceStruct a, ResourceStruct b)
	{
		ResourceStruct returnVal = new ResourceStruct();

		for(int i = 0; i < resourceCount; ++i)
		{
			returnVal.resourceArray[i] = a.resourceArray[i] * b.resourceArray[i];
		}

		return returnVal;
	}

	public static ResourceStruct operator- (ResourceStruct a, ResourceStruct b)
	{
		return a + b * -1;
	}

	//If b is larger in ANY way, then a is not >= b
	public static bool operator>= (ResourceStruct a, ResourceStruct b)
	{
		for(int i = 0; i < resourceCount; ++i)
		{
			if(b.resourceArray[i] > a.resourceArray[i])
				return false;
		}

		return true;
	}

	//If a is larger in ANY way, then a is not <= b
	public static bool operator<= (ResourceStruct a, ResourceStruct b)
	{
		for(int i = 0; i < resourceCount; ++i)
		{
			if(a.resourceArray[i] > b.resourceArray[i])
				return false;
		}
		
		return true;
	}
}
