using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class KahouScene : MonoBehaviour {

	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		TabHandler tab = new TabHandler ();
		tab.tabButtonColor ("Kahou");

		GameObject mainController = GameObject.Find ("GameScene");
		mainController.GetComponent<NowOnButton> ().onButton = "Kahou";

		if (GameObject.Find ("GameScene").GetComponent<NowOnButton> ().lastButton != "Kahou") {
			//Delete Previous View
			Destroy(GameObject.Find ("BusyoStatus"));
			Destroy(GameObject.Find ("SenpouStatus"));
			Destroy(GameObject.Find ("SyoguMenu"));

			//Make Kahou Status
			string kahouPath = "Prefabs/Busyo/KahouStatus";
			GameObject kahou = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahou.transform.SetParent(GameObject.Find ("CenterView").transform);
			kahou.transform.localScale = new Vector2 (1, 1);
			kahou.name = "KahouStatus";
			
			RectTransform kahou_transform = kahou.GetComponent<RectTransform> ();
			kahou_transform.anchoredPosition3D = new Vector3 (240, 31, 0);

			/*Kahou View*/
			string busyoId = mainController.GetComponent<NowOnBusyo>().OnBusyo;
			createKahouStatusView(busyoId);

			//Controller Setting
			mainController.GetComponent<NowOnButton> ().lastButton = "Kahou";

		}
	}
	public void createKahouStatusView(string busyoId){

		//Initialization
		//Delete Previous Kahou
		if(GameObject.FindGameObjectsWithTag("Kahou") != null){
			foreach (GameObject obs in  GameObject.FindGameObjectsWithTag("Kahou")){
				Destroy(obs);
			}
		}

		ArrayList kahou_list = new ArrayList ();
		string tmp = "kahou" + busyoId;
		string kahouString = PlayerPrefs.GetString (tmp);
        if(kahouString == "" || kahouString == null) {
            kahouString = "0,0,0,0,0,0,0,0";
            PlayerPrefs.SetString(tmp, kahouString);
            PlayerPrefs.Flush();
        }
		char[] delimiterChars = {','};
		kahou_list.AddRange (kahouString.Split (delimiterChars));

        
		for (int i=0; i<kahou_list.Count; i++) {
			string kahouId = kahou_list[i].ToString();
			
			if(i == 0){
				//Bugu
				if(kahouId != "0"){
					//Exist
					string buguId = "bugu" + kahouId;
					string buguPath = "Prefabs/Item/Kahou/" + buguId;
					GameObject bugu = Instantiate (Resources.Load (buguPath)) as GameObject;
					bugu.transform.SetParent(GameObject.Find ("ItemBugu").transform);
					bugu.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = bugu.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);

					bugu.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
					bugu.GetComponent<KahouInfo>().kahouType = "bugu";

				}else{
					//Not Exist
					string buguPath = "Prefabs/Item/Kahou/NoBugu";
					GameObject bugu = Instantiate (Resources.Load (buguPath)) as GameObject;
					bugu.transform.SetParent(GameObject.Find ("ItemBugu").transform);
					bugu.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = bugu.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);
					bugu.name = "NoBugu";
				}
				
			}else if(i == 1){
				//Kabuto
				if(kahouId != "0"){
					//Exist
					string kabutoId = "kabuto" + kahouId;
					string kabutoPath = "Prefabs/Item/Kahou/" + kabutoId;
					GameObject kabuto = Instantiate (Resources.Load (kabutoPath)) as GameObject;
					kabuto.transform.SetParent(GameObject.Find ("ItemKabuto").transform);
					kabuto.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = kabuto.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);

					kabuto.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
					kabuto.GetComponent<KahouInfo>().kahouType = "kabuto";
					
				}else{
					//Not Exist
					string kabutoPath = "Prefabs/Item/Kahou/NoKabuto";
					GameObject kabuto = Instantiate (Resources.Load (kabutoPath)) as GameObject;
					kabuto.transform.SetParent(GameObject.Find ("ItemKabuto").transform);
					kabuto.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = kabuto.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);
					kabuto.name = "NoKabuto";
				}
			}else if(i == 2){
				//Gusoku
				if(kahouId != "0"){
					//Exist
					string gusokuId = "gusoku" + kahouId;
					string gusokuPath = "Prefabs/Item/Kahou/" + gusokuId;
					GameObject gusoku = Instantiate (Resources.Load (gusokuPath)) as GameObject;
					gusoku.transform.SetParent(GameObject.Find ("ItemGusoku").transform);
					gusoku.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = gusoku.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);

					gusoku.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
					gusoku.GetComponent<KahouInfo>().kahouType = "gusoku";
					
				}else{
					//Not Exist
					string gusokuPath = "Prefabs/Item/Kahou/NoGusoku";
					GameObject gusoku = Instantiate (Resources.Load (gusokuPath)) as GameObject;
					gusoku.transform.SetParent(GameObject.Find ("ItemGusoku").transform);
					gusoku.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = gusoku.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);
					gusoku.name = "NoGusoku";
				}
				
				
				
				
			}else if(i == 3){
				//Meiba
				if(kahouId != "0"){
					//Exist
					string meibaId = "meiba" + kahouId;
					string meibaPath = "Prefabs/Item/Kahou/" + meibaId;
					GameObject meiba = Instantiate (Resources.Load (meibaPath)) as GameObject;
					meiba.transform.SetParent(GameObject.Find ("ItemMeiba").transform);
					meiba.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = meiba.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);

					meiba.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
					meiba.GetComponent<KahouInfo>().kahouType = "meiba";

				}else{
					//Not Exist
					string meibaPath = "Prefabs/Item/Kahou/NoMeiba";
					GameObject meiba = Instantiate (Resources.Load (meibaPath)) as GameObject;
					meiba.transform.SetParent(GameObject.Find ("ItemMeiba").transform);
					meiba.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = meiba.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);
					meiba.name = "NoMeiba";
				}
				
			}else if(i == 4){
				//Cyadougu1
				if(kahouId != "0"){
					//Exist
					string cyadouguId = "cyadougu" + kahouId;
					string cyadouguPath = "Prefabs/Item/Kahou/" + cyadouguId;
					GameObject cya = Instantiate (Resources.Load (cyadouguPath)) as GameObject;
					cya.transform.SetParent(GameObject.Find ("ItemCyadougu1").transform);
					cya.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = cya.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);

					cya.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
					cya.GetComponent<KahouInfo>().kahouType = "cyadougu";
					
				}else{
					//Not Exist
					string cyadouguPath = "Prefabs/Item/Kahou/NoCyadougu";
					GameObject cya = Instantiate (Resources.Load (cyadouguPath)) as GameObject;
					cya.transform.SetParent(GameObject.Find ("ItemCyadougu1").transform);
					cya.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = cya.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);
					cya.name = "NoCyadougu";
				}
				
			}else if(i == 5){
				//Cyadougu2
				if(kahouId != "0"){
					//Exist
					string cyadouguId = "cyadougu" + kahouId;
					string cyadouguPath = "Prefabs/Item/Kahou/" + cyadouguId;
					GameObject cya = Instantiate (Resources.Load (cyadouguPath)) as GameObject;
					cya.transform.SetParent(GameObject.Find ("ItemCyadougu2").transform);
					cya.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = cya.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);

					cya.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
					cya.GetComponent<KahouInfo>().kahouType = "cyadougu";
					
				}else{
					//Not Exist
					string cyadouguPath = "Prefabs/Item/Kahou/NoCyadougu";
					GameObject cya = Instantiate (Resources.Load (cyadouguPath)) as GameObject;
					cya.transform.SetParent(GameObject.Find ("ItemCyadougu2").transform);
					cya.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = cya.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);
					cya.name = "NoCyadougu";
				}
			}else if(i == 6){
				//Heihousyo
				if(kahouId != "0"){
					//Exist
					string heihousyoId = "heihousyo" + kahouId;
					string heihousyoPath = "Prefabs/Item/Kahou/" + heihousyoId;
					GameObject heihousyo = Instantiate (Resources.Load (heihousyoPath)) as GameObject;
					heihousyo.transform.SetParent(GameObject.Find ("ItemHeihousyo").transform);
					heihousyo.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = heihousyo.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);

					heihousyo.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
					heihousyo.GetComponent<KahouInfo>().kahouType = "heihousyo";
					
				}else{
					//Not Exist
					string heihousyoPath = "Prefabs/Item/Kahou/NoHeihousyo";
					GameObject heihousyo = Instantiate (Resources.Load (heihousyoPath)) as GameObject;
					heihousyo.transform.SetParent(GameObject.Find ("ItemHeihousyo").transform);
					heihousyo.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = heihousyo.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);
					heihousyo.name = "NoHeihousyo";
				}
			}else if(i == 7){
				//Chisikisyo
				if(kahouId != "0"){
					//Exist
					string chisikisyoId = "chishikisyo" + kahouId;
					string chisikisyoPath = "Prefabs/Item/Kahou/" + chisikisyoId;
					GameObject chisikisyo = Instantiate (Resources.Load (chisikisyoPath)) as GameObject;
					chisikisyo.transform.SetParent(GameObject.Find ("ItemChishikisyo").transform);
					chisikisyo.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = chisikisyo.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);

					chisikisyo.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
					chisikisyo.GetComponent<KahouInfo>().kahouType = "chishikisyo";
					
				}else{
					//Not Exist
					string chisikisyoPath = "Prefabs/Item/Kahou/NoChishikisyo";
					GameObject chisikisyo = Instantiate (Resources.Load (chisikisyoPath)) as GameObject;
					chisikisyo.transform.SetParent(GameObject.Find ("ItemChishikisyo").transform);
					chisikisyo.transform.localScale = new Vector3 (1, 1, 1);
					RectTransform transform = chisikisyo.GetComponent<RectTransform> ();
					transform.anchoredPosition3D = new Vector3 (0, -20, 0);
					chisikisyo.name = "NoChishikisyo";
				}
			}
			
		}
	}
}
