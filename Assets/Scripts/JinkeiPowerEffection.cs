using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class JinkeiPowerEffection : MonoBehaviour {


	public void UpdateSenryoku(){
		
		//Param for myDaimyoBusyo
		bool myDaimyoBusyoOnFlg = false;
		List<GameObject> allObjList = new List<GameObject> ();
		int addRatioForMyDaimyoBusyo = 10;

		//Check
		foreach (GameObject slotObj in  GameObject.FindGameObjectsWithTag("Slot")) {
			foreach(Transform busyoObj in slotObj.transform){

				//myDaimyoBusyo Check
				allObjList.Add (busyoObj.gameObject);
				if (busyoObj.gameObject.GetComponent<Senryoku> ().myDaimyoBusyoFlg) {
					myDaimyoBusyoOnFlg = true;                    
					busyoObj.GetComponent<Soudaisyo> ().OnClick ();

				}

				//SameDaimyo Check & Update
				int belongDaimyoId = busyoObj.GetComponent<Senryoku>().belongDaimyoId;
				int numSameDaimyo = 0;
				bool sameDaimyoOnFlg  = false;

				foreach(GameObject tmpSlotObj in  GameObject.FindGameObjectsWithTag("Slot")){
					foreach(Transform tmpBusyoObj in tmpSlotObj.transform){
						if (tmpBusyoObj.GetComponent<Senryoku> ().belongDaimyoId == belongDaimyoId) {
							numSameDaimyo = numSameDaimyo + 1;
							if (numSameDaimyo == 2) {
								sameDaimyoOnFlg = true;
							}
						}
					}
				}
				if (sameDaimyoOnFlg) {
					int totalAtk = busyoObj.GetComponent<Senryoku> ().totalAtk;
					int totalDfc = busyoObj.GetComponent<Senryoku> ().totalDfc;

					int addAtk = 0;
					int addDfc = 0;
					int addRatio = (numSameDaimyo - 1) * 5;
					addAtk = Mathf.FloorToInt (((float)totalAtk * (float)addRatio) / 100);
					addDfc = Mathf.FloorToInt (((float)totalDfc * (float)addRatio) / 100);
					busyoObj.GetComponent<Senryoku> ().belongDaimyoAddAtk = addAtk;
					busyoObj.GetComponent<Senryoku> ().belongDaimyoAddDfc = addDfc;

					//if (Application.loadedLevelName != "preKassen") {
						VisualizeSameDaimyo (busyoObj.gameObject, belongDaimyoId, addAtk, addDfc);
					//}
				} else {
					busyoObj.GetComponent<Senryoku> ().belongDaimyoAddAtk = 0;
					busyoObj.GetComponent<Senryoku> ().belongDaimyoAddDfc = 0;
				}

				busyoObj.GetComponent<Senryoku> ().numSameDaimyo = numSameDaimyo;

			}
		}

		float totalAddAtk = 0;
		float totalAddDfc = 0;

		foreach (GameObject targetBusyoObj in  allObjList) {

			int totalAtk = targetBusyoObj.GetComponent<Senryoku> ().totalAtk;
			int totalDfc = targetBusyoObj.GetComponent<Senryoku> ().totalDfc;

			//Update by myDaimyoBusyo
			if (myDaimyoBusyoOnFlg) {
				float addAtk = 0;
				float addDfc = 0;
				addAtk = ((float)totalAtk * (float)addRatioForMyDaimyoBusyo) / 100;
				addDfc = ((float)totalDfc * (float)addRatioForMyDaimyoBusyo) / 100;
				totalAddAtk = totalAddAtk + addAtk;
				totalAddDfc = totalAddDfc + addDfc;

				targetBusyoObj.GetComponent<Senryoku> ().myDaimyoAddAtk = Mathf.FloorToInt (addAtk);
				targetBusyoObj.GetComponent<Senryoku> ().myDaimyoAddDfc = Mathf.FloorToInt (addDfc);

				//Disable Soudaisyo Button
				targetBusyoObj.GetComponent<Button>().enabled = false;

			} else {
				targetBusyoObj.GetComponent<Senryoku> ().myDaimyoAddAtk = 0;
				targetBusyoObj.GetComponent<Senryoku> ().myDaimyoAddDfc = 0;

				//Enable Soudaisyo Button
				targetBusyoObj.GetComponent<Button>().enabled = true;

			}
		}


		if (myDaimyoBusyoOnFlg) {
			//if (Application.loadedLevelName != "preKassen") {
				VisualizeDaimyoBusyo (Mathf.FloorToInt (totalAddAtk), Mathf.FloorToInt (totalAddDfc));
			//}
		}

	}

	public void VisualizeSameDaimyo(GameObject busyoObj, int daimyoId, int addAtk, int addDfc){

		string flagPath = "Prefabs/Jinkei/Flag";	
		GameObject flag = Instantiate (Resources.Load (flagPath)) as GameObject;

		string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString ();
		flag.GetComponent<Image> ().sprite = 
			Resources.Load (imagePath, typeof(Sprite)) as Sprite;
		flag.transform.SetParent(busyoObj.transform);
		flag.transform.localScale = new Vector3 (0.5f, 0.5f, 0);
		flag.transform.localPosition = new Vector3(0, 0, 0);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            flag.transform.Find ("Effect").GetComponent<Text> ().text = "ATK+" + addAtk + "\n" + "DFC+" + addDfc;
        }else if(langId==3) {
            flag.transform.Find("Effect").GetComponent<Text>().text = "勇武+" + addAtk + "\n" + "防御+" + addDfc;
        }
        else {
            flag.transform.Find("Effect").GetComponent<Text>().text = "武勇+" + addAtk + "\n" + "守備+" + addDfc;
        }
	}

	public void VisualizeDaimyoBusyo(int totalAddAtk, int totalAddDfc){
		string msgPath = "Prefabs/Jinkei/MyDaimyoMessage";	
		GameObject msgObj = Instantiate (Resources.Load (msgPath)) as GameObject;

		GameObject panel = GameObject.Find ("Panel").gameObject;
		msgObj.transform.SetParent(panel.transform);
		msgObj.transform.localScale = new Vector3 (1, 1, 0);
		msgObj.transform.localPosition = new Vector3(0, 0, 0);
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            msgObj.transform.Find ("Text").GetComponent<Text> ().text = "Total ATK+" + totalAddAtk + "\n" + "Total DFC+" + totalAddDfc;
        }else if (langId == 3) {
            msgObj.transform.Find("Text").GetComponent<Text>().text = "总勇武+" + totalAddAtk + "\n" + "总防御+" + totalAddDfc;
        } else {
            msgObj.transform.Find("Text").GetComponent<Text>().text = "総武勇+" + totalAddAtk + "\n" + "総守備+" + totalAddDfc;
        }
	}

    public void EnemySameDaimyoNum(int daimyoId, int senarioId) {

        Daimyo daimyo = new Daimyo();
        BusyoInfoGet BusyoInfoGet = new BusyoInfoGet();
        int daimyoBusyoId = daimyo.getDaimyoBusyoId(daimyoId, senarioId);
        bool daimyoBusyoFlg = false;
        List<int> daimyoIdList = new List<int>() { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("EnemySlot")) {
            int mapId = int.Parse(obs.name.Substring(4));
            
            if (obs.transform.childCount > 0) {
                int childBusyoId = int.Parse(obs.transform.GetChild(0).name);                
                int belongDaimyoId = BusyoInfoGet.getDaimyoId(childBusyoId,senarioId);
                daimyoIdList[mapId-1] = belongDaimyoId;
                if (childBusyoId == daimyoBusyoId) {
                    daimyoBusyoFlg = true;
                }                
            }else {
                daimyoIdList[mapId-1] = 0;
            }
        }
        string flagPath = "Prefabs/Jinkei/Flag";
        GameObject EnemyJinkeiView = GameObject.Find("EnemyJinkeiView");
        List<int> sameDaimyoList = new List<int>();
        List<int> sameDaimyoNumList = new List<int>();
        for (int i = 0; i< daimyoIdList.Count; i++) {
            int daimyoId1 = daimyoIdList[i];
            int mapId = i + 1;
            int count = 0;
            foreach(int daimyoId2 in daimyoIdList) {
                if(daimyoId1 == daimyoId2) {
                    count = count + 1;
                }
            }
            if(count>=2 && daimyoId1 !=0) {
                //flag                
                GameObject flag = Instantiate(Resources.Load(flagPath)) as GameObject;
                string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId1.ToString();
                flag.GetComponent<Image>().sprite =
                    Resources.Load(imagePath, typeof(Sprite)) as Sprite;
                flag.transform.SetParent(EnemyJinkeiView.transform.Find("Slot" + mapId).GetChild(0).transform);
                flag.transform.localScale = new Vector3(0.5f, 0.5f, 0);
                flag.transform.localPosition = new Vector3(0, 0, 0);
                Destroy(flag.transform.Find("Effect").gameObject);
                if(!sameDaimyoList.Contains(daimyoId1)) {
                    sameDaimyoList.Add(daimyoId1);
                    sameDaimyoNumList.Add(count);
                }
            }
        }

        string sameDaimyoString = "";
        string sameDaimyoNumString = "";        
        for(int i=0; i<sameDaimyoList.Count; i++) {
            string sameDaimyo = sameDaimyoList[i].ToString();
            string sameDaimyoNum = sameDaimyoNumList[i].ToString();
            if (sameDaimyoString == "") {
                sameDaimyoString = sameDaimyo;
                sameDaimyoNumString = sameDaimyoNum;
            }else {
                sameDaimyoString = sameDaimyoString + "," + sameDaimyo;
                sameDaimyoNumString = sameDaimyoNumString + "," + sameDaimyoNum;
            }
        }
        GameObject.Find("StartBtn").GetComponent<startKassen2>().sameDaimyo = sameDaimyoString;
        GameObject.Find("StartBtn").GetComponent<startKassen2>().sameDaimyoNum = sameDaimyoNumString;

    }


}
