using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_shisya_mst : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public bool selectFlg;
		public string name;
		public string Slot;
		public string Serihu1;
		public string Serihu2;
		public string Serihu3;
		public string YesRequried1;
		public string YesRequried2;
		public string yesEffect;
		public string yesEffectValue;
		public string noEffect;
		public string noEffectValue;
		public string OKSerihu;
		public string NGSerihu;
		public string nameEng;
		public string Serihu1Eng;
		public string Serihu2Eng;
		public string Serihu3Eng;
		public string OKSerihuEng;
		public string NGSerihuEng;
	}
}