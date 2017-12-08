using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_kanni_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int No;
		public string Kanni;
		public string Ikai;
		public string EffectLabel;
		public string EffectTarget;
		public int Effect;
		public string EffectUnit;
		public string TargetKuni;
		public int NeedKuniQty;
		public int SyoukaijyoRank;
		public string IkaiEng;
		public string EffectLabelEng;
		public string KanniSChn;
		public string IkaiSChn;
		public string EffectLabelSChn;
	}
}