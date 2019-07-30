using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class TransactionButton : Unlockable {

	public static KeyCode upgradeKey = KeyCode.Q;
	public static KeyCode buyKey = KeyCode.E;

	//This initializes the hideininspector values below
	public ButtonBehaviour behaviour;
	
	[HideInInspector]
	public ResourceStruct cost;

	[HideInInspector]
	public ResourceStruct returnValue;
	
	[HideInInspector]
	public ResourceStruct buyCost;

	[HideInInspector]
	public ResourceStruct upgradeCost;

	[HideInInspector]
	public int maxLevel;
	
	[HideInInspector]
	public int level = 0;

	[HideInInspector]
	public float delay;

	private ResourceStruct buyUpgradeCost
	{
		get
		{
			if(level > 0)
			{
				return upgradeCost;
			}
			else
			{
				return buyCost;
			}
		}

		set
		{
			if(level > 0)
			{
				upgradeCost = value;
			}
			else
			{
				buyCost = value;
			}
		}
	}

	private Button transactionButton;
	private Button upgradeButton;
	private ButtonBase transactionObj = null;
	private ButtonBase upgradeObj = null;
	private Text levelCounter;
	
	private Scrollbar bar;
	
	private bool locked = false;
	private GameObject lockedObj;

	private bool interactable = false;

	private bool upgradeable = true;
	private bool upgradeCooldown = false;

	private bool running = false;

	private float timepassed = 0f;
	// Use this for initialization

	private UpgradeBehaviour upgradeBehaviour;

	private RepresentStructHandler costRep;
	private RepresentStructHandler rewardRep;
	private RepresentStructHandler upgradeRep;
	private Canvas parentCanvas;

	void Awake()
	{		
		upgradeRep = transform.GetChild(0).GetComponent<RepresentStructHandler>();

		upgradeButton = transform.GetChild(1).GetChild(0).GetComponent<Button>();
		levelCounter = transform.GetChild(1).GetChild(1).GetComponentInChildren<Text>();

		transactionButton = transform.GetChild(2).GetComponent<Button>();
		bar = transform.GetChild(2).GetChild(0).GetComponentInChildren<Scrollbar>();
		lockedObj = transform.GetChild(2).GetChild(1).gameObject;

		costRep = transform.GetChild(3).GetChild(0).GetComponent<RepresentStructHandler>();
		rewardRep = transform.GetChild(3).GetChild(1).GetComponent<RepresentStructHandler>();

		parentCanvas = GetComponentInParent<Canvas>();
	}

	void Start()
	{
		behaviour.Initialize(this);
		upgradeRep.Represent(upgradeCost);

		upgradeBehaviour = GetComponent<UpgradeBehaviour>();

		transactionObj = new ButtonBase(parentCanvas, transactionButton, buyKey, (RectTransform) transform);
		
		if(upgradeBehaviour == null || upgradeBehaviour.Equals(null))
		{
			upgradeObj = null;
		}
		else
		{
			upgradeObj = new ButtonBase(parentCanvas, upgradeButton, upgradeKey, (RectTransform) transform);
		}
		
		if(upgradeObj != null)
		{
			upgradeObj.press += Upgrade;
		}

		transactionObj.press += DoTransaction;

		interactable = level > 0 && !locked;
		levelCounter.text = level.ToString();
	
		upgradeBehaviour = GetComponent<UpgradeBehaviour>();
	
		costRep.Represent(cost);
		rewardRep.Represent(returnValue);

		if(upgradeObj == null)
		{
			upgradeable = false;
			upgradeButton.transform.parent.gameObject.SetActive(false);
			
			interactable = false;
			levelCounter.transform.parent.gameObject.SetActive(false);
		}
	}
	
	private void UpdateInteractables()
	{
		if(upgradeObj != null)
		{
			upgradeObj.SetInteractable(ResourceHandler.instance.allyResource >= upgradeCost && upgradeable && !upgradeCooldown);
		}

		transactionObj.SetInteractable(ResourceHandler.instance.allyResource >= cost && interactable && !running);
	}
	void Update () 
	{
		if(Input.GetKeyUp(upgradeKey) && upgradeObj != null)
		{
			upgradeCooldown = false;
			StopCoroutine("UpgradeCoroutine");
			UpdateInteractables();
			upgradeObj.Finish();
		}
		else
		{
			UpdateInteractables();
		}

		if(upgradeObj != null)
		{
			upgradeObj.Update();
		}

		transactionObj.Update();

		bar.gameObject.SetActive(running);
		bar.value = timepassed / delay;
	}

	private float initialUpgradeDelay = 0.5f;
	private float upgradeDelay = 0.5f;

	private IEnumerator UpgradeDelayChange()
	{
		upgradeDelay /= 1.4f;
		yield return new WaitForSeconds(0.5f);
		upgradeDelay = initialUpgradeDelay;
	}

	private IEnumerator UpgradeCoroutine(ButtonBase upgradeObj)
	{
		StopCoroutine("UpgradeDelayChange");
		StartCoroutine("UpgradeDelayChange");
		
		upgradeCooldown = true;
		yield return new WaitForSeconds(upgradeDelay);
		upgradeCooldown = false;
		UpdateInteractables();
		upgradeObj.Finish();
	}
	
	public void Upgrade()
	{
		ResourceHandler.instance.allyResource -= buyUpgradeCost;

		UpdateInteractables();
		if(level > 0)
		{
			upgradeBehaviour.DoUpgrade();
		}

		++level;
		levelCounter.text = level.ToString();

		costRep.Represent(cost);
		rewardRep.Represent(returnValue);
		upgradeRep.Represent(upgradeCost);

		if(level > 0)
		{
			interactable = true;
		}

		if(level >= maxLevel)
		{
			upgradeable = false;
		}
		
		StartCoroutine("UpgradeCoroutine", upgradeObj);

	}

	public void DoTransaction()
	{
		ResourceHandler.instance.allyResource -= cost;
		UpdateInteractables();
		interactable = false;
		upgradeable = false;
		StartCoroutine(WaitAndDo());
	}

	public override void Unlock()
	{
		locked = false;

		if(upgradeBehaviour == null)
		{			
			interactable = true;
			level = 1;
			upgradeable = false;
		}
		else
		{
			upgradeable = true;
		}

		if(lockedObj != false)
			lockedObj.SetActive(false);	
	}

	public override void Lock()
	{
		locked = true;
		upgradeable = false;
		if(lockedObj != false)
			lockedObj.SetActive(true);
	}
	
	private IEnumerator WaitAndDo()
	{
		running = true;
		timepassed = 0;
		while(timepassed <= delay)
		{
			yield return new WaitForSeconds(0.01f);
			timepassed += 0.01f;
		}
		timepassed = 0;

		running = false;
		interactable = true;

		if(level < maxLevel)
			upgradeable = true;
		
		ResourceHandler.instance.allyResource += returnValue;
		UpdateInteractables();
		if(upgradeObj != null)
			upgradeObj.Finish();
		transactionObj.Finish();
	}
	
	public void SetName(string s)
	{
		transform.GetChild(2).GetComponentInChildren<Text>().text = s;
	}
}

