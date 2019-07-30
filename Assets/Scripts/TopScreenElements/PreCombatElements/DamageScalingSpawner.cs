using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScalingSpawner : MonoBehaviour {
	static float SCALING_FACTOR = 0.001f;
	public GameObject mercElementPrefab;


	public SpawnableMercenary[] possibleSpawns;
	public float swarmingFactor = 1.0f;
	Mercenary GenerateMercenary(SpawnableMercenary smerc)
	{
		Mercenary toReturn = smerc.merc.Copy();
		float difficulty = SCALING_FACTOR * MarketManager.instance.GetDifficulty();
		toReturn.attack += Mathf.RoundToInt(difficulty * smerc.atkScaling);
		toReturn.health += Mathf.RoundToInt(difficulty * smerc.hlthScaling);
		toReturn.attackSpeed += Mathf.RoundToInt(difficulty * smerc.spdScaling);
		return toReturn;
	}


	Queue<Mercenary> queue = new Queue<Mercenary>();
	Coroutine spawningRoutine = null;

	[SerializeField]
	float budget = 0.0f;

	public void StartWave()
	{
		spawningRoutine = StartCoroutine(SpawningRoutine());
		budget = 0.0f;
	}

	public void StopWave()
	{
		if(spawningRoutine != null)
		{
			StopCoroutine(spawningRoutine);
			spawningRoutine = null;
		}
		queue.Clear();
		budget = 0.0f;
	}

	SpawnableMercenary ChoseSpawnableMercenary()
	{
		float[] weights = new float[possibleSpawns.Length];
		float totalWeight = 0.0f;
		for(int i = 0; i < possibleSpawns.Length; ++i)
		{
			weights[i] = Mathf.Pow(possibleSpawns[i].cost + possibleSpawns[i].costScaling * SCALING_FACTOR * MarketManager.instance.GetDifficulty(), -1 * swarmingFactor);
			totalWeight += weights[i];
		}
		float chosen = Random.Range(0.0f, totalWeight);
		int index = 0;
		while(chosen > weights[index])
		{
			chosen -= weights[index];
			++index;
		}
		return possibleSpawns[index];
	}

	IEnumerator SpawningRoutine()
	{
		while(true)
		{
			SpawnableMercenary chosen = ChoseSpawnableMercenary();
			float cost = chosen.cost + chosen.costScaling * SCALING_FACTOR * MarketManager.instance.GetDifficulty();
			while(budget < cost)
			{
				float currentBudget = MarketManager.instance.GetDifficulty();
				if(currentBudget == 0)
				{
					MarketManager.instance.EndRoundEarly();
					yield break;
				}
				else
				{
					budget += Time.deltaTime * currentBudget;
				}
				yield return 0;
			}
			budget -= cost;
			queue.Enqueue(GenerateMercenary(chosen));
		}
	}

	void Update()
	{
		while(queue.Count > 0 && RandomSummon(queue.Peek()))
		{
			queue.Dequeue();
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

	bool RandomSummon(Mercenary merc)
	{
		foreach(MercSlot slot in MercSlot.enemySlots.Values)
		{
			if(!slot.occupied)
			{
				Summon(merc, slot);
				return true;
			}
		}
		return false;
	}
}
