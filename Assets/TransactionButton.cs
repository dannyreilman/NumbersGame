using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TransactionButton : Unlockable {

	public ResourceStruct cost;
	public ResourceStruct returnValue;

	public ResourceStruct buyCost;

	public ResourceStruct upgradeCost;

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

	public int maxLevel;
	
	[HideInInspector]
	public int level = 0;

	public float delay;

	private Button obj;
	public Button upgradeObj;
	public Text levelCounter;
	
	public Scrollbar bar;
	
	private bool locked = false;
	public GameObject lockedObj;

	[HideInInspector]
	public bool interactable = false;

	[HideInInspector]
	public bool upgradeable = true;

	[HideInInspector]
	public bool running = false;

	private float timepassed = 0f;
	// Use this for initialization

	private UpgradeBehaviour upgradeBehaviour;

	private RectTransform rectTrans;

	private bool repeat = false;
	private bool upgradeRepeat = false;

	public GameObject upgradeRepeatObj;
	public GameObject repeatObj;

	private RepresentStructHandler costRep;
	private RepresentStructHandler rewardRep;

	private Canvas parentCanvas;

	void Awake()
	{
		obj = transform.GetChild(0).GetComponent<Button>();
		rectTrans = GetComponent<RectTransform>();
		Transform t = transform;
		while(t.GetComponent<Canvas>() == null)
		{
			t = t.parent;
		}

		parentCanvas = t.GetComponent<Canvas>();

		costRep = transform.GetChild(5).GetComponent<RepresentStructHandler>();
		rewardRep = transform.GetChild(6).GetComponent<RepresentStructHandler>();
	}

	void Start()
	{
		upgradeBehaviour = GetComponent<UpgradeBehaviour>();
	
		costRep.Represent(cost);
		rewardRep.Represent(returnValue);

		if(upgradeBehaviour == null || upgradeBehaviour.Equals(null))
		{
			upgradeable = false;
			level = 1;
		}
	}

	void Update () 
	{
		if(!(upgradeBehaviour == null || upgradeBehaviour.Equals(null)))
		{
			upgradeObj.interactable = ResourceHandler.instance.resource >= upgradeCost && upgradeable;
			upgradeRepeatObj.SetActive(upgradeRepeat);
		}

		repeatObj.SetActive(repeat);

		obj.interactable = ResourceHandler.instance.resource >= cost && interactable;
		bar.gameObject.SetActive(running);
		bar.value = timepassed / delay;
		
		Vector2 mouseLoc = Input.mousePosition;

		if(RectTransformUtility.RectangleContainsScreenPoint(rectTrans, mouseLoc))
		{
			if(Input.GetKeyDown(KeyCode.E))
			{
				if(Input.GetKey(KeyCode.LeftShift))
				{
					if(repeat)
					{
						repeat = false;
					}
					else
					{
						repeat = true;
						DoTransaction();
					}
				}
				else
				{
					DoTransaction();
				}
			}

			if(Input.GetKeyDown(KeyCode.Q))
			{
				if(Input.GetKey(KeyCode.LeftShift))
				{
					upgradeRepeat = !upgradeRepeat;
					if(upgradeRepeat)
					{
						StartCoroutine(UpgradeCoroutine());
					}
				}
				else
				{
					Upgrade();
				}
			}

		}
	}

	private IEnumerator UpgradeCoroutine()
	{
		while(upgradeRepeat)
		{
			Upgrade();
			yield return new WaitForSeconds(0.125f);
		}
	}
	
	
	public void Upgrade()
	{
		if(ResourceHandler.instance.resource >= buyUpgradeCost && upgradeable)
		{
			ResourceHandler.instance.resource -= buyUpgradeCost;

			if(level > 0)
			{
				upgradeBehaviour.DoUpgrade();
			}

			++level;
			levelCounter.text = level.ToString();

			costRep.Represent(cost);
			rewardRep.Represent(returnValue);

			if(level > 0)
			{
				interactable = true;
			}

			if(level >= maxLevel)
			{
				upgradeable = false;
			}
		}
		else
		{
			upgradeRepeat = false;
		}
	}
	public void DoTransaction()
	{
		if(ResourceHandler.instance.resource >= cost && interactable)
		{
			ResourceHandler.instance.resource -= cost;
			interactable = false;
			upgradeable = false;
			running = true;
			StartCoroutine(WaitAndDo());
		}
		else
		{
			repeat = false;
		}
	}

	public override void Unlock()
	{
		locked = false;

		if(!(upgradeObj == null || upgradeObj.Equals(null)))
		{
			upgradeable = true;
		}
		else
		{
			upgradeable = false;
			interactable = true;
			level = 1;
		}

		lockedObj.SetActive(false);	
	}

	public override void Lock()
	{
		locked = true;
		upgradeable = false;
		lockedObj.SetActive(true);
	}
	
	private IEnumerator WaitAndDo()
	{
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
		
		ResourceHandler.instance.resource += returnValue;

		if(repeat)
		{
			DoTransaction();
		}
	}
}
