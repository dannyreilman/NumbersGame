using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercenaryButton : Unlockable
{
	public MercBehaviour behaviour;
	[HideInInspector]
	public ResourceStruct upgradeCost;
	[HideInInspector]
	public int maxCount = 1;
	[HideInInspector]
	public Image icon;

	private Button upgradeButton;
	private ButtonBase upgradeObj = null;
	private Canvas parentCanvas;
	private RepresentStructHandler upgradeRep;
	private int level = 0;
	private Text levelCounter;

	[HideInInspector]
	public MercTrayElement trayElement = null;

	bool locked = false;

	public List<float> respawnTimes = new List<float>();
	public override void Unlock()
	{
		locked = false;
	}

	public override void Lock()
	{
		locked = true;
	}

	void Awake()
	{
		levelCounter = transform.GetChild(4).GetComponentInChildren<Text>();
		levelCounter.text = level.ToString();

		upgradeButton = transform.GetChild(2).GetComponent<Button>();
		parentCanvas = GetComponentInParent<Canvas>();

		upgradeRep = transform.GetChild(1).GetChild(0).GetComponent<RepresentStructHandler>();

		icon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
	}

	// Use this for initialization
	void Start () {
		behaviour.Initialize(this);
		
		upgradeRep.Represent(upgradeCost);

		upgradeObj = new ButtonBase(parentCanvas, upgradeButton, TransactionButton.upgradeKey, (RectTransform) transform, false);
		upgradeObj.press += Upgrade;
	}

	private void UpdateInteractables()
	{
		upgradeObj.SetInteractable(ResourceHandler.instance.allyResource >= upgradeCost && level < behaviour.levels.Length && !locked);
	}

	void Upgrade()
	{
		ResourceHandler.instance.allyResource -= upgradeCost;
		++level;

		if(trayElement == null || trayElement.Equals(null))
		{
			trayElement = MercTrayMain.instance.AddMercTrayElement(behaviour.levels[level-1]);
			trayElement.assignedButton = this;
			trayElement.count = maxCount;
		}
		trayElement.UpdateBehaviour(behaviour.levels[level-1]);

		levelCounter.text = level.ToString();
		upgradeRep.Represent(upgradeCost);
		UpdateInteractables();
	}

	public void Respawn()
	{
		respawnTimes.Add(behaviour.levels[level-1].respawnTime);
	}

	// Update is called once per frame
	void Update ()
	{
		UpdateInteractables();
		upgradeObj.Update();
		for(int i = respawnTimes.Count - 1; i >= 0; --i)
		{
			respawnTimes[i] -= Time.deltaTime;
			if(respawnTimes[i] < 0)
			{
				respawnTimes.RemoveAt(i);
				++trayElement.count;
			}
		}	
	}

	public float GetRespawnPercent()
	{
		if(respawnTimes.Count == 0)
			return -1.0f;

		float minTime = float.MaxValue;
		foreach (float timeLeft in respawnTimes)
		{
			if (timeLeft/behaviour.levels[level-1].respawnTime < minTime)
			{
				minTime = timeLeft/behaviour.levels[level-1].respawnTime;
			}
		}
		return Mathf.Max(0.0f, 1 - minTime);
	}
}
