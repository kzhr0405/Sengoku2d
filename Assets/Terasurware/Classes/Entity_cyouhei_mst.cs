using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_cyouhei_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int No;
		public string requiredItemTyp;
		public int requiredItemQty;
		public int requiredMoney;
	}
}