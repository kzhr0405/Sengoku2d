using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class SenkouButton : MonoBehaviour {
	
	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		/*Common Process*/
		string BoardPath = "Prefabs/PostKassen/Popup";
		GameObject board = Instantiate(Resources.Load (BoardPath)) as GameObject;
		board.transform.parent = GameObject.Find ("Canvas").transform;
		board.transform.localScale = new Vector2 (0.6f,0.5f);
		board.transform.localPosition = new Vector2 (0,0);

		//Senkou List
		string senkouListPath = "Prefabs/PostKassen/SenkouList";
		GameObject senkouList = Instantiate(Resources.Load (senkouListPath)) as GameObject;
		senkouList.transform.parent = GameObject.Find ("Popup(Clone)").transform;
		senkouList.transform.localScale = new Vector2 (1,1);
		senkouList.transform.localPosition = new Vector3 (0,0,0);

		List<BusyoSenkou> livingBusyo = new List<BusyoSenkou>();
		livingBusyo=getSenkou ();

		for(int i=0;i<livingBusyo.Count;i++){
			int id = livingBusyo[i].id;
			int senkou = livingBusyo[i].senkou;
            if(senkou<0) {
                senkou = 0;
            }

			int juni = i + 1;

			//Slot
			string slotPath = "Prefabs/PostKassen/Slot";
			GameObject slot = Instantiate(Resources.Load (slotPath)) as GameObject;
			slot.transform.parent = GameObject.Find ("SenkouList(Clone)").transform;
			slot.transform.localScale = new Vector2 (1,1);


            //Value Change
            int langId = PlayerPrefs.GetInt("langId");
            if (langId == 2) {
                slot.transform.FindChild("Title").GetComponent<Text>().text = "No. " + juni;
            }else {
                slot.transform.FindChild("Title").GetComponent<Text>().text = "戦功第" + juni + "位";
            }
			slot.transform.FindChild("Kunkou").GetComponent<TextMesh>().text = senkou.ToString();

			//Busyo
			string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
			GameObject busyo = Instantiate(Resources.Load (busyoPath)) as GameObject;
			busyo.name = id.ToString ();
			busyo.transform.parent = slot.transform;
			busyo.transform.localScale = new Vector2 (0.8f,1);
			busyo.GetComponent<RectTransform>().sizeDelta = new Vector2(200,200);
			busyo.transform.localPosition = new Vector2(130,130);

			Destroy (busyo.GetComponent<DragHandler>());

		}
	}

	public List<BusyoSenkou> getSenkou(){
		//Get Living Busyo
		List<BusyoSenkou> livingBusyo = new List<BusyoSenkou>();
		foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Player")) {
			if(obs.GetComponent<Kunkou>() != null){
				if(obs.GetComponent<Kunkou>().engunFlg == false){
					if(obs.name !="kengou"){
						livingBusyo.Add(new BusyoSenkou(int.Parse(obs.name.Replace("(Clone)","")), obs.GetComponent<Kunkou>().kunkou));
					}
				}
			}
		}
		
		//Get Dead Busyo
		List<BusyoSenkou> deadBusyo = new List<BusyoSenkou>();
		deadBusyo = GameObject.Find ("GameScene").GetComponent<DeadBusyo>().deadBusyo;
		livingBusyo.AddRange (deadBusyo);
		
		//Sort
		livingBusyo.Sort((a,b) => b.senkou - a.senkou);

		return livingBusyo;
	}
}
