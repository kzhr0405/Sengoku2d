using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_stageLink_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int kuniId;
		public string Link;
	}
}