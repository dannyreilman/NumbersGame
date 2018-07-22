using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MercenaryButton : MonoBehaviour {

	public static float DEATH_FACTOR = 2.0f;
	public MercBehaviour behaviour;
	[HideInInspector]
	public ResourceStruct upgradeCost;
	[HideInInspector]
	public ResourceStruct buyCost;
	[HideInInspector]
	public int maxCount = 1;
	[HideInInspector]
	public Image icon;

	private Button upgradeButton;
	private Button buyButton;
	private ButtonBase upgradeObj = null;
	private ButtonBase buyObj = null;
	private Canvas parentCanvas;
	private RepresentStructHandler upgradeRep;
	private RepresentStructHandler buyRep;
	private int level = 1;
	private Text levelCounter;

	[HideInInspector]
	public MercTrayElement trayElement = null;
	
	void Awake()
	{
		levelCounter = transform.GetChild(4).GetComponentInChildren<Text>();
		levelCounter.text = level.ToString();

		upgradeButton = transform.GetChild(2).GetChild(0).GetComponent<Button>();
		buyButton = transform.GetChild(2).GetChild(1).GetComponent<Button>();
		parentCanvas = GetComponentInParent<Canvas>();

		upgradeRep = transform.GetChild(1).GetChild(0).GetComponent<RepresentStructHandler>();
		buyRep = transform.GetChild(1).GetChild(1).GetComponent<RepresentStructHandler>();

		icon = transform.GetChild(0).GetChild(0).GetComponent<Image>();
	}

	// Use this for initialization
	void Start () {
		behaviour.Initialize(this);
		
		upgradeRep.Represent(upgradeCost);
		buyRep.Represent(buyCost);

		upgradeObj = new ButtonBase(parentCanvas, upgradeButton, TransactionButton.upgradeKey, (RectTransform) transform, false);
		upgradeObj.press += Upgrade;
		buyObj = new ButtonBase(parentCanvas, buyButton, TransactionButton.buyKey, (RectTransform) transform, false);
		buyObj.press += Buy;
	}

	private void UpdateInteractables()
	{
		upgradeObj.SetInteractable(ResourceHandler.instance.allyResource >= upgradeCost && level < behaviour.levels.Length);
		buyObj.SetInteractable(ResourceHandler.instance.allyResource >= buyCost && (trayElement == null || trayElement.Equals(null) || trayElement.count < maxCount));
	}

	void Upgrade()
	{
		ResourceHandler.instance.allyResource -= upgradeCost;
		++level;

		if(!(trayElement == null || trayElement.Equals(null)))
		{
			trayElement.UpdateBehaviour(behaviour.levels[level-1]);
		}

		levelCounter.text = level.ToString();
		upgradeRep.Represent(upgradeCost);
		buyRep.Represent(buyCost);
		UpdateInteractables();
	}

	void Buy()
	{
		ResourceHandler.instance.allyResource -= buyCost;
		if(trayElement == null || trayElement.Equals(null))
		{
			trayElement = MercTrayMain.instance.AddMercTrayElement(behaviour.levels[level-1]);
		}
		else
		{
			++trayElement.count;
		}

		upgradeRep.Represent(upgradeCost);
		buyRep.Represent(buyCost);
		UpdateInteractables();
	}

	// Update is called once per frame
	void Update () {
		UpdateInteractables();
		upgradeObj.Update();
		buyObj.Update();
	}
}
