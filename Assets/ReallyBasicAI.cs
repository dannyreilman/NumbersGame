using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReallyBasicAI : MonoBehaviour {
	public GameObject mercElementPrefab;
	public static float basicDelay = 5.0f;
	public Mercenary basic;
	public Mercenary strong;
	public static float strongDelay = 50.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine(SummonBasic());
		StartCoroutine(SummonStrong());
	}
	void RandomSummon(Mercenary merc)
	{
		foreach(MercSlot slot in MercSlot.enemySlots.Values)
		{
			if(!slot.occupied)
			{
				Summon(merc, slot);
				return;
			}
		}
	}
	private IEnumerator Deploy(MercSlot slot)
	{
		yield return new WaitForSeconds(0.1f);
		slot.attackProgress = 1;
	}
	void Summon(Mercenary merc, MercSlot slot)
	{
		GameObject instance = GameObject.Instantiate(mercElementPrefab, Vector3.zero, Quaternion.identity);
		MercenaryRepresenter mr = instance.GetComponent<MercenaryRepresenter>();
		slot.occupying = merc.Copy();
		mr.SetBehaviour(slot.occupying);
		mr.transform.SetParent(slot.transform);
		mr.transform.localPosition = new Vector3(0,0,0);
		slot.attackProgress = -1000.0f;
		StartCoroutine(Deploy(slot));
	}

	IEnumerator SummonBasic()
	{
		while(true)
		{
			yield return new WaitForSeconds(basicDelay);
			while(!MarketManager.instance.inMarket)
			{
				yield return new WaitForSeconds(0.01f);
			}
			RandomSummon(basic);
		}
	}
	
	IEnumerator SummonStrong()
	{
		while(true)
		{
			yield return new WaitForSeconds(strongDelay);
			while(!MarketManager.instance.inMarket)
			{
				yield return new WaitForSeconds(0.01f);
			}
			RandomSummon(strong);
		}
	}
}
