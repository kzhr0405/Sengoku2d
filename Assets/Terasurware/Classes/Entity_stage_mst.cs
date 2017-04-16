using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_stage_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int kuniId;
		public string kuniName;
		public int id;
		public string stageName;
		public int powerTyp;
		public int LocationX;
		public int LocationY;
		public int stageMap;
		public string kuniNameEng;
		public string stageNameEng;
	}
}