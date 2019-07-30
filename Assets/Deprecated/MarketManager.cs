using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MarketManager : MonoBehaviour 
{
	public static float minWaveLength = 15.0f;
	public static float maxWaveLength = 45.0f;
	public static float timeBetweenWaves = 10.0f;
	static float BONUS_SCALE = 25.0f;
	public int waveNum = 1;
	public float baseDifficulty = 0.25f;
	public float perWaveDifficulty = 0.05f;
	public float inWaveDifficulty  = 0.01f;
	public static MarketManager instance;
	public float timeLeft;
	public bool inWave = false;

	public DamageScalingSpawner spawner;

	public float GetDifficulty()
	{
		if(inWave)
		{
			float spawningTimeLeft = timeLeft - (maxWaveLength - minWaveLength);
			if(spawningTimeLeft < 0)
				return 0;
			return baseDifficulty + perWaveDifficulty * waveNum + inWaveDifficulty * (minWaveLength - spawningTimeLeft);
		}
		else
			return 0;
	}


	// Use this for initialization
	void Awake () 
	{
		Assert.IsNull(instance);
		instance = this;
		timeLeft = 5.0f;
	}

	int GetTimeBonus()
	{
		return Mathf.RoundToInt(timeLeft * BONUS_SCALE);
	}

	public void EndRoundEarly()
	{
		int bonus = GetTimeBonus();
		ResourceHandler.instance.allyResource.resourceArray[0] += bonus;
		Debug.Log("Time Bonus: " + bonus.ToString());
		timeLeft = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		timeLeft -= Time.deltaTime;
		if(timeLeft <= 0)
		{
			inWave = !inWave;
			if(inWave)
			{
				timeLeft = maxWaveLength;
				spawner.StartWave();
			}
			else
			{
				timeLeft = timeBetweenWaves;
				spawner.StopWave();
				++waveNum;
			}
		}
	}
}
