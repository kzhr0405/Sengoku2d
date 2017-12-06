using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_quest_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public bool daily;
		public string title;
		public string exp;
		public string target;
		public int amnt;
		public string titleEng;
		public string expEng;
		public string titleSChn;
		public string expSChn;
	}
}