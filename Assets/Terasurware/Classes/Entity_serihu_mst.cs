using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_serihu_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public string rank;
		public string heisyu;
		public string touyouMsg;
		public string tsuihouMsg;
		public string senpouMsg;
		public string nameEng;
		public string touyouMsgEng;
		public string tsuihouMsgEng;
		public string senpouMsgEng;
	}
}