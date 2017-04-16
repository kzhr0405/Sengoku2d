using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GacyaSimulator : MonoBehaviour {

	public int busyo0 = 0;
	public int busyo1 = 0;
	public int busyo2 = 0;
	public int busyo3 = 0;
	public int busyo4 = 0;
	public int busyo5 = 0;
	public int busyo6 = 0;
	public int busyo7 = 0;
	public int busyo8 = 0;
	public int busyo9 = 0;
	public int busyo10 = 0;
	public int busyo11 = 0;
	public int busyo12 = 0;
	public int busyo13 = 0;
	public int busyo14 = 0;
	public int busyo15 = 0;
	public int busyo16 = 0;
	public int busyo17 = 0;
	public int busyo18 = 0;
	public int busyo19 = 0;
	public int busyo20 = 0;

	void Start () {

		for (int i=0; i<10000; i++) {
			int[] hitBusyo = gacyaFree();
			//int[] hitBusyo = gacyaMoney();

			busyoCounter(hitBusyo,0);
			busyoCounter(hitBusyo,1);
			busyoCounter(hitBusyo,2);
		}

	}

	public int[] gacyaFree(){
		int[] hitBusyo = new int[3];
		
		List<int> busyoListByWeight = new List<int> ();
		
		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		
		for(int i=0; i<busyoMst.param.Count; i++){
			int weight = busyoMst.param [i].GacyaFree;
			int busyoId = busyoMst.param[i].id;
			
			for(int j=0; j<weight; j++){
				busyoListByWeight.Add (busyoId);
			}
		}
		
		//Get 3 by Randam without dupplication
		for(int k=0; k<3; k++){
			int rdmId = UnityEngine.Random.Range(0,busyoListByWeight.Count);
			hitBusyo[k] = busyoListByWeight[rdmId];
		}

		return hitBusyo;
	}

	public int[] gacyaMoney(){
		int[] hitBusyo = new int[3];
		
		List<int> busyoListByWeight = new List<int> ();
		
		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;
		
		for(int i=0; i<busyoMst.param.Count; i++){
			int weight = busyoMst.param [i].GacyaTama;
			int busyoId = busyoMst.param[i].id;
			
			for(int j=0; j<weight; j++){
				busyoListByWeight.Add (busyoId);
			}
		}
		
		//Get 3 by Randam without dupplication
		for(int k=0; k<3; k++){
			int rdmId = UnityEngine.Random.Range(0,busyoListByWeight.Count);
			hitBusyo[k] = busyoListByWeight[rdmId];
		}
		
		return hitBusyo;
	}


	public void busyoCounter(int[] gacyaRst, int count){
		if (gacyaRst [count] == 0) {
			busyo0 = busyo0 + gacyaRst[count];
		}else if(gacyaRst[count] == 1){
			busyo1 = busyo1 + gacyaRst[count];
		}else if(gacyaRst[count] == 2){
			busyo2 = busyo2 + gacyaRst[count];
		}else if(gacyaRst[count] == 3){
			busyo3 = busyo3 + gacyaRst[count];
		}else if(gacyaRst[count] == 4){
			busyo4 = busyo4 + gacyaRst[count];
		}else if(gacyaRst[count] == 5){
			busyo5 = busyo5 + gacyaRst[count];
		}else if(gacyaRst[count] == 6){
			busyo6 = busyo6 + gacyaRst[count];
		}else if(gacyaRst[count] == 7){
			busyo7 = busyo7 + gacyaRst[count];
		}else if(gacyaRst[count] == 8){
			busyo8 = busyo8 + gacyaRst[count];
		}else if(gacyaRst[count] == 9){
			busyo9 = busyo9 + gacyaRst[count];
		}else if(gacyaRst[count] == 10){
			busyo10 = busyo10 + gacyaRst[count];	
		}else if(gacyaRst[count] == 11){
			busyo11 = busyo11 + gacyaRst[count];	
		}else if(gacyaRst[count] == 12){
			busyo12 = busyo12 + gacyaRst[count];	
		}else if(gacyaRst[count] == 13){
			busyo13 = busyo13 + gacyaRst[count];	
		}else if(gacyaRst[count] == 14){
			busyo14 = busyo14 + gacyaRst[count];	
		}else if(gacyaRst[count] == 15){
			busyo15 = busyo15 + gacyaRst[count];	
		}else if(gacyaRst[count] == 16){
			busyo16 = busyo16 + gacyaRst[count];	
		}else if(gacyaRst[count] == 17){
			busyo17 = busyo17 + gacyaRst[count];	
		}else if(gacyaRst[count] == 18){
			busyo18 = busyo18 + gacyaRst[count];	
		}else if(gacyaRst[count] == 19){
			busyo19 = busyo19 + gacyaRst[count];	
		}else if(gacyaRst[count] == 20){
			busyo20 = busyo20 + gacyaRst[count];	
		}
	}
}
