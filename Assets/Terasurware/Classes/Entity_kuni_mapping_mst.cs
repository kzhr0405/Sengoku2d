using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_kuni_mapping_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int Souce;
		public int Open;
		public string linkStageId;
		public int ArrowX;
		public int ArrowY;
	}
}