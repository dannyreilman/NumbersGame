 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeployedMerc : MonoBehaviour
{
	public Mercenary behaviour
	{
		get
		{
			return behaviour_internal;
		}
		set
		{
			behaviour_internal = value;
			behaviour_internal.home = this;
			behaviour_internal.Initialize();
			GetComponent<MercenaryRepresenter>().SetBehaviour(behaviour_internal);
		}
	}

	[SerializeField]
	Mercenary behaviour_internal;

	public MercTrayElement parent;
	MercSlot occupyingSlot = null;

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
		if(target != null && target.ally && !target.occupied)
		{
            parent.waiting = null;
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

		transform.SetParent(parent.transform);
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

	public void TakeDamage(int damage)
	{
		behaviour.TakeDamage(damage);
	}

	public void Die()
	{
        Debug.Log("Death");
		parent.Respawn();
		Destroy(gameObject);
	}

	public void ReturnToHand()
	{
		if(occupyingSlot != null)
		{
			occupyingSlot.occupying = null;
			occupyingSlot = null;
		}
		++parent.count;
        Destroy(gameObject);
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
