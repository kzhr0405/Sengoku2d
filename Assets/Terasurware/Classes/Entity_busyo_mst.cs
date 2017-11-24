using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_busyo_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public string rank;
		public string heisyu;
		public int basehp;
		public int baseatk;
		public int basedfc;
		public int hp;
		public int atk;
		public int dfc;
		public int spd;
		public int minHp;
		public int minAtk;
		public int minDfc;
		public int minSpd;
		public int ship;
		public int senpou_id;
		public int saku_id;
		public int GacyaFree;
		public int GacyaTama;
		public int daimyoId;
		public int daimyoHst;
		public int daimyoId1;
		public int daimyoHst1;
		public int daimyoId2;
		public int daimyoHst2;
		public int daimyoId3;
		public int daimyoHst3;
		public string nameEng;
		public string nameSChn;
	}
}