using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MercTrayElement : MonoBehaviour
{
	public Mercenary behaviour;
	public KeyCode binding = KeyCode.None;
	
	Transform parent;
	MercSlot occupyingSlot = null;

	NumberPanel healthPanel;
	NumberPanel attackPanel;
	NumberPanel speedPanel;
	Transform keybindDisplay;
	private float attackProgress = 0.0f;
	void Start()
	{
		behaviour.Initialize();
		parent = transform.parent;
		healthPanel = transform.GetChild(0).GetComponent<NumberPanel>();
		attackPanel = transform.GetChild(1).GetComponent<NumberPanel>();
		speedPanel = transform.GetChild(2).GetComponent<NumberPanel>();
		keybindDisplay = parent.GetChild(2);
	}

	// Update is called once per frame
	void Update () 
	{
		healthPanel.displayNum = behaviour.health;
		attackPanel.displayNum = behaviour.attack;
		speedPanel.displayNum = behaviour.attackSpeed;

		if(occupyingSlot != null)
		{
			//In battlefield state
			behaviour.Update();
			attackProgress += behaviour.attackSpeed * Time.deltaTime;

			while(attackProgress > 1)
			{
				occupyingSlot.doAttack(behaviour.attack);
				attackProgress -= 1;
			}

			if(behaviour.health <= 0)
			{
				Die();
			}
		}
	}

	public void SetKeybind(KeyCode k)
	{
		keybindDisplay.gameObject.SetActive(true);
		keybindDisplay.GetChild(1).GetComponent<Text>().text = k.ToString();
		binding = k;
	}

	public void SetNoBind()
	{
		keybindDisplay.gameObject.SetActive(false);
		binding = KeyCode.None;
	}

	public bool inPlay()
	{
		return occupyingSlot != null;
	}
	private void OccupySlot(MercSlot slot)
	{
		occupyingSlot = slot;
		slot.occupied = true;
		transform.SetParent(slot.transform);
		transform.localPosition = new Vector3(0,0,0);
	}
	public void SetDragging()
	{
		MercTrayMain.dragging = this;
		transform.SetParent(MercTrayMain.parentOfDrag);
	}
	private bool tryOccupy(MercSlot target)
	{
		if(target != null && target.ally && !target.occupied)
		{
			OccupySlot(MercTrayMain.hoveringSlot);
			return true;
		}
		return false;
	}

	public void StopDragging()
	{
		if(tryOccupy(MercTrayMain.hoveringSlot))
		{
			OccupySlot(MercTrayMain.hoveringSlot);
			return;
		}

		transform.SetParent(parent);
		transform.SetSiblingIndex(1);
		transform.localPosition = new Vector3(0,0,0);
	}

	public void Trigger(MercSlot target)
	{
		if(occupyingSlot != null)
		{
			//Do active ability
		}
		else
		{
			tryOccupy(target);
		}
	}

	public void updatePositionToMouse()
	{
		transform.position = Input.mousePosition;
	}

	public void Die()
	{
		//REMOVE KEYBINDING
	}
}
