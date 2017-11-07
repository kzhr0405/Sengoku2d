using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_item_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string itemCode;
		public string itemName;
		public string itemExp;
		public int effect;
		public string canBuy;
		public string canSell;
		public int buy;
		public int sell;
		public int itemRatio;
		public string itemNameEng;
		public string itemExpEng;
		public string itemNameSChn;
		public string itemExpSChn;
	}
}