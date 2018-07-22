using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercSlot : MonoBehaviour 
{

	public static Dictionary<ResourceEnum, MercSlot> enemySlots = new Dictionary<ResourceEnum, MercSlot>();
	const int DIRECT_FACTOR = 5;
	const int LOCK_FACTOR = 2;
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
				if(occupying.home != null)
				{
					occupying.home.Die();
				}

				occupying.Die();
				occupying = null;
			}

			if(!MarketManager.instance.inMarket)
			{
				if(occupying.home == null)
				{
					occupying.Die();
				}
				else
				{
					occupying.home.ReturnToHand();
				}
				occupying = null;
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
		else
		{
			int allyAmt = ResourceHandler.instance.allyResource.GetResource(row);
			int enemyAmt = ResourceHandler.instance.enemyResource.GetResource(row);
			if(MarketManager.instance.locks[(int)row] == (ally ? 1 : -1))
			{
				int strength = LOCK_FACTOR * occupying.attack;
				if(ally)
				{
					ResourceHandler.instance.allyResource += ResourceStruct.GetOne(row) * strength;
				}
				else
				{
					ResourceHandler.instance.enemyResource += ResourceStruct.GetOne(row) * strength;
				}
				emitText("+" + strength);
			}
			else 
			{
				if((ally ? enemyAmt : allyAmt) <= 0)
				{
					MarketManager.instance.Lock(row, ally);
				}
				else
				{
					int strength = DIRECT_FACTOR * occupying.attack;
					if(ally)
					{
						ResourceHandler.instance.enemyResource -= ResourceStruct.GetOne(row) * strength;
					}
					else
					{
						ResourceHandler.instance.allyResource -= ResourceStruct.GetOne(row) * strength;
					}
					opposite.emitText("-" + strength);
				}
			}
		}
		return true;

	}
}
