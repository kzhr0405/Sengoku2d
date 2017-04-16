using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_saku_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string typ;
		public string name;
		public string effection;
		public int lv1;
		public int lv2;
		public int lv3;
		public int lv4;
		public int lv5;
		public int lv6;
		public int lv7;
		public int lv8;
		public int lv9;
		public int lv10;
		public int lv11;
		public int lv12;
		public int lv13;
		public int lv14;
		public int lv15;
		public int lv16;
		public int lv17;
		public int lv18;
		public int lv19;
		public int lv20;
		public bool shipFlg;
		public string nameEng;
		public string effectionEng;
	}
}