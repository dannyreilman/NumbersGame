using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MercTrayElement : MonoBehaviour 
{
	public static MercTrayElement dragging = null;
	public static bool draggingMouse = false;
	public static MercSlot hovering = null;
	public static Transform parentOfDrag = null;

	public Mercenary behaviour;

	RectTransform child;

	MercSlot activeSlot = null;

	NumberPanel healthPanel;
	NumberPanel attackPanel;
	NumberPanel speedPanel;

	void Start()
	{
		behaviour.Initialize();
		child = (RectTransform)transform.GetChild(0);
		healthPanel = child.GetChild(0).GetComponent<NumberPanel>();
		attackPanel = child.GetChild(1).GetComponent<NumberPanel>();
		speedPanel = child.GetChild(2).GetComponent<NumberPanel>();
	}

	// Update is called once per frame
	void Update () 
	{
		healthPanel.displayNum = behaviour.health;
		attackPanel.displayNum = behaviour.attack;
		speedPanel.displayNum = behaviour.attackSpeed;

		if(activeSlot == null)
		{
			//In tray state
			if(dragging != null && draggingMouse && (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)))
			{
				dragging.stopDragging();
				draggingMouse = false;
				dragging = null;
			}

			if(dragging == null && Input.GetMouseButtonDown(0) && RectTransformUtility.RectangleContainsScreenPoint((RectTransform)transform, Input.mousePosition))
			{
				setDragging();
				draggingMouse = true;
				updatePosition();
			}
		}
		else
		{
			//In battlefield state
			behaviour.Update();
		}
	}

	public bool inPlay()
	{
		return activeSlot != null;
	}

	public void setDragging()
	{
		dragging = this;
		child.SetParent(parentOfDrag);
	}

	public void stopDragging()
	{
		PointerEventData ped = new PointerEventData(ScreenShift.system);
		ped.position = Input.mousePosition;

		List<RaycastResult> results = new List<RaycastResult>();

		ScreenShift.raycaster.Raycast(ped, results);

		foreach(RaycastResult r in results)
		{
			MercSlot slot = r.gameObject.GetComponent<MercSlot>();

			if(slot != null && slot.occupied == false)
			{
				activeSlot = slot;
				slot.occupied = true;
				child.SetParent(slot.transform);
				child.localPosition = new Vector3(0,0,0);

				return;
			}
		}

		child.SetParent(transform);
		child.localPosition = new Vector3(0,0,0);
	}

	public void updatePosition()
	{
		child.position = Input.mousePosition;
	}
}
