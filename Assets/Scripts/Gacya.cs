using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Gacya : MonoBehaviour {

	public GameObject KumoLeftObj;
	public GameObject KumoRightObj;

    //test
    public int SRank = 0;
    public int ARank = 0;
    public int BRank = 0;


    void Start(){
		KumoLeftObj = GameObject.Find ("KumoLeft").gameObject;
		KumoRightObj = GameObject.Find ("KumoRight").gameObject;
	}


	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		int[] hitBusyo = new int[3];
        int langId = PlayerPrefs.GetInt("langId");

        //Check
        Message msg = new Message();
		if(name == "DailyGacyaButton"){

			TouyouController script = GameObject.Find("TouyouController").GetComponent<TouyouController>();
			int freeGacyaCount = script.freeGacyaCount;
			if(freeGacyaCount<5){
				audioSources [8].Play ();
                //Reset Touyou Hist
                PlayerPrefs.DeleteKey("touyouHst");
                PlayerPrefs.Flush();

                //Disable
                GetComponent<Button>().enabled = false;
				GameObject.Find("BusyoDamaGacyaButton").GetComponent<Button>().enabled = false;

				//Reduce
				int countUp = freeGacyaCount + 1;
				int remain = 5 - countUp;
				script.freeGacyaCount = countUp;
				GameObject.Find("Count").GetComponent<Text>().text = remain.ToString();

				//Data
				PlayerPrefs.SetString("freeGacyaDate",  System.DateTime.Today.ToString ());
				PlayerPrefs.SetInt("freeGacyaCounter", countUp);
				PlayerPrefs.SetBool ("questDailyFlg18",true);

				PlayerPrefs.Flush();

				//Gacya
				viewBusyo (doGacya (),true);
			}else{
				audioSources [4].Play ();

				msg.makeMessage(msg.getMessage(54,langId));
			}

		}else if(name == "BusyoDamaGacyaButton"){
            //BusyoDama Qty Check
            if (Application.loadedLevelName == "tutorialTouyou") {
                audioSources[8].Play();

                //Disable
                GetComponent<Button>().enabled = false;
                GameObject.Find("DailyGacyaButton").GetComponent<Button>().enabled = false;

                //Gacya
                viewBusyo(doTutorialGacya(), true);

                GameObject tBack = GameObject.Find("tBack");
                GameObject.Find("CenterView").transform.SetParent(tBack.transform);


                TextController textScript = GameObject.Find("TextBoard").transform.Find("Text").GetComponent<TextController>();
                textScript.tutorialId = 7;
                textScript.SetText(7);
                textScript.SetNextLine();

                GameObject busyoDamaGacyaBtn = GameObject.Find("tButton").transform.Find("BusyoDamaGacya").gameObject;
                busyoDamaGacyaBtn.transform.SetParent(GameObject.Find("UnderView").transform);
                busyoDamaGacyaBtn.transform.localPosition = new Vector2(320,-300);
                Destroy(busyoDamaGacyaBtn.transform.Find("point_up").gameObject);
            }else { 
                int busyoDama = PlayerPrefs.GetInt("busyoDama");
			    if (busyoDama >= 100) {
				    audioSources [8].Play ();
                    //Reset Touyou Hist
                    PlayerPrefs.DeleteKey("touyouHst");
                    PlayerPrefs.Flush();

                    //Disable
                    GetComponent<Button>().enabled = false;
				    GameObject.Find("DailyGacyaButton").GetComponent<Button>().enabled = false;

				    //Reduce Qty
				    int newBusyoDama = busyoDama - 100;
				    PlayerPrefs.SetInt ("busyoDama",newBusyoDama);
				    PlayerPrefs.SetBool ("questDailyFlg19", true);
				    PlayerPrefs.Flush ();

				    //Change Screen
				    GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = newBusyoDama.ToString();

				    //Gacya
				    viewBusyo (doGacya (),true);

			    } else {
				    audioSources [4].Play ();
				    msg.makeMessage(msg.getMessage(2,langId));
			    }
            }
        }
	}

	public int[] doGacya(){

		KumoLeftObj.GetComponent<KumoMove> ().runFlg = true;
		KumoRightObj.GetComponent<KumoMove> ().runFlg = true;

		//Atari Busyo
		int[] hitBusyo = new int[3];

		List<int> busyoListByWeight = new List<int> ();

		Entity_busyo_mst busyoMst  = Resources.Load ("Data/busyo_mst") as Entity_busyo_mst;

		if(name == "DailyGacyaButton"){

			//Input BusyoId by Weight of each busyo
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


		}else if(name == "BusyoDamaGacyaButton"){
            //Input BusyoId by Weight of each busyo
            //SRank = 0;
            //ARank = 0;
            //BRank = 0;

            for (int i=0; i<busyoMst.param.Count; i++){
				int weight = busyoMst.param [i].GacyaTama;
				int busyoId = busyoMst.param[i].id;
				
				for(int j=0; j<weight; j++){
					busyoListByWeight.Add (busyoId);

                    /*
                    //test
                    string rank = busyoMst.param[i].rank;
                    if (rank=="S") {
                        SRank = SRank + 1;
                    }else if (rank == "A") {
                        ARank = ARank + 1;
                    }else if (rank == "B") {
                        BRank = BRank + 1;
                    }*/
                }
			}
			
			//Get 3 by Randam without dupplication
			for(int k=0; k<3; k++){
				int rdmId = UnityEngine.Random.Range(0,busyoListByWeight.Count);
				hitBusyo[k] = busyoListByWeight[rdmId];
			}
		}

        //keep gacya history
        string hitBusyoString = hitBusyo [0] + "," + hitBusyo [1] +","+ hitBusyo [2];
        PlayerPrefs.SetString("gacyaHst",hitBusyoString);
		PlayerPrefs.Flush();

        //hitBusyo = new int[3] { 111, 111, 111 };
        return hitBusyo;
	}

    public int[] doTutorialGacya() {

        GameObject tButton = GameObject.Find("tButton");
        KumoLeftObj.transform.SetParent(tButton.transform);
        KumoRightObj.transform.SetParent(tButton.transform);

        KumoLeftObj.GetComponent<KumoMove>().runFlg = true;
        KumoRightObj.GetComponent<KumoMove>().runFlg = true;

        //Atari Busyo
        int[] hitBusyo = new int[3] {4,16,201};


        return hitBusyo;
    }



    public void viewBusyo(int[] hitBusyo, bool waitSecondFlg){
			
		if (waitSecondFlg) {
			StartCoroutine(visualizeBusyo(2.0f,hitBusyo));
		} else {
			foreach ( Transform n in GameObject.Find ("CenterView").transform ){
				GameObject.Destroy(n.gameObject);
			}
			visualizeBusyo2(hitBusyo);
		}
	}


	IEnumerator visualizeBusyo(float delay, int[] hitBusyo){
		

		yield return new WaitForSeconds (delay);

		foreach ( Transform n in GameObject.Find ("CenterView").transform ){
			GameObject.Destroy(n.gameObject);
		}

		//1st
		string busyoPath1 = "Prefabs/Player/Unit/BusyoUnit";
		GameObject busyo1 = Instantiate (Resources.Load (busyoPath1)) as GameObject;
		busyo1.name = hitBusyo [0].ToString();
		busyo1.transform.SetParent (GameObject.Find ("CenterView").transform);
		busyo1.transform.localScale = new Vector2 (8, 8);
		RectTransform busyo1_transform = busyo1.GetComponent<RectTransform>();
		busyo1_transform.anchoredPosition3D = new Vector3(200,180,0);
		busyo1_transform.sizeDelta = new Vector2( 30, 30);
		busyo1.GetComponent<DragHandler> ().enabled = false;	
		busyo1.transform.Find ("Text").GetComponent<Text> ().color = new Color (255, 255, 255, 255);

		//2nd
		string busyoPath2 = "Prefabs/Player/Unit/BusyoUnit";
		GameObject busyo2 = Instantiate (Resources.Load (busyoPath2)) as GameObject;
		busyo2.name = hitBusyo [1].ToString();
		busyo2.transform.SetParent (GameObject.Find ("CenterView").transform);
		busyo2.transform.localScale = new Vector2 (8, 8);
		RectTransform busyo2_transform = busyo2.GetComponent<RectTransform>();
		busyo2_transform.anchoredPosition3D = new Vector3(600,180,0);
		busyo2_transform.sizeDelta = new Vector2( 30, 30);
		busyo2.GetComponent<DragHandler> ().enabled = false;	
		busyo2.transform.Find ("Text").GetComponent<Text> ().color = new Color (255, 255, 255, 255);

		//3rd
		string busyoPath3 = "Prefabs/Player/Unit/BusyoUnit";
		GameObject busyo3 = Instantiate (Resources.Load (busyoPath3)) as GameObject;
		busyo3.name = hitBusyo [2].ToString();
		busyo3.transform.SetParent (GameObject.Find ("CenterView").transform);
		busyo3.transform.localScale = new Vector2 (8, 8);
		RectTransform busyo3_transform = busyo3.GetComponent<RectTransform>();
		busyo3_transform.anchoredPosition3D = new Vector3(1000,180,0);
		busyo3_transform.sizeDelta = new Vector2( 30, 30);
		busyo3.GetComponent<DragHandler> ().enabled = false;	
		busyo3.transform.Find ("Text").GetComponent<Text> ().color = new Color (255, 255, 255, 255);


		/*Button or Batu*/

		//Touyou history
		string touyouHst = PlayerPrefs.GetString("touyouHst");
        if(Application.loadedLevelName == "tutorialTouyou") {
            touyouHst = null;
        }
        BusyoInfoGet busyo = new BusyoInfoGet ();

		if (touyouHst == null || touyouHst == "") {
			//OK can touyou

			//Show touyou button
			string buttonPath = "Prefabs/Touyou/Button";
			GameObject button1 = Instantiate (Resources.Load (buttonPath)) as GameObject;
			button1.transform.SetParent (busyo1.transform);
			button1.transform.localScale = new Vector2 (0.12f, 0.25f);
			RectTransform button1_transform = button1.GetComponent<RectTransform>();
			button1_transform.anchoredPosition = new Vector2 (0,-20);
			button1.GetComponent<TouyouView> ().busyoId = hitBusyo [0];
			string rank1 = busyo.getRank(hitBusyo [0]);
			button1.GetComponent<TouyouView> ().busyoRank = rank1;
			button1.name = "1";

			if(rank1 == "S" || rank1 == "A"){
				string effectPath = "Prefabs/Touyou/gacyaEffect" + rank1;
				GameObject effect = Instantiate (Resources.Load (effectPath)) as GameObject;
				effect.transform.SetParent (busyo1.transform);
				effect.transform.localScale = new Vector2 (18, 11);
				effect.transform.localPosition = new Vector3 (0, 10, 0);

			}

			GameObject button2 = Instantiate (Resources.Load (buttonPath)) as GameObject;
			button2.transform.SetParent (busyo2.transform);
			button2.transform.localScale = new Vector2 (0.12f, 0.25f);
			RectTransform button2_transform = button2.GetComponent<RectTransform>();
			button2_transform.anchoredPosition = new Vector2 (0,-20);
			button2.GetComponent<TouyouView> ().busyoId = hitBusyo [1];
			string rank2 = busyo.getRank(hitBusyo [1]);
			button2.GetComponent<TouyouView> ().busyoRank = rank2;
			button2.name = "2";

			if(rank2 == "S" || rank2 == "A"){
				string effectPath = "Prefabs/Touyou/gacyaEffect" + rank2;
				GameObject effect = Instantiate (Resources.Load (effectPath)) as GameObject;
				effect.transform.SetParent (busyo2.transform);
				effect.transform.localScale = new Vector2 (18, 11);
				effect.transform.localPosition = new Vector3 (0, 10, 0);

			}

			GameObject button3 = Instantiate (Resources.Load (buttonPath)) as GameObject;
			button3.transform.SetParent (busyo3.transform);
			button3.transform.localScale = new Vector2 (0.12f, 0.25f);
			RectTransform button3_transform = button3.GetComponent<RectTransform>();
			button3_transform.anchoredPosition = new Vector2 (0,-20);
			button3.GetComponent<TouyouView> ().busyoId = hitBusyo [2];
			string rank3 = busyo.getRank(hitBusyo [2]);
			button3.GetComponent<TouyouView> ().busyoRank = rank3;
			button3.name = "3";

			if(rank3 == "S" || rank3 == "A"){
				string effectPath = "Prefabs/Touyou/gacyaEffect" + rank3;
				GameObject effect = Instantiate (Resources.Load (effectPath)) as GameObject;
				effect.transform.SetParent (busyo3.transform);
				effect.transform.localScale = new Vector2 (18, 11);
				effect.transform.localPosition = new Vector3 (0, 10, 0);

			}

		} else {
			//NG already touyou
			string batuPath = "Prefabs/Touyou/Batu";
			char[] delimiterChars = {','};
			string[] tokens = touyouHst.Split(delimiterChars);
			if(tokens[0] == "1"){
				//Left
				GameObject batu1 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu1.transform.SetParent (busyo1.transform);
				batu1.transform.localScale = new Vector2 (1, 1);
				RectTransform batu1_transform = batu1.GetComponent<RectTransform>();
				batu1_transform.anchoredPosition = new Vector2 (0,0);

				GameObject batu2 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu2.transform.SetParent (busyo2.transform);
				batu2.transform.localScale = new Vector2 (1, 1);
				RectTransform batu2_transform = batu2.GetComponent<RectTransform>();
				batu2_transform.anchoredPosition = new Vector2 (0,0);
				batu2.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;

				GameObject batu3 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu3.transform.SetParent (busyo3.transform);
				batu3.transform.localScale = new Vector2 (1, 1);
				RectTransform batu3_transform = batu3.GetComponent<RectTransform>();
				batu3_transform.anchoredPosition = new Vector2 (0,0);
				batu3.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;


			}else if(tokens[1] == "1"){
				//Left
				GameObject batu1 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu1.transform.SetParent (busyo1.transform);
				batu1.transform.localScale = new Vector2 (1, 1);
				RectTransform batu1_transform = batu1.GetComponent<RectTransform>();
				batu1_transform.anchoredPosition = new Vector2 (0,0);
				batu1.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;


				GameObject batu2 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu2.transform.SetParent (busyo2.transform);
				batu2.transform.localScale = new Vector2 (1, 1);
				RectTransform batu2_transform = batu2.GetComponent<RectTransform>();
				batu2_transform.anchoredPosition = new Vector2 (0,0);

				GameObject batu3 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu3.transform.SetParent (busyo3.transform);
				batu3.transform.localScale = new Vector2 (1, 1);
				RectTransform batu3_transform = batu3.GetComponent<RectTransform>();
				batu3_transform.anchoredPosition = new Vector2 (0,0);
				batu3.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;


			}else if(tokens[2] == "1"){
				//Left
				GameObject batu1 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu1.transform.SetParent (busyo1.transform);
				batu1.transform.localScale = new Vector2 (1, 1);
				RectTransform batu1_transform = batu1.GetComponent<RectTransform>();
				batu1_transform.anchoredPosition = new Vector2 (0,0);
				batu1.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;


				GameObject batu2 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu2.transform.SetParent (busyo2.transform);
				batu2.transform.localScale = new Vector2 (1, 1);
				RectTransform batu2_transform = batu2.GetComponent<RectTransform>();
				batu2_transform.anchoredPosition = new Vector2 (0,0);
				batu2.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;

				GameObject batu3 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu3.transform.SetParent (busyo3.transform);
				batu3.transform.localScale = new Vector2 (1, 1);
				RectTransform batu3_transform = batu3.GetComponent<RectTransform>();
				batu3_transform.anchoredPosition = new Vector2 (0,0);
			}
		}
	}

	public void visualizeBusyo2(int[] hitBusyo){
		
		string busyoPath1 =  "Prefabs/Player/Unit/BusyoUnit";
		GameObject busyo1 = Instantiate (Resources.Load (busyoPath1)) as GameObject;
		busyo1.name = hitBusyo[0].ToString();
		busyo1.transform.SetParent (GameObject.Find ("CenterView").transform);
		busyo1.transform.localScale = new Vector2 (8, 8);
		RectTransform busyo1_transform = busyo1.GetComponent<RectTransform>();
		busyo1_transform.anchoredPosition3D = new Vector3(200,180,0);
		busyo1_transform.sizeDelta = new Vector2( 30, 30);
		busyo1.GetComponent<DragHandler> ().enabled = false;	
		busyo1.transform.Find ("Text").GetComponent<Text> ().color = new Color (255, 255, 255, 255);

		//2nd
		string busyoPath2 =  "Prefabs/Player/Unit/BusyoUnit";
		GameObject busyo2 = Instantiate (Resources.Load (busyoPath2)) as GameObject;
		busyo2.name = hitBusyo[1].ToString();
		busyo2.transform.SetParent (GameObject.Find ("CenterView").transform);
		busyo2.transform.localScale = new Vector2 (8, 8);
		RectTransform busyo2_transform = busyo2.GetComponent<RectTransform>();
		busyo2_transform.anchoredPosition3D = new Vector3(600,180,0);
		busyo2_transform.sizeDelta = new Vector2( 30, 30);
		busyo2.GetComponent<DragHandler> ().enabled = false;	
		busyo2.transform.Find ("Text").GetComponent<Text> ().color = new Color (255, 255, 255, 255);

		//3rd
		string busyoPath3 = "Prefabs/Player/Unit/BusyoUnit";
		GameObject busyo3 = Instantiate (Resources.Load (busyoPath3)) as GameObject;
		busyo3.name = hitBusyo[2].ToString();
		busyo3.transform.SetParent (GameObject.Find ("CenterView").transform);
		busyo3.transform.localScale = new Vector2 (8, 8);
		RectTransform busyo3_transform = busyo3.GetComponent<RectTransform>();
		busyo3_transform.anchoredPosition3D = new Vector3(1000,180,0);
		busyo3_transform.sizeDelta = new Vector2( 30, 30);
		busyo3.GetComponent<DragHandler> ().enabled = false;	
		busyo3.transform.Find ("Text").GetComponent<Text> ().color = new Color (255, 255, 255, 255);


		/*Button or Batu*/

		//Touyou history
		string touyouHst = PlayerPrefs.GetString("touyouHst");
        BusyoInfoGet busyo = new BusyoInfoGet ();

		if (touyouHst == null || touyouHst == "") {
			//OK can touyou

			//Show touyou button
			string buttonPath = "Prefabs/Touyou/Button";
			GameObject button1 = Instantiate (Resources.Load (buttonPath)) as GameObject;
			button1.transform.SetParent (busyo1.transform);
			button1.transform.localScale = new Vector2 (0.12f, 0.25f);
			RectTransform button1_transform = button1.GetComponent<RectTransform>();
			button1_transform.anchoredPosition = new Vector2 (0,-20);
			button1.GetComponent<TouyouView> ().busyoId = hitBusyo [0];
			string rank1 = busyo.getRank(hitBusyo [0]);
			button1.GetComponent<TouyouView> ().busyoRank = rank1;
			button1.name = "1";

			if(rank1 == "S" || rank1 == "A"){
				string effectPath = "Prefabs/Touyou/gacyaEffect" + rank1;
				GameObject effect = Instantiate (Resources.Load (effectPath)) as GameObject;
				effect.transform.SetParent (busyo1.transform);
				effect.transform.localScale = new Vector2 (18, 11);
				effect.transform.localPosition = new Vector3 (0, 10, 0);

			}

			GameObject button2 = Instantiate (Resources.Load (buttonPath)) as GameObject;
			button2.transform.SetParent (busyo2.transform);
			button2.transform.localScale = new Vector2 (0.12f, 0.25f);
			RectTransform button2_transform = button2.GetComponent<RectTransform>();
			button2_transform.anchoredPosition = new Vector2 (0,-20);
			button2.GetComponent<TouyouView> ().busyoId = hitBusyo [1];
			string rank2 = busyo.getRank(hitBusyo [1]);
			button2.GetComponent<TouyouView> ().busyoRank = rank2;
			button2.name = "2";

			if(rank2 == "S" || rank2 == "A"){
				string effectPath = "Prefabs/Touyou/gacyaEffect" + rank2;
				GameObject effect = Instantiate (Resources.Load (effectPath)) as GameObject;
				effect.transform.SetParent (busyo2.transform);
				effect.transform.localScale = new Vector2 (18, 11);
				effect.transform.localPosition = new Vector3 (0, 10, 0);

			}

			GameObject button3 = Instantiate (Resources.Load (buttonPath)) as GameObject;
			button3.transform.SetParent (busyo3.transform);
			button3.transform.localScale = new Vector2 (0.12f, 0.25f);
			RectTransform button3_transform = button3.GetComponent<RectTransform>();
			button3_transform.anchoredPosition = new Vector2 (0,-20);
			button3.GetComponent<TouyouView> ().busyoId = hitBusyo [2];
			string rank3 = busyo.getRank(hitBusyo [2]);
			button3.GetComponent<TouyouView> ().busyoRank = rank3;
			button3.name = "3";

			if(rank3 == "S" || rank3 == "A"){
				string effectPath = "Prefabs/Touyou/gacyaEffect" + rank3;
				GameObject effect = Instantiate (Resources.Load (effectPath)) as GameObject;
				effect.transform.SetParent (busyo3.transform);
				effect.transform.localScale = new Vector2 (18, 11);
				effect.transform.localPosition = new Vector3 (0, 10, 0);

			}

		} else {
			//NG already touyou
			string batuPath = "Prefabs/Touyou/Batu";
			char[] delimiterChars = {','};
			string[] tokens = touyouHst.Split(delimiterChars);
			if(tokens[0] == "1"){
				//Left
				GameObject batu1 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu1.transform.SetParent (busyo1.transform);
				batu1.transform.localScale = new Vector2 (1, 1);
				RectTransform batu1_transform = batu1.GetComponent<RectTransform>();
				batu1_transform.anchoredPosition = new Vector2 (0,0);

				GameObject batu2 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu2.transform.SetParent (busyo2.transform);
				batu2.transform.localScale = new Vector2 (1, 1);
				RectTransform batu2_transform = batu2.GetComponent<RectTransform>();
				batu2_transform.anchoredPosition = new Vector2 (0,0);
				batu2.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;

				GameObject batu3 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu3.transform.SetParent (busyo3.transform);
				batu3.transform.localScale = new Vector2 (1, 1);
				RectTransform batu3_transform = batu3.GetComponent<RectTransform>();
				batu3_transform.anchoredPosition = new Vector2 (0,0);
				batu3.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;


			}else if(tokens[1] == "1"){
				//Left
				GameObject batu1 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu1.transform.SetParent (busyo1.transform);
				batu1.transform.localScale = new Vector2 (1, 1);
				RectTransform batu1_transform = batu1.GetComponent<RectTransform>();
				batu1_transform.anchoredPosition = new Vector2 (0,0);
				batu1.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;


				GameObject batu2 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu2.transform.SetParent (busyo2.transform);
				batu2.transform.localScale = new Vector2 (1, 1);
				RectTransform batu2_transform = batu2.GetComponent<RectTransform>();
				batu2_transform.anchoredPosition = new Vector2 (0,0);

				GameObject batu3 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu3.transform.SetParent (busyo3.transform);
				batu3.transform.localScale = new Vector2 (1, 1);
				RectTransform batu3_transform = batu3.GetComponent<RectTransform>();
				batu3_transform.anchoredPosition = new Vector2 (0,0);
				batu3.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;


			}else if(tokens[2] == "1"){
				//Left
				GameObject batu1 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu1.transform.SetParent (busyo1.transform);
				batu1.transform.localScale = new Vector2 (1, 1);
				RectTransform batu1_transform = batu1.GetComponent<RectTransform>();
				batu1_transform.anchoredPosition = new Vector2 (0,0);
				batu1.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;


				GameObject batu2 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu2.transform.SetParent (busyo2.transform);
				batu2.transform.localScale = new Vector2 (1, 1);
				RectTransform batu2_transform = batu2.GetComponent<RectTransform>();
				batu2_transform.anchoredPosition = new Vector2 (0,0);
				batu2.transform.Find("TouyouZumiText").GetComponent<Text>().enabled = false;

				GameObject batu3 = Instantiate (Resources.Load (batuPath)) as GameObject;
				batu3.transform.SetParent (busyo3.transform);
				batu3.transform.localScale = new Vector2 (1, 1);
				RectTransform batu3_transform = batu3.GetComponent<RectTransform>();
				batu3_transform.anchoredPosition = new Vector2 (0,0);
			}
		}
	}
}
