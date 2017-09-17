using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_daimyo_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int daimyoId;
		public string daimyoName;
		public int colorR;
		public int colorG;
		public int colorB;
		public int busyoId;
		public int senryoku;
		public string daimyoNameEng;
		public string clanName;
		public string clanNameEng;
	}
}