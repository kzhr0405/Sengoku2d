using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class JinkeiScene : MonoBehaviour {

	public GameObject totalHpValue;
	public GameObject totalAtkValue;
	public GameObject totalDfcValue;
	public GameObject KakuteiButton;

	void Start () {

        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        if (!tutorialDoneFlg || Application.loadedLevelName != "tutorialHyojyo") {

            //Kamon
            int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
            string myDaimyoStatusPath = "Prefabs/Kamon/" + myDaimyo.ToString();
            GameObject.Find("KamonView").GetComponent<Image>().sprite =
                Resources.Load(myDaimyoStatusPath, typeof(Sprite)) as Sprite;

            //jinkei limit update
            Exp kuniExp = new Exp();
            int kuniLv = PlayerPrefs.GetInt("kuniLv");
            int jinkeiLimit = kuniExp.getJinkeiLimit(kuniLv);
            PlayerPrefs.SetInt("jinkeiLimit", jinkeiLimit);
            PlayerPrefs.Flush();

            /*Status Initial View*/
            int addLimit = 0;
            if (PlayerPrefs.HasKey("addJinkei1")) {
                addLimit = 1;
            }
            if (PlayerPrefs.HasKey("addJinkei2")) {
                addLimit = addLimit + 1;
            }
            if (PlayerPrefs.HasKey("addJinkei3")) {
                addLimit = addLimit + 1;
            }
            if (PlayerPrefs.HasKey("addJinkei4")) {
                addLimit = addLimit + 1;
            }
            int totalLimit = jinkeiLimit + addLimit;
            GameObject.Find("jinkeiLimitValue").GetComponent<Text>().text = totalLimit.ToString();
        

            totalHpValue = GameObject.Find ("totalHpValue").gameObject;
		    totalAtkValue = GameObject.Find ("totalAtkValue").gameObject;
		    totalDfcValue = GameObject.Find ("totalDfcValue").gameObject;
		    KakuteiButton = GameObject.Find ("KakuteiButton").gameObject;

		    JinkeiFormButton jinkeiForm = new JinkeiFormButton ();
		    List<string> jinkeiBusyo_list = new List<string>();

		    //Jinkei View Change
		    int jinkei = PlayerPrefs.GetInt ("jinkei");
            KakuteiButton.GetComponent<Jinkei> ().selectedJinkei = jinkei;

            BusyoInfoGet busyoScript = new BusyoInfoGet();
		    if (jinkei == 1) {
			    int soudaisyo = PlayerPrefs.GetInt("soudaisyo1");
			    //Clear Previous Unit
			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")) {
				    //Enable 1,2,7,8,11,12,13,14,17,18,21,22
				    if (obs.name == "Slot1" || obs.name == "Slot2" || obs.name == "Slot7" || obs.name == "Slot8" ||
					    obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot13" || obs.name == "Slot14" ||
					    obs.name == "Slot17" || obs.name == "Slot18" || obs.name == "Slot21" || obs.name == "Slot22") {

					    obs.GetComponent<Image> ().enabled = true;
					    string mapId = "1map" + obs.name.Substring (4);
					    if (PlayerPrefs.HasKey (mapId)) {
						    int busyoId = PlayerPrefs.GetInt (mapId);
						    jinkeiBusyo_list.Add(busyoId.ToString());

						    //Instantiate
						    string path = "Prefabs/Player/Unit/BusyoUnit";
						    GameObject chldBusyo = Instantiate (Resources.Load (path)) as GameObject;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.parent = obs.transform;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.localScale = new Vector2 (4, 4);
						    chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);

						    chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

						    //Button
						    chldBusyo.AddComponent<Button>();
						    chldBusyo.AddComponent<Soudaisyo>();
						    chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

						    //soudaisyo
						    if(soudaisyo == int.Parse(chldBusyo.name)){
							    chldBusyo.GetComponent<Soudaisyo>().OnClick();
						    }

                            //Add Kamon
                            string KamonPath = "Prefabs/Jinkei/Kamon";
                            GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                            kamon.transform.SetParent(chldBusyo.transform);
                            kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                            kamon.transform.localPosition = new Vector2(-15, -12);
                            int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                            if (daimyoId == 0)
                            {
                                daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                            }
                            string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                            kamon.GetComponent<Image>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                            //Add Heisyu
                            string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                            string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                            GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                            heisyuObj.transform.SetParent(chldBusyo.transform, false);
                            heisyuObj.transform.localPosition = new Vector2(10, -10);
                            heisyuObj.transform.SetAsFirstSibling();
                        }
					
					    //Disable 3,4,5,6,9,10,15,16,19,20,23,24,25
				    } else {
					    obs.GetComponent<Image> ().enabled = false;
					
					    if (obs.transform.IsChildOf (obs.transform)) {
						
						    foreach (Transform n in obs.transform) {
							    GameObject.Destroy (n.gameObject);
						    }
						
					    }
				    }
			    }
			    UnitOnScrollView(jinkeiBusyo_list);

			    //Button Color
			    jinkeiForm.ButtonColorChanger("Gyorin");


			    //鶴翼
		    } else if (jinkei == 2) {
			    int soudaisyo = PlayerPrefs.GetInt("soudaisyo2");

			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
				    //Enable 3,4,5,7,8,11,12,17,18,23,24,25
				    if(obs.name == "Slot3" || obs.name == "Slot4" ||obs.name == "Slot5" ||obs.name == "Slot7" ||
				       obs.name == "Slot8" ||obs.name == "Slot11" ||obs.name == "Slot12" ||obs.name == "Slot17" ||
				       obs.name == "Slot18" ||obs.name == "Slot23" ||obs.name == "Slot24" ||obs.name == "Slot25"){
					
					    obs.GetComponent<Image>().enabled = true;
					    string mapId = "2map" + obs.name.Substring(4);
					    if(PlayerPrefs.HasKey(mapId)){
						    int busyoId = PlayerPrefs.GetInt(mapId);
						    jinkeiBusyo_list.Add(busyoId.ToString());

						    //Instantiate
						    string path = "Prefabs/Player/Unit/BusyoUnit";
						    GameObject chldBusyo = Instantiate (Resources.Load (path)) as GameObject;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.parent = obs.transform;
						    chldBusyo.name = busyoId.ToString();
						    chldBusyo.transform.localScale = new Vector2(4,4);
						    chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
						    chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

						    //Button
						    chldBusyo.AddComponent<Button>();
						    chldBusyo.AddComponent<Soudaisyo>();
						    chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

						    //soudaisyo
						    if(soudaisyo == int.Parse(chldBusyo.name)){
							    chldBusyo.GetComponent<Soudaisyo>().OnClick();
						    }

                            //Add Kamon
                            string KamonPath = "Prefabs/Jinkei/Kamon";
                            GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                            kamon.transform.SetParent(chldBusyo.transform);
                            kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                            kamon.transform.localPosition = new Vector2(-15, -12);
                            int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                            if (daimyoId == 0)
                            {
                                daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                            }
                            string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                            kamon.GetComponent<Image>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                            //Add Heisyu
                            string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                            string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                            GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                            heisyuObj.transform.SetParent(chldBusyo.transform, false);
                            heisyuObj.transform.localPosition = new Vector2(10, -10);
                            heisyuObj.transform.SetAsFirstSibling();
                        }
					
					    //Disable 1,2,6,9,10,13,14,15,16,19,20,21,22
				    }else{
					    obs.GetComponent<Image>().enabled = false;
					
					    if(obs.transform.IsChildOf(obs.transform)){
						
						    foreach ( Transform n in obs.transform ){
							    GameObject.Destroy(n.gameObject);
						    }	
					    }
				    }
			    }
			    UnitOnScrollView(jinkeiBusyo_list);

			    //Button Color
			    jinkeiForm.ButtonColorChanger("Kakuyoku");


		    } else if (jinkei == 3) {
			    int soudaisyo = PlayerPrefs.GetInt("soudaisyo3");

			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
				    //Enable 3,7,8,9,11,12,14,15,16,20,21,25
				    if(obs.name == "Slot3" || obs.name == "Slot7" ||obs.name == "Slot8" ||obs.name == "Slot9" ||
				       obs.name == "Slot11" ||obs.name == "Slot12" ||obs.name == "Slot14" ||obs.name == "Slot15" ||
				       obs.name == "Slot16" ||obs.name == "Slot20" ||obs.name == "Slot21" ||obs.name == "Slot25"){
					
					    obs.GetComponent<Image>().enabled = true;
					    string mapId = "3map" + obs.name.Substring(4);
					    if(PlayerPrefs.HasKey(mapId)){
						    int busyoId = PlayerPrefs.GetInt(mapId);
						    jinkeiBusyo_list.Add(busyoId.ToString());

						    //Instantiate
						    string path = "Prefabs/Player/Unit/BusyoUnit";
						    GameObject chldBusyo = Instantiate (Resources.Load (path)) as GameObject;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.parent = obs.transform;
						    chldBusyo.name = busyoId.ToString();
						    chldBusyo.transform.localScale = new Vector2(4,4);
						    chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
						    chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

						    //Button
						    chldBusyo.AddComponent<Button>();
						    chldBusyo.AddComponent<Soudaisyo>();
						    chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

						    //soudaisyo
						    if(soudaisyo == int.Parse(chldBusyo.name)){
							    chldBusyo.GetComponent<Soudaisyo>().OnClick();
						    }

                            //Add Kamon
                            string KamonPath = "Prefabs/Jinkei/Kamon";
                            GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                            kamon.transform.SetParent(chldBusyo.transform);
                            kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                            kamon.transform.localPosition = new Vector2(-15, -12);
                            int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                            if (daimyoId == 0)
                            {
                                daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                            }
                            string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                            kamon.GetComponent<Image>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                            //Add Heisyu
                            string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                            string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                            GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                            heisyuObj.transform.SetParent(chldBusyo.transform, false);
                            heisyuObj.transform.localPosition = new Vector2(10, -10);
                            heisyuObj.transform.SetAsFirstSibling();
                        }
					
					    //Disable 1,2,4,5,6,10,13,17,18,19,22,23,24
				    }else{
					    obs.GetComponent<Image>().enabled = false;
					    if(obs.transform.IsChildOf(obs.transform)){
						
						    foreach ( Transform n in obs.transform ){
							    GameObject.Destroy(n.gameObject);
						    }	
					    }
				    }
			    }
			    UnitOnScrollView(jinkeiBusyo_list);

			    //Button Color
			    jinkeiForm.ButtonColorChanger("Engetsu");


		    } else if (jinkei == 4) {
			    int soudaisyo = PlayerPrefs.GetInt("soudaisyo4");

			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
				    //Enable 1,2,7,8,12,13,14,18,19,20,24,25
				    if(obs.name == "Slot1" || obs.name == "Slot2" ||obs.name == "Slot7" ||obs.name == "Slot8" ||
				       obs.name == "Slot12" ||obs.name == "Slot13" ||obs.name == "Slot14" ||obs.name == "Slot18" ||
				       obs.name == "Slot19" ||obs.name == "Slot20" ||obs.name == "Slot24" ||obs.name == "Slot25"){
					
					    obs.GetComponent<Image>().enabled = true;
					    string mapId = "4map" + obs.name.Substring(4);
					    if(PlayerPrefs.HasKey(mapId)){
						    int busyoId = PlayerPrefs.GetInt(mapId);
						    jinkeiBusyo_list.Add(busyoId.ToString());

						    //Instantiate
						    string path = "Prefabs/Player/Unit/BusyoUnit";
						    GameObject chldBusyo = Instantiate (Resources.Load (path)) as GameObject;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.SetParent(obs.transform);
						    chldBusyo.name = busyoId.ToString();
						    chldBusyo.transform.localScale = new Vector2(4,4);
						    chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
						    chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

						    //Button
						    chldBusyo.AddComponent<Button>();
						    chldBusyo.AddComponent<Soudaisyo>();
						    chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

						    //soudaisyo
						    if(soudaisyo == int.Parse(chldBusyo.name)){
							    chldBusyo.GetComponent<Soudaisyo>().OnClick();
						    }

                            //Add Kamon
                            string KamonPath = "Prefabs/Jinkei/Kamon";
                            GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                            kamon.transform.SetParent(chldBusyo.transform);
                            kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                            kamon.transform.localPosition = new Vector2(-15, -12);
                            int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                            if (daimyoId == 0)
                            {
                                daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                            }
                            string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                            kamon.GetComponent<Image>().sprite =
                                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                            //Add Heisyu
                            string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                            string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                            GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                            heisyuObj.transform.SetParent(chldBusyo.transform, false);
                            heisyuObj.transform.localPosition = new Vector2(10, -10);
                            heisyuObj.transform.SetAsFirstSibling();
                        }
					
					    //Disable 3,4,5,6,9,10,11,15,16,17,21,22,23
				    }else{
					    obs.GetComponent<Image>().enabled = false;
					    if(obs.transform.IsChildOf(obs.transform)){
						
						    foreach ( Transform n in obs.transform ){
							    GameObject.Destroy(n.gameObject);
						    }	
					    }
				    }
			    }
			    UnitOnScrollView(jinkeiBusyo_list);

			    //Button Color
			    jinkeiForm.ButtonColorChanger("Gankou");

		    }

		    JinkeiPowerEffection powerEffection = new JinkeiPowerEffection ();
		    powerEffection.UpdateSenryoku ();

		    StatusView ();
		    SenryokuView ();
	    }else {
            //retry tutorial
            //Kamon
            int myDaimyo = 1;
            string myDaimyoStatusPath = "Prefabs/Kamon/" + myDaimyo.ToString();
            GameObject.Find("KamonView").GetComponent<Image>().sprite =
                Resources.Load(myDaimyoStatusPath, typeof(Sprite)) as Sprite;

            //jinkei limit update
            GameObject.Find("jinkeiLimitValue").GetComponent<Text>().text = 3.ToString();

            int jinkei = 1;
            totalHpValue = GameObject.Find("totalHpValue").gameObject;
            totalAtkValue = GameObject.Find("totalAtkValue").gameObject;
            totalDfcValue = GameObject.Find("totalDfcValue").gameObject;
            KakuteiButton = GameObject.Find("KakuteiButton").gameObject;
            KakuteiButton.GetComponent<Jinkei>().selectedJinkei = jinkei;
            BusyoInfoGet busyoScript = new BusyoInfoGet();
            JinkeiFormButton jinkeiForm = new JinkeiFormButton();

            int soudaisyo = 19;
            foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Slot")) {
                //Enable 1,2,7,8,11,12,13,14,17,18,21,22
                if (obs.name == "Slot1" || obs.name == "Slot2" || obs.name == "Slot7" || obs.name == "Slot8" ||
                    obs.name == "Slot11" || obs.name == "Slot12" || obs.name == "Slot13" || obs.name == "Slot14" ||
                    obs.name == "Slot17" || obs.name == "Slot18" || obs.name == "Slot21" || obs.name == "Slot22") {

                    obs.GetComponent<Image>().enabled = true;
                    
                    if (obs.name == "Slot12") {
                        int busyoId = 19;
                        
                        //Instantiate
                        string path = "Prefabs/Player/Unit/BusyoUnit";
                        GameObject chldBusyo = Instantiate(Resources.Load(path)) as GameObject;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.parent = obs.transform;
                        chldBusyo.name = busyoId.ToString();
                        chldBusyo.transform.localScale = new Vector2(4, 4);
                        chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);

                        chldBusyo.transform.localPosition = new Vector3(0, 0, 0);

                        //Button
                        chldBusyo.AddComponent<Button>();
                        chldBusyo.AddComponent<Soudaisyo>();
                        chldBusyo.GetComponent<Button>().onClick.AddListener(chldBusyo.GetComponent<Soudaisyo>().OnClick);

                        //soudaisyo
                        if (soudaisyo == int.Parse(chldBusyo.name)) {
                            chldBusyo.GetComponent<Soudaisyo>().OnClick();
                        }

                        //Add Kamon
                        string KamonPath = "Prefabs/Jinkei/Kamon";
                        GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                        kamon.transform.SetParent(chldBusyo.transform);
                        kamon.transform.localScale = new Vector2(0.1f, 0.1f);
                        kamon.transform.localPosition = new Vector2(-15, -12);
                        int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name));
                        if (daimyoId == 0) {
                            daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name));
                        }
                        string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                        kamon.GetComponent<Image>().sprite =
                            Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                        //Add Heisyu
                        string heisyu = busyoScript.getHeisyu(int.Parse(chldBusyo.name));
                        string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                        GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                        heisyuObj.transform.SetParent(chldBusyo.transform, false);
                        heisyuObj.transform.localPosition = new Vector2(10, -10);
                        heisyuObj.transform.SetAsFirstSibling();
                    }

                    //Disable 3,4,5,6,9,10,15,16,19,20,23,24,25
                }
                else {
                    obs.GetComponent<Image>().enabled = false;

                    if (obs.transform.IsChildOf(obs.transform)) {

                        foreach (Transform n in obs.transform) {
                            GameObject.Destroy(n.gameObject);
                        }

                    }
                }
            }
            UnitOnScrollViewTutorial();

            //Button Color
            jinkeiForm.ButtonColorChanger("Gyorin");


        }
    }

	public void UnitOnScrollView(List<string> jinkeiBusyo_list){

		//Clear Previous Unit
		foreach (Transform chd in GameObject.Find ("Content").transform){
			//Delete
			Destroy(chd.gameObject);

		}

		//Scroll View Change
		string myBusyoString = PlayerPrefs.GetString ("myBusyo");
		char[] delimiterChars = {','};

		List<string> myBusyo_list = new List<string>();
		if (myBusyoString.Contains (",")) {
			myBusyo_list = new List<string> (myBusyoString.Split (delimiterChars));
		} else {
			myBusyo_list.Add(myBusyoString);
		}

		for(int i=0; i < jinkeiBusyo_list.Count; i++ ){
			myBusyo_list.Remove(jinkeiBusyo_list[i]);
		}

		//Instantiate scroll view
		string scrollPath = "Prefabs/Jinkei/Slot";
        BusyoInfoGet busyoScript = new BusyoInfoGet();
		for(int j=0; j<myBusyo_list.Count; j++){

            if(myBusyo_list[j] != "0") {
			    //Slot
			    GameObject prefab = Instantiate (Resources.Load (scrollPath)) as GameObject;
			    prefab.transform.SetParent(GameObject.Find ("Content").transform);
			    prefab.transform.localScale = new Vector3 (1, 1, 1);
			    prefab.transform.localPosition = new Vector3(0, 0, 0);
			    prefab.name = "Slot";

                //Busyo
                string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
			    GameObject busyo = Instantiate (Resources.Load (busyoPath)) as GameObject;
			    busyo.name = myBusyo_list[j];

                //Add Kamon
                string KamonPath = "Prefabs/Jinkei/Kamon";
                GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
                kamon.transform.SetParent(busyo.transform);
                kamon.transform.localScale = new Vector2(0.1f,0.1f);
                kamon.transform.localPosition = new Vector2(-15, -12);
                int daimyoId = busyoScript.getDaimyoId(int.Parse(busyo.name));
                if (daimyoId == 0){
                    daimyoId = busyoScript.getDaimyoHst(int.Parse(busyo.name));
                }
                string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
                kamon.GetComponent<Image>().sprite =
                    Resources.Load(imagePath, typeof(Sprite)) as Sprite;

                //Add Heisyu
                string heisyu = busyoScript.getHeisyu(int.Parse(busyo.name));
                string heisyuPath = "Prefabs/Jinkei/" + heisyu;
                GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
                heisyuObj.transform.SetParent(busyo.transform, false);
                heisyuObj.transform.localPosition = new Vector2(10, -10);
                heisyuObj.transform.SetAsFirstSibling();


                busyo.transform.SetParent(prefab.transform);
			    busyo.transform.localScale = new Vector3 (4, 4, 4);
			    busyo.name = myBusyo_list[j].ToString();
			    busyo.AddComponent<Senryoku>().GetPlayerSenryoku(busyo.name);

			    busyo.transform.localPosition = new Vector3(0, 0, 0);
            }
		}        
	}

    public void UnitOnScrollViewTutorial() {

        //Clear Previous Unit
        foreach (Transform chd in GameObject.Find("Content").transform) {
            //Delete
            Destroy(chd.gameObject);

        }

        //Scroll View Change
        string myBusyoTutorial = PlayerPrefs.GetInt("tutorialBusyo").ToString();
        
        //Instantiate scroll view
        string scrollPath = "Prefabs/Jinkei/Slot";
        BusyoInfoGet busyoScript = new BusyoInfoGet();       
        if (myBusyoTutorial != "0") {
            //Slot
            GameObject prefab = Instantiate(Resources.Load(scrollPath)) as GameObject;
            prefab.transform.SetParent(GameObject.Find("Content").transform);
            prefab.transform.localScale = new Vector3(1, 1, 1);
            prefab.transform.localPosition = new Vector3(0, 0, 0);
            prefab.name = "Slot";

            //Busyo
            string busyoPath = "Prefabs/Player/Unit/BusyoUnit";
            GameObject busyo = Instantiate(Resources.Load(busyoPath)) as GameObject;
            busyo.name = myBusyoTutorial;

            //Add Kamon
            string KamonPath = "Prefabs/Jinkei/Kamon";
            GameObject kamon = Instantiate(Resources.Load(KamonPath)) as GameObject;
            kamon.transform.SetParent(busyo.transform);
            kamon.transform.localScale = new Vector2(0.1f, 0.1f);
            kamon.transform.localPosition = new Vector2(-15, -12);
            int daimyoId = busyoScript.getDaimyoId(int.Parse(busyo.name));
            if (daimyoId == 0) {
                daimyoId = busyoScript.getDaimyoHst(int.Parse(busyo.name));
            }
            string imagePath = "Prefabs/Kamon/MyDaimyoKamon/" + daimyoId.ToString();
            kamon.GetComponent<Image>().sprite =
                Resources.Load(imagePath, typeof(Sprite)) as Sprite;

            //Add Heisyu
            string heisyu = busyoScript.getHeisyu(int.Parse(busyo.name));
            string heisyuPath = "Prefabs/Jinkei/" + heisyu;
            GameObject heisyuObj = Instantiate(Resources.Load(heisyuPath)) as GameObject;
            heisyuObj.transform.SetParent(busyo.transform, false);
            heisyuObj.transform.localPosition = new Vector2(10, -10);
            heisyuObj.transform.SetAsFirstSibling();


            busyo.transform.SetParent(prefab.transform);
            busyo.transform.localScale = new Vector3(4, 4, 4);
            busyo.name = myBusyoTutorial.ToString();
            busyo.AddComponent<Senryoku>().GetPlayerSenryoku(busyo.name);

            busyo.transform.localPosition = new Vector3(0, 0, 0);
        }
        
    }




    public void StatusView(){
		

		int busyoQty = 0;

		foreach(Transform child in GameObject.Find("JinkeiView").transform){
			//Count up Mago Exist
			if(child.childCount >0){
				//busyoQty
				busyoQty = busyoQty +1;

			}
		}
		GameObject.Find ("jinkeiQtyValue").GetComponent<Text> ().text = busyoQty.ToString ();
	}

	void FixedUpdate(){
		SenryokuView ();
	}

	public void SenryokuView(){
		//Current Status
	
		int totalHp = 0;
		int totalAtk = 0;
		int totalDfc = 0;
		int totalLv = 0;
		int totalChLv = 0;
        int totalChQty = 0;

        GameObject JinkeiView = GameObject.Find ("JinkeiView").gameObject;
		foreach(Transform childSlot in JinkeiView.transform){
			//Count up Mago Exist
			if(childSlot.childCount >0){
				//senryoku
				foreach(Transform busyo in childSlot.transform){
					Senryoku senryokuScript = busyo.gameObject.GetComponent<Senryoku> ();
					totalHp = totalHp + senryokuScript.totalHp;
					totalAtk = totalAtk + senryokuScript.totalAtk + senryokuScript.myDaimyoAddAtk + senryokuScript.belongDaimyoAddAtk;
					totalDfc = totalDfc + senryokuScript.totalDfc  + senryokuScript.myDaimyoAddDfc + senryokuScript.belongDaimyoAddDfc;
					totalLv = totalLv + senryokuScript.lv;
					totalChLv = totalChLv + senryokuScript.chlv;
                    totalChQty = totalChQty + senryokuScript.chQty;
                }
			}
		}
        if (Application.loadedLevelName == "tutorialHyojyo") {
            if(GameObject.Find("copiedJinkeiView") != null) {
            GameObject copiedJinkeiView = GameObject.Find("copiedJinkeiView").gameObject;
                foreach (Transform childSlot in copiedJinkeiView.transform) {
                    //Count up Mago Exist
                    if (childSlot.childCount > 0) {
                        //senryoku
                        foreach (Transform busyo in childSlot.transform) {
                            Senryoku senryokuScript = busyo.gameObject.GetComponent<Senryoku>();
                            totalHp = totalHp + senryokuScript.totalHp;
                            totalAtk = totalAtk + senryokuScript.totalAtk + senryokuScript.myDaimyoAddAtk + senryokuScript.belongDaimyoAddAtk;
                            totalDfc = totalDfc + senryokuScript.totalDfc + senryokuScript.myDaimyoAddDfc + senryokuScript.belongDaimyoAddDfc;
                            totalLv = totalLv + senryokuScript.lv;
                            totalChLv = totalChLv + senryokuScript.chlv;
                            totalChQty = totalChQty + senryokuScript.chQty;
                        }
                    }
                }
            }
        }


        totalHpValue.GetComponent<Text> ().text = totalHp.ToString ();
		totalAtkValue.GetComponent<Text> ().text = totalAtk.ToString ();
		totalDfcValue.GetComponent<Text> ().text = totalDfc.ToString ();

		KakuteiButton.GetComponent<JinkeiConfirmButton> ().totalLv = totalLv;
		KakuteiButton.GetComponent<JinkeiConfirmButton> ().totalChLv = totalChLv;
        KakuteiButton.GetComponent<JinkeiConfirmButton>().totalChQty = totalChQty;
    }
}