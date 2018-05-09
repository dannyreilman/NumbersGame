using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewButton", menuName = "ScriptObjects/MercBehaviour", order = 1)]
public class MercBehaviour : ScriptableObject 
{
	public Mercenary[] levels;

	public void Initialize(TransactionButton button)
	{
		
	}
}
