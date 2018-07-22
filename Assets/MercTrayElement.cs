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
	Transform keybindDisplay;

	public int count = 1;
	void Start()
	{
		behaviour.Initialize();
		behaviour.home = this;
		GetComponent<MercenaryRepresenter>().SetBehaviour(behaviour);
		parent = transform.parent;
		keybindDisplay = parent.GetChild(2);
	}

	// Update is called once per frame
	void Update () 
	{
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
	private IEnumerator Deploy()
	{
		yield return new WaitForSeconds(0.1f);
		occupyingSlot.attackProgress = 1;
	}
	private void OccupySlot(MercSlot slot)
	{
		occupyingSlot = slot;
		slot.occupying = behaviour;
		transform.SetParent(slot.transform);
		transform.localPosition = new Vector3(0,0,0);
		//Negative so that it has to deploy before attacking
		
		occupyingSlot.attackProgress = -1000.0f;
		StartCoroutine(Deploy());
	}
	public void SetDragging()
	{
		MercTrayMain.dragging = this;
		transform.SetParent(MercTrayMain.parentOfDrag);
	}
	public bool tryOccupy(MercSlot target)
	{
		if(target != null && target.ally && !target.occupied && MarketManager.instance.inMarket)
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
			return;
		}

		ReturnToHand();
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

	public void TakeDamage(int damage)
	{
		behaviour.TakeDamage(damage);
	}
	public void Die()
	{
		BindingHandler.removeBind(binding);
		Destroy(parent.gameObject);
	}

	public void ReturnToHand()
	{
		if(occupyingSlot != null)
		{
			occupyingSlot.occupying = null;
			occupyingSlot = null;
		}
		
		transform.SetParent(parent);
		transform.SetSiblingIndex(1);
		transform.localPosition = new Vector3(0,0,0);
	}

	public void UpdateBehaviour(Mercenary merc)
	{
		if(!inPlay())
		{
			behaviour = merc.Copy();
			behaviour.Initialize();
			GetComponent<MercenaryRepresenter>().SetBehaviour(behaviour);
		}
	}
}
