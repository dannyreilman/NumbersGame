using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *	Allows clicking buttons, keybinds, and repeats to be abstracted together
 */
public class ButtonBase 
{
	public Button button = null;
	public KeyCode code = KeyCode.None;
	public KeyCode repeatCode = KeyCode.None;
	public bool holdRepeat = false;
	public bool shiftRepeat = false;

	private Canvas parentCanvas = null;
	private RectTransform rectTrans = null;

	public delegate void VoidDelegate();
	public VoidDelegate press;

	private bool interactable = true;
	private bool shiftRepeating = false;

	//Public Interface
	public ButtonBase(Canvas parentCanvas, Button button, KeyCode code, RectTransform rectTrans, bool holdRepeat = false, bool shiftRepeat = true, KeyCode repeatCode = KeyCode.LeftShift)
	{
		this.button = button;
		this.code = code;
		this.repeatCode = repeatCode;
		this.holdRepeat = holdRepeat;
		this.shiftRepeat = shiftRepeat;
		this.parentCanvas = parentCanvas;
		this.rectTrans = rectTrans;

		button.onClick.AddListener(delegate {TryPress();});
	}

	public void Finish()
	{
		if(holdRepeat && Hovering() && Input.GetKey(code))
		{
			TryPress();
		}
		else if(shiftRepeat && shiftRepeating)
		{
			TryPress();
		}
	}
	
	public void SetInteractable(bool interactable)
	{
		button.interactable = interactable;
		this.interactable = interactable;
	}

	public void Update () 
	{
		if(Hovering() && Input.GetKeyDown(code))
		{
			if(shiftRepeat && Input.GetKey(repeatCode))
				shiftRepeating = !shiftRepeating;
			TryPress();
		}
	}
	//End Public Interface

	private void TryPress()
	{
		if(interactable)
			press();
	}	
	public bool Hovering()
	{
		Vector2 mouseLoc = Input.mousePosition;
		return RectTransformUtility.RectangleContainsScreenPoint(rectTrans, mouseLoc);
	}
}
