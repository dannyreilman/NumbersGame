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
	public static DeployedMerc dragging = null;
	public static MercSlot hoveringSlot = null;
	public static MercTrayElement hoveringElement = null;
	public static Transform parentOfDrag = null;
	private GraphicRaycaster raycaster;
	public GameObject mercTrayElementPrefab;
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

			if(r.gameObject.GetComponentInParent<T>() != null)
			{
				variable = r.gameObject.GetComponentInParent<T>();
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
				if(e.waiting != null)
					e.waiting.Trigger(hoveringSlot);
			}
		}
	}
	
	public void OnPointerDown(PointerEventData pointerEventData)
  {
		if(hoveringElement != null && 
			 hoveringElement.waiting != null &&
			 pointerEventData.button == 0)
		{
			hoveringElement.waiting.SetDragging();
			hoveringElement.waiting.updatePositionToMouse();
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

	public MercTrayElement AddMercTrayElement(Mercenary toAdd)
	{
		GameObject summoned = GameObject.Instantiate(mercTrayElementPrefab, 
																								 Vector3.zero,
																								 Quaternion.identity, 
																								 transform);
		MercTrayElement elem = summoned.GetComponentInChildren<MercTrayElement>();
		elem.behaviour = toAdd.Copy();
		return elem;
	}
}

