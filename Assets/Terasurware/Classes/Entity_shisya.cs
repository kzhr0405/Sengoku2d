using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_shisya : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public string who;
		public string YesRequried1;
		public string YesRequried2;
		public string yesEffect;
		public string yesEffectValue;
		public string noEffect;
		public string noEffectValue;
	}
}