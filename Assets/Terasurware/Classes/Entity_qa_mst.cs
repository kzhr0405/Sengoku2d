using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_qa_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string title;
		public string Exp;
		public string titleEng;
		public string ExpEng;
	}
}