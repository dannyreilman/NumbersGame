using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine;

public class MercTrayMain : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public static KeyCode overrideBinding = KeyCode.LeftShift;
	public static MercTrayMain instance = null;
	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}
	public static MercTrayElement dragging = null;
	public static MercSlot hoveringSlot = null;
	public static MercTrayElement hoveringElement = null;
	public static Transform parentOfDrag = null;
	private GraphicRaycaster raycaster;
	void Start()
	{
		parentOfDrag = transform.parent.parent;
		raycaster = GetComponentInParent<GraphicRaycaster>();
	}
	private void FindHovering<T>(out T variable, PointerEventData mouse)
	{
		List<RaycastResult> results = new List<RaycastResult>();
		raycaster.Raycast(mouse, results);

		foreach(RaycastResult r in results)
		{
			if(r.gameObject.GetComponent<T>() != null)
			{
				variable = r.gameObject.GetComponent<T>();
				return;
			}
		}
		
		variable = default(T);
	}

	// Update is called once per frame
	void Update () 
	{
		PointerEventData ped = new PointerEventData(EventSystem.current);
		ped.position = Input.mousePosition;
		FindHovering<MercSlot>(out hoveringSlot, ped);
		FindHovering<MercTrayElement>(out hoveringElement, ped);

		if(dragging != null)
		{
			dragging.updatePositionToMouse();
		}

		if(hoveringElement != null)
		{
			bool overrideBindingPressed = Input.GetKey(overrideBinding);
			if(overrideBindingPressed)
			{
				KeyCode pressed = BindingHandler.getAnyPressed();
				if(pressed != KeyCode.None)
				{
					BindingHandler.addBind(pressed, hoveringElement);
				}
			}
		}

		if(hoveringSlot != null)
		{
			foreach(MercTrayElement e in BindingHandler.getTriggeredElements())
			{
				e.Trigger(hoveringSlot);
			}
		}
	}
	
	public void OnPointerDown(PointerEventData pointerEventData)
    {
		if(hoveringElement != null && pointerEventData.button == 0)
		{
			hoveringElement.SetDragging();
			hoveringElement.updatePositionToMouse();
		}
    }

	public void OnPointerUp(PointerEventData pointerEventData)
    {
        if(dragging != null && pointerEventData.button == 0)
		{
			dragging.StopDragging();
			dragging = null;
		}
    }
}

