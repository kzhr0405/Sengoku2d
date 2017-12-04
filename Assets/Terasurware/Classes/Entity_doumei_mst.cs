using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_doumei_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int senarioId;
		public int doumeiSrc;
		public int doumeiDst;
	}
}