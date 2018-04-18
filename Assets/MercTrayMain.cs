using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MercTrayMain : MonoBehaviour 
{
	public static MercTrayMain instance = null;

	static KeyCode[] bindingArray = {
									KeyCode.Alpha1, 
									KeyCode.Alpha2, 
									KeyCode.Alpha3, 
									KeyCode.Alpha4, 
									KeyCode.Alpha5, 
									KeyCode.Alpha6, 
									KeyCode.Alpha7, 
									KeyCode.Alpha8,
									KeyCode.Alpha9,
									KeyCode.Alpha0
									};
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
	void Start()
	{
		MercTrayElement.parentOfDrag = transform.parent.parent;
	}

	int dragging = -1;

	// Update is called once per frame
	void Update () 
	{
		//dragging mouse is set when setDragging is called
		if(!MercTrayElement.draggingMouse)
		{
			if(MercTrayElement.dragging == null)
			{
				for(int i = 0; i < 10 && i < transform.childCount; ++i)
				{
					if(Input.GetKeyDown(bindingArray[i]))
					{
						MercTrayElement temp = transform.GetChild(i).GetComponent<MercTrayElement>();
						if(temp != null && !temp.inPlay())
						{
							temp.setDragging();
							temp.stopDragging();
							MercTrayElement.dragging = null;
						}
					}
				}

			}
		}

		if(MercTrayElement.dragging != null)
		{
			MercTrayElement.dragging.updatePosition();
		}
	}
}
