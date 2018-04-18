using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TypeReferences;

[CreateAssetMenu(fileName = "NewMerc", menuName = "ScriptObjects/Mercenary", order = 1)]
public class Mercenary : ScriptableObject 
{
	public int attack;
	public float attackSpeed;
	public int health;

    [ClassImplements(typeof(MercenaryCall))]
	public ClassTypeReference onAttack;
	private MercenaryCall onAttackObj = null;

    [ClassImplements(typeof(MercenaryCall))]
	public ClassTypeReference onTakeDamage;
	private MercenaryCall onTakeDamageObj = null;

    [ClassImplements(typeof(MercenaryCall))]
	public ClassTypeReference onUpdate;
	private MercenaryCall onUpdateObj = null;
	

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
			onUpdateObj.Call();
	}
}
