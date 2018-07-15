using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingHandler : MonoBehaviour
{
	private static Dictionary<KeyCode, MercTrayElement> bindings = new Dictionary<KeyCode, MercTrayElement>();
	public static List<KeyCode> reserved = new List<KeyCode>();

	void Awake()
	{
		reserved.Add(TransactionButton.buyKey);
		reserved.Add(TransactionButton.upgradeKey);
		reserved.Add(Plate.buyKey);
		reserved.Add(ScreenShift.shiftKey);
		reserved.Add(MercTrayMain.overrideBinding);
		//Mouse press
		reserved.Add(KeyCode.Mouse0);
	}
	public static bool isTaken(KeyCode toCheck)
	{
		return bindings.ContainsKey(toCheck) && bindings[toCheck] != null;
	}
	public static void addBind(KeyCode key, MercTrayElement element)
	{
		if(bindings.ContainsKey(key) && bindings[key] != null)
		{
			if(bindings[key] == element)
				return;

			bindings[key].SetNoBind();
		}

		if(element.binding != KeyCode.None)
		{
			bindings.Remove(element.binding);
		}

		bindings[key] = element;
		element.SetKeybind(key);
	}
	public static bool validKey(KeyCode toCheck)
	{
		foreach(KeyCode code in reserved)
		{
			if(code == toCheck)
				return false;
		}

		return true;
	}

	//Returns any valid keycode that was pressed this frame, or None
	public static KeyCode getAnyPressed()
	{
		foreach(KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
		{
			if (validKey(vKey) && Input.GetKeyDown(vKey))
			{
				return vKey;
			}
		}
		return KeyCode.None;
	}

	public static List<MercTrayElement> getTriggeredElements()
	{
		List<MercTrayElement> toReturn = new List<MercTrayElement>();

		foreach(var bind in bindings)
		{
			if(bind.Value != null && Input.GetKeyDown(bind.Key))
			{
				toReturn.Add(bind.Value);
			}
		}

		return toReturn;
	} 
}
