using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercSlot : MonoBehaviour 
{

	public static Dictionary<ResourceEnum, MercSlot> enemySlots = new Dictionary<ResourceEnum, MercSlot>();
	const int DIRECT_FACTOR = 5;
	public bool general;
	public MercSlot opposite;
	[HideInInspector]	
	public ResourceEnum row;
	[HideInInspector]
	public Mercenary occupying = null;
	public bool occupied
	{
		get
		{
			return occupying != null;
		}
	}
	public bool ally;
	public GameObject floatText;
	private Animator attackAnim;
	public float attackProgress;
	void Awake()
	{
		attackAnim = GetComponent<Animator>();
		if(!general)
		{
			for(int i = 0; i < transform.parent.childCount; ++i)
			{
				MercSlot comp = transform.parent.GetChild(i).GetComponent<MercSlot>();
				if(comp != null && comp != this)
				{
					opposite = comp;
				}
			}
		}
		else
		{
			opposite = null;
		}
	}

	void Start()
	{
		if(!ally && !general)
		{
			enemySlots[row] = this;
		}
	}
	void Update()
	{
		if(occupying != null)
		{
			if(!ally && !MarketManager.instance.inWave)
			{
				Destroy(transform.GetChild(0).gameObject);
				Destroy(occupying);
				occupying = null;
			}
			else
			{
				occupying.Update();

				attackProgress += occupying.attackSpeed * Time.deltaTime;

				while(attackProgress > 1)
				{
					doAttack(occupying.attack);
					occupying.Attack();
					attackProgress -= 1;
				}
				
				if(occupying.health <= 0)
				{
					occupying.Die();
					occupying = null;
				}
			}
		}
	}

	public void emitText(string text)
	{
		GameObject emmitted =  Object.Instantiate(floatText, 
												  transform.position, 
												  transform.rotation, 
												  transform);
		emmitted.GetComponent<Text>().text = text;
		emmitted.GetComponent<Text>().color = ResourceHandler.instance.getColor(row);
	}
	
	public bool doAttack(int attack)
	{
		if(opposite == null || opposite.Equals(null))
		{
			return false;
		}

		attackAnim.SetTrigger("Attack");

		if(opposite.occupied)
		{
			opposite.occupying.TakeDamage(attack);
		}
		else if(ally)
		{
			//Fail to attack if no opposite enemy
			return false;
		}
		else
		{
			int amount = ResourceHandler.instance.allyResource.GetResource(row);
			int strength = DIRECT_FACTOR * occupying.attack;
			int resourceLost = Mathf.Min(amount, strength);
			ResourceHandler.instance.allyResource -= ResourceStruct.GetOne(row) * resourceLost;
			ResourceHandler.instance.allyResource -= ResourceStruct.GetOne(ResourceEnum.money) * (strength - resourceLost);

			opposite.emitText("-" + strength);
		}
		return true;

	}
}
