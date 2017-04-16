using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_icon_exp_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string exp;
		public string expEng;
	}
}