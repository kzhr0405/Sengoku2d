using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_quest_count_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public bool daily;
		public int grp;
		public string title;
		public string exp;
		public string target;
		public string criteriaTyp;
		public int criteria;
		public int amnt;
		public string titleEng;
		public string expEng;
	}
}