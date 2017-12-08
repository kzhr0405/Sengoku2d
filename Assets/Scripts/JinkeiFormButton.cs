using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class JinkeiFormButton : MonoBehaviour {

	public void OnClick() {

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();
        int senarioId = PlayerPrefs.GetInt("senarioId");
        Resources.UnloadUnusedAssets ();
		ButtonColorChanger(name);

        if (Application.loadedLevelName == "preKassen") {
            preKassen preKassenScript = GameObject.Find("GameScene").GetComponent<preKassen>();
            int jinkei = 0;
            if (name=="Gyorin") {
                jinkei = 1;
            }
            else if(name=="Kakuyoku") {
                 jinkei = 2;

            }
            else if(name=="Engetsu") {
                 jinkei = 3;

            }
            else if(name =="Gankou") {
                 jinkei = 4;

            }
            int weatherId = preKassenScript.weatherId;
            bool isAttackedFlg = preKassenScript.isAttackedFlg;

            preKassenScript.prekassenPlayerJinkei(jinkei, weatherId, isAttackedFlg,true, false,senarioId);
            GameObject.Find("BusyoScrollMenu").GetComponent<PopScrollSlider>().onceSlideInFlg = false;

        }
        else if(Application.loadedLevelName == "preKaisen") {
            preKaisen preKassenScript = GameObject.Find("GameScene").GetComponent<preKaisen>();
            int jinkei = 0;
            if (name == "Gyorin") {
                jinkei = 1;
            }
            else if (name == "Kakuyoku") {
                jinkei = 2;

            }
            else if (name == "Engetsu") {
                jinkei = 3;

            }
            else if (name == "Gankou") {
                jinkei = 4;

            }
            int weatherId = preKassenScript.weatherId;
            bool isAttackedFlg = preKassenScript.isAttackedFlg;

            preKassenScript.prekassenPlayerJinkei(jinkei, weatherId, isAttackedFlg, true,senarioId);
            GameObject.Find("BusyoScrollMenu").GetComponent<PopScrollSlider>().onceSlideInFlg = false;

        }
        else {
            GameObject kakuteiButton = GameObject.Find ("KakuteiButton");
		    GameObject gameScene = GameObject.Find("GameScene") as GameObject;
		    int busyoQty = 0;

		    //Clear Previous Unit
		    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
			    if(obs.transform.childCount > 0){
				    //Delete
				    DestroyImmediate(obs.transform.GetChild(0).gameObject);
			    }
		    }

            //Set sorting as default
            GameObject.Find("BusyoSortDropdown").GetComponent<Dropdown>().value = 0;

            //魚鱗
            BusyoInfoGet busyoScript = new BusyoInfoGet();
            if (name == "Gyorin"){
			    int soudaisyo = PlayerPrefs.GetInt("soudaisyo1");

			    List<string> jinkeiBusyo_list = new List<string>();
			    kakuteiButton.GetComponent<Jinkei>().selectedJinkei = 1;

			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
				    //Enable 1,2,7,8,11,12,13,14,17,18,21,22
				    if(obs.name == "Slot1" || obs.name == "Slot2" ||obs.name == "Slot7" ||obs.name == "Slot8" ||
				       obs.name == "Slot11" ||obs.name == "Slot12" ||obs.name == "Slot13" ||obs.name == "Slot14" ||
				       obs.name == "Slot17" ||obs.name == "Slot18" ||obs.name == "Slot21" ||obs.name == "Slot22"){

					    obs.GetComponent<Image>().enabled = true;
					    string mapId = "1map" + obs.name.Substring(4);
					    if(PlayerPrefs.HasKey(mapId)){
						    busyoQty = busyoQty + 1;
						    int busyoId = PlayerPrefs.GetInt(mapId);

						    //Instantiate
						    string path = "Prefabs/Player/Unit/BusyoUnit";
						    GameObject chldBusyo = Instantiate(Resources.Load (path)) as GameObject;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.parent = obs.transform;
						    chldBusyo.name = busyoId.ToString();
						    chldBusyo.transform.localScale = new Vector2(4,4);
						    chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
						    jinkeiBusyo_list.Add(busyoId.ToString());

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
                            int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name), senarioId);
                            if (daimyoId == 0) {
                                daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name), senarioId);
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

				    //Disable 3,4,5,6,9,15,16,19,20,23,24,25
				    }else{
					    obs.GetComponent<Image>().enabled = false;

					    if(obs.transform.IsChildOf(obs.transform)){

						    foreach ( Transform n in obs.transform ){
							    GameObject.Destroy(n.gameObject);
						    }

					    }
				    }
			    }
			    gameScene.GetComponent<JinkeiScene>().UnitOnScrollView(jinkeiBusyo_list);

		    //鶴翼
		    }else if(name == "Kakuyoku"){
			    int soudaisyo = PlayerPrefs.GetInt("soudaisyo2");

			    List<string> jinkeiBusyo_list = new List<string>();
			    kakuteiButton.GetComponent<Jinkei>().selectedJinkei = 2;

			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
				    //Enable 3,4,5,7,8,11,12,17,18,23,24,25
				    if(obs.name == "Slot3" || obs.name == "Slot4" ||obs.name == "Slot5" ||obs.name == "Slot7" ||
				       obs.name == "Slot8" ||obs.name == "Slot11" ||obs.name == "Slot12" ||obs.name == "Slot17" ||
				       obs.name == "Slot18" ||obs.name == "Slot23" ||obs.name == "Slot24" ||obs.name == "Slot25"){
					
					    obs.GetComponent<Image>().enabled = true;
					    string mapId = "2map" + obs.name.Substring(4);
					    if(PlayerPrefs.HasKey(mapId)){
						    busyoQty = busyoQty + 1;
						    int busyoId = PlayerPrefs.GetInt(mapId);
						
						    //Instantiate
						    string path = "Prefabs/Player/Unit/BusyoUnit";
						    GameObject chldBusyo = Instantiate(Resources.Load (path)) as GameObject;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.parent = obs.transform;
						    chldBusyo.name = busyoId.ToString();
						    chldBusyo.transform.localScale = new Vector2(4,4);
						    chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
						    jinkeiBusyo_list.Add(busyoId.ToString());

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
                            int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name),senarioId);
                            if (daimyoId == 0) {
                                daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name),senarioId);
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

					    //Disable 1,2,6,9,10,13,14,15,16,19,20,21
				    }else{
					    obs.GetComponent<Image>().enabled = false;

					    if(obs.transform.IsChildOf(obs.transform)){
						
						    foreach ( Transform n in obs.transform ){
							    GameObject.Destroy(n.gameObject);
						    }	
					    }
				    }
			    }
			    gameScene.GetComponent<JinkeiScene>().UnitOnScrollView(jinkeiBusyo_list);

		    //偃月
		    }else if(name == "Engetsu"){
			    int soudaisyo = PlayerPrefs.GetInt("soudaisyo3");

			    List<string> jinkeiBusyo_list = new List<string>();
			    kakuteiButton.GetComponent<Jinkei>().selectedJinkei = 3;

			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
				    //Enable 3,7,8,9,11,12,14,15,16,20,21,25
				    if(obs.name == "Slot3" || obs.name == "Slot7" ||obs.name == "Slot8" ||obs.name == "Slot9" ||
				       obs.name == "Slot11" ||obs.name == "Slot12" ||obs.name == "Slot14" ||obs.name == "Slot15" ||
				       obs.name == "Slot16" ||obs.name == "Slot20" ||obs.name == "Slot21" ||obs.name == "Slot25"){
					
					    obs.GetComponent<Image>().enabled = true;
					    string mapId = "3map" + obs.name.Substring(4);
					    if(PlayerPrefs.HasKey(mapId)){
						    busyoQty = busyoQty + 1;
						    int busyoId = PlayerPrefs.GetInt(mapId);
						
						    //Instantiate
						    string path = "Prefabs/Player/Unit/BusyoUnit";
						    GameObject chldBusyo = Instantiate(Resources.Load (path)) as GameObject;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.parent = obs.transform;
						    chldBusyo.name = busyoId.ToString();
						    chldBusyo.transform.localScale = new Vector2(4,4);
						    chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
						    jinkeiBusyo_list.Add(busyoId.ToString());

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
                            int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name), senarioId);
                            if (daimyoId == 0) {
                                daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name), senarioId);
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
			    gameScene.GetComponent<JinkeiScene>().UnitOnScrollView(jinkeiBusyo_list);

		    //雁行
		    }else if(name == "Gankou"){
			    int soudaisyo = PlayerPrefs.GetInt("soudaisyo4");

			    List<string> jinkeiBusyo_list = new List<string>();
			    kakuteiButton.GetComponent<Jinkei>().selectedJinkei = 4;

			    foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Slot")){
				    //Enable 1,2,7,8,12,13,14,18,19,20,24,25
				    if(obs.name == "Slot1" || obs.name == "Slot2" ||obs.name == "Slot7" ||obs.name == "Slot8" ||
				       obs.name == "Slot12" ||obs.name == "Slot13" ||obs.name == "Slot14" ||obs.name == "Slot18" ||
				       obs.name == "Slot19" ||obs.name == "Slot20" ||obs.name == "Slot24" ||obs.name == "Slot25"){
					
					    obs.GetComponent<Image>().enabled = true;
					    string mapId = "4map" + obs.name.Substring(4);
					    if(PlayerPrefs.HasKey(mapId)){
						    busyoQty = busyoQty + 1;
						    int busyoId = PlayerPrefs.GetInt(mapId);
						
						    //Instantiate
						    string path = "Prefabs/Player/Unit/BusyoUnit";
						    GameObject chldBusyo = Instantiate(Resources.Load (path)) as GameObject;
						    chldBusyo.name = busyoId.ToString ();
						    chldBusyo.transform.parent = obs.transform;
						    chldBusyo.name = busyoId.ToString();
						    chldBusyo.transform.localScale = new Vector2(4,4);
						    chldBusyo.AddComponent<Senryoku>().GetPlayerSenryoku(chldBusyo.name);
						    jinkeiBusyo_list.Add(busyoId.ToString());

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
                            int daimyoId = busyoScript.getDaimyoId(int.Parse(chldBusyo.name), senarioId);
                            if (daimyoId == 0) {
                                daimyoId = busyoScript.getDaimyoHst(int.Parse(chldBusyo.name), senarioId);
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
			    gameScene.GetComponent<JinkeiScene>().UnitOnScrollView(jinkeiBusyo_list);

		    }

		    //Qty of Busyo on Status
		    GameObject.Find ("jinkeiQtyValue").GetComponent<Text> ().text = busyoQty.ToString ();
		    JinkeiPowerEffection powerEffection = new JinkeiPowerEffection ();
		    powerEffection.UpdateSenryoku ();

        }
	}




	public void ButtonColorChanger(string btnName){
		Color pushedTabColor = new Color (118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
		Color pushedTextColor = new Color (190f / 255f, 190f / 255f, 80f / 255f, 255f / 255f);

		Color normalTabColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
		Color normalTextColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);


		GameObject jinkeiButton = GameObject.Find ("JinkeiButton").gameObject;
		GameObject gyorin = jinkeiButton.transform.Find ("Gyorin").gameObject;
		GameObject kakuyoku = jinkeiButton.transform.Find ("Kakuyoku").gameObject;
		GameObject engetsu = jinkeiButton.transform.Find ("Engetsu").gameObject;
		GameObject gankou = jinkeiButton.transform.Find ("Gankou").gameObject;

		gyorin.GetComponent<Image> ().color = normalTabColor;
		kakuyoku.GetComponent<Image> ().color = normalTabColor;
		engetsu.GetComponent<Image> ().color = normalTabColor;
		gankou.GetComponent<Image> ().color = normalTabColor;

		gyorin.transform.Find ("Text").GetComponent<Text> ().color = normalTextColor;
		kakuyoku.transform.Find ("Text").GetComponent<Text> ().color = normalTextColor;
		engetsu.transform.Find ("Text").GetComponent<Text> ().color = normalTextColor;
		gankou.transform.Find ("Text").GetComponent<Text> ().color = normalTextColor;

		//Change selected Tab Color
		if(btnName == "Gyorin"){
			gyorin.GetComponent<Image> ().color = pushedTabColor;
			gyorin.transform.Find ("Text").GetComponent<Text> ().color = pushedTextColor;
		}else if(btnName == "Kakuyoku"){
			kakuyoku.GetComponent<Image> ().color = pushedTabColor;
			kakuyoku.transform.Find ("Text").GetComponent<Text> ().color = pushedTextColor;
		}else if(btnName == "Engetsu"){
			engetsu.GetComponent<Image> ().color = pushedTabColor;
			engetsu.transform.Find ("Text").GetComponent<Text> ().color = pushedTextColor;
		}else if(btnName == "Gankou"){
			gankou.GetComponent<Image> ().color = pushedTabColor;
			gankou.transform.Find ("Text").GetComponent<Text> ().color = pushedTextColor;
		}

	}
}
