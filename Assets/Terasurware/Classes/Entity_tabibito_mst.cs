using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_tabibito_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int Id;
		public string Typ;
		public int GrpID;
		public string Grp;
		public string Name;
		public string Rank;
		public string Exp;
		public string ItemMst;
		public int ItemMstId;
		public int ItemQty;
		public string GrpEng;
		public string NameEng;
		public string ExpEng;
	}
}