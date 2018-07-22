using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TypeReferences;

[CreateAssetMenu(fileName = "NewMerc", menuName = "ScriptObjects/Mercenary", order = 1)]
public class Mercenary : ScriptableObject 
{
	public int attack;
	public float attackSpeed;
	public int health;
	public Sprite sprite;

    [ClassImplements(typeof(MercenaryCall))]
	public ClassTypeReference onAttack;
	private MercenaryCall onAttackObj = null;
	public float[] onAttackArgs;

    [ClassImplements(typeof(MercenaryCall))]
	public ClassTypeReference onTakeDamage;
	private MercenaryCall onTakeDamageObj = null;
	public float[] onTakeDamageArgs;

    [ClassImplements(typeof(MercenaryCall))]
	public ClassTypeReference onUpdate;
	private MercenaryCall onUpdateObj = null;
	public float[] onUpdateArgs;

	[HideInInspector]
	public MercTrayElement home = null;

	[HideInInspector]
	public MercenaryRepresenter rep = null;
	

	public void Initialize()
	{
		if(onAttack.Type != null)
			onAttackObj = (MercenaryCall)System.Activator.CreateInstance(onAttack.Type);
		
		if(onTakeDamage.Type != null)
			onTakeDamageObj = (MercenaryCall)System.Activator.CreateInstance(onTakeDamage.Type);

		if(onUpdate.Type != null)
			onUpdateObj = (MercenaryCall)System.Activator.CreateInstance(onUpdate.Type);
	}

	public void Update()
	{
		if(onUpdateObj != null)
			onUpdateObj.Call(onUpdateArgs);
	}
	public void TakeDamage(int damage)
	{
		health -= damage;
		if(onTakeDamageObj != null && health > 0)
			onTakeDamageObj.Call(onTakeDamageArgs);
	}
	public void Attack()
	{
		if(onAttackObj != null)
			onAttackObj.Call(onAttackArgs);
	}

	public void Die()
	{
		if(rep != null)
		{
			rep.Die();
		}
	}

	public Mercenary Copy()
	{
		Mercenary toReturn = new Mercenary();

		toReturn.attack = attack;
		toReturn.attackSpeed = attackSpeed;
		toReturn.health = health;
		toReturn.onAttack = onAttack;
		toReturn.onAttackArgs = onAttackArgs;
		toReturn.onUpdate = onUpdate;
		toReturn.onUpdateArgs = onUpdateArgs;
		toReturn.onTakeDamage = onTakeDamage;
		toReturn.onTakeDamageArgs = onTakeDamageArgs;
		toReturn.sprite = sprite;

		return toReturn;
	}
}
