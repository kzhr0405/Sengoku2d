using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class KahouSoukoScene : MonoBehaviour {

	public void OnClick () {

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		/*Scene Change*/
		GameObject.Find ("GameScene").GetComponent<SoukoScene> ().currentTab = "KahouScene";

		/*button color change*/
		Color pushedTabColor = new Color (118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
		Color pushedTextColor = new Color (219f / 255f, 219f / 255f, 212f / 255f, 255f / 255f);
		Color normalTabColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
		Color normalTextColor = new Color (255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);

		//Clear Color
		GameObject kahou = GameObject.Find ("Kahou");
		GameObject dougu = GameObject.Find ("Dougu");

		kahou.GetComponent<Image> ().color = pushedTabColor;
		dougu.GetComponent<Image> ().color = normalTabColor;

		kahou.transform.Find ("Text").GetComponent<Text> ().color = pushedTextColor;
		dougu.transform.Find ("Text").GetComponent<Text> ().color = normalTextColor;

		/*Initialize Kahou View*/
		GameObject itemView = GameObject.Find ("ItemView");
		itemView.transform.Find ("GetMoney").GetComponent<Image>().enabled = false;
		GameObject sellBtn = GameObject.Find ("SellButton");
		sellBtn.GetComponent<Image>().enabled = false;
		sellBtn.GetComponent<Button>().enabled = false;
		sellBtn.transform.Find("Text").GetComponent<Text>().enabled = false;
		itemView.transform.Find ("KahouEffectValue").GetComponent<Text> ().text = "";
		itemView.transform.Find ("KahouEffectLabel").GetComponent<Text> ().text = "";
		GameObject.Find ("GetMoneyValue").GetComponent<Text> ().text = "";
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            itemView.transform.Find ("ItemNameValue").GetComponent<Text> ().text = "Select Item";
        }else if(langId==3) {
            itemView.transform.Find("ItemNameValue").GetComponent<Text>().text = "选择家宝";
        }else {
            itemView.transform.Find("ItemNameValue").GetComponent<Text>().text = "家宝選択";
        }

		GameObject.Find ("Background").GetComponent<Image>().enabled = false;
		GameObject.Find ("Fill").GetComponent<Image>().enabled = false;
		GameObject.Find ("Handle").GetComponent<Image>().enabled = false;
		GameObject.Find ("SellQty").GetComponent<Image>().enabled = false;
		GameObject.Find ("SellQtyValue").GetComponent<Text>().text = "";
		GameObject.Find ("DouguExpValue").GetComponent<Text>().text = "";


		foreach ( Transform n in itemView.transform ){
			if(n.tag == "Kahou"){
				GameObject.Destroy(n.gameObject);
			}
		}
		string defaultIconPath = "Prefabs/Item/Kahou/NoKabuto";
		GameObject defIcon = Instantiate (Resources.Load (defaultIconPath)) as GameObject;
		defIcon.transform.SetParent(itemView.transform);
		defIcon.transform.localScale = new Vector2 (1, 1);
		RectTransform defTransform = defIcon.GetComponent<RectTransform> ();
		defTransform.anchoredPosition3D = new Vector3 (0, 120, 0);
		defIcon.GetComponent<Button> ().enabled = false;

		/*avairable Kaho*/
		//Clear Previous Data
		GameObject content = GameObject.Find ("Content");
		foreach ( Transform n in content.transform ){
			Destroy(n.gameObject);
		}

		//Common Prametor
		char[] delimiterChars = {','};
		KahouStatusGet kahouStatus = new KahouStatusGet ();

		//availableBugu				
		string availableBuguString = PlayerPrefs.GetString("availableBugu");
        string numPath = "Prefabs/Souko/Num";
        List<string> doneKahouList = new List<string>();
        if (availableBuguString != null && availableBuguString !=""){
			string[] availableBugu_list = availableBuguString.Split (delimiterChars);
            Dictionary<string, int> dicBugu = new Dictionary<string, int>();
            foreach (string key in availableBugu_list) {
                if (dicBugu.ContainsKey(key)) dicBugu[key]++; else dicBugu.Add(key, 1);
            }

            foreach (string key in dicBugu.Keys) {
                string kahouId = key;
                string kahouTyp = "bugu";
				string kahouTypId = kahouTyp + kahouId;
                    
                string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);
                GameObject Num = Instantiate(Resources.Load(numPath)) as GameObject;
                Num.transform.SetParent(kahouIcon.transform,false);
                Num.GetComponent<Text>().text = dicBugu[key].ToString();
                Num.name = "Num";

                //Kahou Status
                List<string> kahouInfoList = new List<string> ();
				kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
                KahouInfo KahouInfo = kahouIcon.GetComponent<KahouInfo>();
                KahouInfo.kahouId = int.Parse(kahouId);
                KahouInfo.kahouType = kahouTyp;
                KahouInfo.kahouName = kahouInfoList[0];
                KahouInfo.kahouTarget = kahouInfoList[2];
                KahouInfo.kahouEffect = int.Parse(kahouInfoList[3]);
                KahouInfo.kahouUnit = kahouInfoList[4];
                KahouInfo.kahouSell = int.Parse(kahouInfoList[6]);
                KahouInfo.qty = dicBugu[key];
                kahouIcon.name = kahouTypId;
                doneKahouList.Add(kahouTypId);
                
			}
		}

		//availableKabuto				
		string availableKabutoString = PlayerPrefs.GetString("availableKabuto");
		if(availableKabutoString != null && availableKabutoString !=""){
			string[] availableKabuto_list = availableKabutoString.Split (delimiterChars);
            Dictionary<string, int> dicKabuto = new Dictionary<string, int>();
            foreach (string key in availableKabuto_list) {
                if (dicKabuto.ContainsKey(key)) dicKabuto[key]++; else dicKabuto.Add(key, 1);
            }

            foreach (string key in dicKabuto.Keys) {
                string kahouId = key;
                string kahouTyp = "kabuto";
				string kahouTypId = kahouTyp + kahouId;
                
                string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);
                GameObject Num = Instantiate(Resources.Load(numPath)) as GameObject;
                Num.transform.SetParent(kahouIcon.transform, false);
                Num.GetComponent<Text>().text = dicKabuto[key].ToString();
                Num.name = "Num";

                //Kahou Status
                List<string> kahouInfoList = new List<string> ();
                KahouInfo KahouInfo = kahouIcon.GetComponent<KahouInfo>();
                kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
                KahouInfo.kahouId = int.Parse(kahouId);
                KahouInfo.kahouType = kahouTyp;
                KahouInfo.kahouName = kahouInfoList[0];
                KahouInfo.kahouTarget = kahouInfoList[2];
                KahouInfo.kahouEffect = int.Parse(kahouInfoList[3]);
                KahouInfo.kahouUnit = kahouInfoList[4];
                KahouInfo.kahouSell = int.Parse(kahouInfoList[6]);
                KahouInfo.qty = dicKabuto[key];
                kahouIcon.name = kahouTypId;
                doneKahouList.Add(kahouTypId);
                
			}
		}

		//availableGusoku				
		string availableGusokuString = PlayerPrefs.GetString("availableGusoku");
		if(availableGusokuString != null && availableGusokuString !=""){
			string[] availableGusoku_list = availableGusokuString.Split (delimiterChars);
            Dictionary<string, int> dicGusoku = new Dictionary<string, int>();
            foreach (string key in availableGusoku_list) {
                if (dicGusoku.ContainsKey(key)) dicGusoku[key]++; else dicGusoku.Add(key, 1);
            }

            foreach (string key in dicGusoku.Keys) {
                string kahouId = key;
                string kahouTyp = "gusoku";
				string kahouTypId = kahouTyp + kahouId;

                
                string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);
                GameObject Num = Instantiate(Resources.Load(numPath)) as GameObject;
                Num.transform.SetParent(kahouIcon.transform, false);
                Num.GetComponent<Text>().text = dicGusoku[key].ToString();
                Num.name = "Num";

                //Kahou Status
                List<string> kahouInfoList = new List<string> ();
                KahouInfo KahouInfo = kahouIcon.GetComponent<KahouInfo>();
                kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
                KahouInfo.kahouId = int.Parse(kahouId);
                KahouInfo.kahouType = kahouTyp;
                KahouInfo.kahouName = kahouInfoList[0];
                KahouInfo.kahouTarget = kahouInfoList[2];
                KahouInfo.kahouEffect = int.Parse(kahouInfoList[3]);
                KahouInfo.kahouUnit = kahouInfoList[4];
                KahouInfo.kahouSell = int.Parse(kahouInfoList[6]);
                KahouInfo.qty = dicGusoku[key];
                kahouIcon.name = kahouTypId;
                doneKahouList.Add(kahouTypId);            

			}
		}

		//availableMeiba				
		string availableMeibaString = PlayerPrefs.GetString("availableMeiba");
		if(availableMeibaString != null && availableMeibaString !=""){
			string[] availableMeiba_list = availableMeibaString.Split (delimiterChars);
            Dictionary<string, int> dicMeiba = new Dictionary<string, int>();
            foreach (string key in availableMeiba_list) {
                if (dicMeiba.ContainsKey(key)) dicMeiba[key]++; else dicMeiba.Add(key, 1);
            }

            foreach (string key in dicMeiba.Keys) {
                string kahouId = key;
                string kahouTyp = "meiba";
				string kahouTypId = kahouTyp + kahouId;
                
                string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);
                GameObject Num = Instantiate(Resources.Load(numPath)) as GameObject;
                Num.transform.SetParent(kahouIcon.transform, false);
                Num.GetComponent<Text>().text = dicMeiba[key].ToString();
                Num.name = "Num";

                //Kahou Status
                List<string> kahouInfoList = new List<string> ();
                KahouInfo KahouInfo = kahouIcon.GetComponent<KahouInfo>();
                kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
                KahouInfo.kahouId = int.Parse(kahouId);
                KahouInfo.kahouType = kahouTyp;
                KahouInfo.kahouName = kahouInfoList[0];
                KahouInfo.kahouTarget = kahouInfoList[2];
                KahouInfo.kahouEffect = int.Parse(kahouInfoList[3]);
                KahouInfo.kahouUnit = kahouInfoList[4];
                KahouInfo.kahouSell = int.Parse(kahouInfoList[6]);
                KahouInfo.qty = dicMeiba[key];
                kahouIcon.name = kahouTypId;
                doneKahouList.Add(kahouTypId);
                
			}
		}

		//availableCyadougu				
		string availableCyadouguString = PlayerPrefs.GetString("availableCyadougu");
		if(availableCyadouguString != null && availableCyadouguString !=""){
			string[] availableCyadougu_list = availableCyadouguString.Split (delimiterChars);
            Dictionary<string, int> dicCyadougu = new Dictionary<string, int>();
            foreach (string key in availableCyadougu_list) {
                if (dicCyadougu.ContainsKey(key)) dicCyadougu[key]++; else dicCyadougu.Add(key, 1);
            }

            foreach (string key in dicCyadougu.Keys) {
                string kahouId = key;
                string kahouTyp = "cyadougu";
				string kahouTypId = kahouTyp + kahouId;
                
                string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);
                GameObject Num = Instantiate(Resources.Load(numPath)) as GameObject;
                Num.transform.SetParent(kahouIcon.transform, false);
                Num.GetComponent<Text>().text = dicCyadougu[key].ToString();
                Num.name = "Num";

                //Kahou Status
                List<string> kahouInfoList = new List<string> ();
                KahouInfo KahouInfo = kahouIcon.GetComponent<KahouInfo>();
                kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
                KahouInfo.kahouId = int.Parse(kahouId);
                KahouInfo.kahouType = kahouTyp;
                KahouInfo.kahouName = kahouInfoList[0];
                KahouInfo.kahouTarget = kahouInfoList[2];
                KahouInfo.kahouEffect = int.Parse(kahouInfoList[3]);
                KahouInfo.kahouUnit = kahouInfoList[4];
                KahouInfo.kahouSell = int.Parse(kahouInfoList[6]);
                KahouInfo.qty = dicCyadougu[key];
                kahouIcon.name = kahouTypId;
                doneKahouList.Add(kahouTypId);
                
			}
		}

		//availableHeihousyo				
		string availableHeihousyoString = PlayerPrefs.GetString("availableHeihousyo");
		if(availableHeihousyoString != null && availableHeihousyoString !=""){
			string[] availableHeihousyo_list = availableHeihousyoString.Split (delimiterChars);
            Dictionary<string, int> dicHeihousyo = new Dictionary<string, int>();
            foreach (string key in availableHeihousyo_list) {
                if (dicHeihousyo.ContainsKey(key)) dicHeihousyo[key]++; else dicHeihousyo.Add(key, 1);
            }

            foreach (string key in dicHeihousyo.Keys) {
                string kahouId = key;
                string kahouTyp = "heihousyo";
				string kahouTypId = kahouTyp + kahouId;

                string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);
                GameObject Num = Instantiate(Resources.Load(numPath)) as GameObject;
                Num.transform.SetParent(kahouIcon.transform, false);
                Num.GetComponent<Text>().text = dicHeihousyo[key].ToString();
                Num.name = "Num";

                //Kahou Status
                List<string> kahouInfoList = new List<string> ();
                KahouInfo KahouInfo = kahouIcon.GetComponent<KahouInfo>();
                kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
                KahouInfo.kahouId = int.Parse(kahouId);
                KahouInfo.kahouType = kahouTyp;
                KahouInfo.kahouName = kahouInfoList[0];
                KahouInfo.kahouTarget = kahouInfoList[2];
                KahouInfo.kahouEffect = int.Parse(kahouInfoList[3]);
                KahouInfo.kahouUnit = kahouInfoList[4];
                KahouInfo.kahouSell = int.Parse(kahouInfoList[6]);
                KahouInfo.qty = dicHeihousyo[key];
                kahouIcon.name = kahouTypId;
                doneKahouList.Add(kahouTypId);
                
			}
		}

		//availableChishikisyo				
		string availableChishikisyoString = PlayerPrefs.GetString("availableChishikisyo");
		if(availableChishikisyoString != null && availableChishikisyoString !=""){
			string[] availableChishikisyo_list = availableChishikisyoString.Split (delimiterChars);
            Dictionary<string, int> dicChishikisyo = new Dictionary<string, int>();
            foreach (string key in availableChishikisyo_list) {
                if (dicChishikisyo.ContainsKey(key)) dicChishikisyo[key]++; else dicChishikisyo.Add(key, 1);
            }

            foreach (string key in dicChishikisyo.Keys) {
                string kahouId = key;
                string kahouTyp = "chishikisyo";
				string kahouTypId = kahouTyp + kahouId;
                
                string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);
                GameObject Num = Instantiate(Resources.Load(numPath)) as GameObject;
                Num.transform.SetParent(kahouIcon.transform, false);
                Num.GetComponent<Text>().text = dicChishikisyo[key].ToString();
                Num.name = "Num";

                //Kahou Status
                List<string> kahouInfoList = new List<string> ();
                KahouInfo KahouInfo = kahouIcon.GetComponent<KahouInfo>();
                kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
                KahouInfo.kahouId = int.Parse(kahouId);
                KahouInfo.kahouType = kahouTyp;
                KahouInfo.kahouName = kahouInfoList[0];
                KahouInfo.kahouTarget = kahouInfoList[2];
                KahouInfo.kahouEffect = int.Parse(kahouInfoList[3]);
                KahouInfo.kahouUnit = kahouInfoList[4];
                KahouInfo.kahouSell = int.Parse(kahouInfoList[6]);
                KahouInfo.qty = dicChishikisyo[key];
                kahouIcon.name = kahouTypId;
                doneKahouList.Add(kahouTypId);                
			}
		}

	}
}
