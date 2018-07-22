using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercenaryRepresenter : MonoBehaviour {

	NumberPanel healthPanel;
	NumberPanel attackPanel;
	NumberPanel speedPanel;
	Image icon;
	Mercenary behaviour = null;

	// Use this for initialization
	void Awake () {
		healthPanel = transform.GetChild(0).GetComponent<NumberPanel>();
		attackPanel = transform.GetChild(1).GetComponent<NumberPanel>();
		speedPanel = transform.GetChild(2).GetComponent<NumberPanel>();
		icon = GetComponent<Image>();
	}
	
	public void SetBehaviour(Mercenary behaviour_in)
	{
		behaviour = behaviour_in;
		icon.sprite = behaviour.sprite;
		behaviour.rep = this;
	}

	// Update is called once per frame
	void Update () {
		healthPanel.displayNum = behaviour.health;
		attackPanel.displayNum = behaviour.attack;
		speedPanel.displayNum = behaviour.attackSpeed;
	}
	public void Die()
	{
		Destroy(gameObject);
	}
}
