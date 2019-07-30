using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MercTrayElement : MonoBehaviour
{
	public GameObject deployedMercPrefab;
	public MercenaryButton assignedButton;
	public KeyCode binding = KeyCode.None;
	MercSlot occupyingSlot = null;
	public DeployedMerc waiting = null;
	public Mercenary behaviour;
	Transform keybindDisplay;
	Slider respawnDisplay;

	int count_internal = 0;
	public int count
	{
		get
		{
			return count_internal;
		}
		set
		{
			count_internal = value;
			OnCountChange();
		}
	}

	void OnCountChange()
	{
		if(waiting == null && count > 0)
		{
			GameObject created = GameObject.Instantiate(deployedMercPrefab, transform);
			waiting = created.GetComponent<DeployedMerc>();
			waiting.behaviour = behaviour.Copy();
			waiting.parent = this;
		}
		else if (waiting != null && count <= 0)
		{
			Destroy(waiting.gameObject);
			waiting = null;
		}
	}

	void Start()
	{
		keybindDisplay = transform.GetChild(1);
		respawnDisplay = transform.GetChild(2).GetComponent<Slider>();
		//Triggers spawning of deployedMerc
		count = 1;
	}

	void Update()
	{
		float respawnProgress = assignedButton.GetRespawnPercent();
		if(respawnProgress < 0)
		{
			respawnDisplay.gameObject.SetActive(false);
		}
		else
		{
			respawnDisplay.gameObject.SetActive(true);
			respawnDisplay.normalizedValue = respawnProgress;
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

	public void Respawn()
	{
		assignedButton.Respawn();
	}

	public void UpdateBehaviour(Mercenary merc)
	{
		behaviour = merc.Copy();
		if(waiting != null)
		{
			waiting.UpdateBehaviour(behaviour.Copy());
		}
	}
}
