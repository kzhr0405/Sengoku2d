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
		public string clanName;
		public int colorR;
		public int colorG;
		public int colorB;
		public int busyoId;
		public int busyoId1;
		public int busyoId2;
		public int busyoId3;
		public int senryoku;
		public string clanNameEng;
		public string clanNameSChn;
	}
}