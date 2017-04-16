using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_kuni_lv_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int lv;
		public int requiredExp;
		public int totalExp;
		public int busyoJinkeiLimit;
		public int busyoStockLimit;
	}
}