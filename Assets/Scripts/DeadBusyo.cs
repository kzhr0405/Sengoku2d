using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeadBusyo : MonoBehaviour {
	
	public List<BusyoSenkou> deadBusyo = new List<BusyoSenkou>();

	public void AddDeadBusyo (GameObject obs){
		deadBusyo.Add(new BusyoSenkou(int.Parse(obs.name.Replace("(Clone)","")), obs.GetComponent<Kunkou>().kunkou));
	}
}
