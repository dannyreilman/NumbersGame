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
	public bool holdRepeat = false;
	private Canvas parentCanvas = null;
	private RectTransform rectTrans = null;

	public delegate void VoidDelegate();
	public VoidDelegate press;

	private bool interactable = true;

	//Public Interface
	public ButtonBase(Canvas parentCanvas, Button button, KeyCode code, RectTransform rectTrans, bool holdRepeat = true)
	{
		this.button = button;
		this.code = code;
		this.holdRepeat = holdRepeat;
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
	}
	
	public void SetInteractable(bool interactable)
	{
		button.interactable = interactable;
		this.interactable = interactable;

	}

	public void Update () 
	{
		if(Hovering() && Input.GetKeyDown(code))
			TryPress();
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
