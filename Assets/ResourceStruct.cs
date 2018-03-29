using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
	
[System.Serializable]
public enum ResourceEnum
{
	money, red, blue
}

[System.Serializable]
public struct ResourceStruct {

	public int GetResource(ResourceEnum e)
	{
		if(e == ResourceEnum.money)
		{
			return money;
		}
		else if(e == ResourceEnum.red)
		{
			return red;
		}
		else if(e == ResourceEnum.blue)
		{
			return blue;
		}
		else
		{
			Assert.AreEqual(e, ResourceEnum.blue);
			return -1;
		}
	}

	public int money;
	public int red;
	public int blue;

	public ResourceStruct(int money, int red, int blue)
	{
		this.money = money;
		this.red = red;
		this.blue = blue;
	}


	public override string ToString()
	{
		return "Money: " + money + " \tRed: " + red + " \tBlue: " + blue; 
	}

	public static ResourceStruct operator+ (ResourceStruct a, ResourceStruct b)
	{
		ResourceStruct returnVal = new ResourceStruct();
		returnVal.money = a.money + b.money;
		returnVal.red = a.red + b.red;
		returnVal.blue = a.blue + b.blue;

		return returnVal;
	}

	public static ResourceStruct operator* (ResourceStruct a, float b)
	{
		ResourceStruct returnVal = new ResourceStruct();
		returnVal.money = Mathf.CeilToInt(a.money * b);
		returnVal.red = Mathf.CeilToInt(a.red * b);
		returnVal.blue = Mathf.CeilToInt(a.blue * b);

		return returnVal;
	}

	public static ResourceStruct operator* (ResourceStruct a, ResourceStruct b)
	{
		ResourceStruct returnVal = new ResourceStruct();
		returnVal.money = a.money * b.money;
		returnVal.red = a.red * b.red;
		returnVal.blue = a.blue * b.blue;

		return returnVal;
	}

	public static ResourceStruct operator- (ResourceStruct a, ResourceStruct b)
	{
		return a + b * -1;
	}

	//If b is larger in ANY way, then a is not >= b
	public static bool operator>= (ResourceStruct a, ResourceStruct b)
	{
		if(a.money < b.money)
			return false;
		if(a.red < b.red)
			return false;
		if(a.blue < b.blue)
			return false;
		return true;
	}

	//If a is larger in ANY way, then a is not <= b
	public static bool operator<= (ResourceStruct a, ResourceStruct b)
	{
		if(a.money > b.money)
			return false;
		if(a.red > b.red)
			return false;
		if(a.red > b.red)
			return false;
		return true;
	}
}
