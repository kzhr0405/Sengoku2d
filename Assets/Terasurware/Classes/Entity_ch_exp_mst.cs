using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_ch_exp_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int lv;
		public int requiredMoney;
		public int totalMoney;
	}
}