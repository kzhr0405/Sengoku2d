using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_kuni_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int kunId;
		public string kuniName;
		public int locationX;
		public int locationY;
		public int daimyoId;
		public string naisei;
		public bool isSeaFlg;
		public string kassenStage;
		public bool isSnowFlg;
		public string kuniNameEng;
	}
}