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

		kahou.transform.FindChild ("Text").GetComponent<Text> ().color = pushedTextColor;
		dougu.transform.FindChild ("Text").GetComponent<Text> ().color = normalTextColor;

		/*Initialize Kahou View*/
		GameObject itemView = GameObject.Find ("ItemView");
		itemView.transform.FindChild ("GetMoney").GetComponent<Image>().enabled = false;
		GameObject sellBtn = GameObject.Find ("SellButton");
		sellBtn.GetComponent<Image>().enabled = false;
		sellBtn.GetComponent<Button>().enabled = false;
		sellBtn.transform.FindChild("Text").GetComponent<Text>().enabled = false;
		itemView.transform.FindChild ("KahouEffectValue").GetComponent<Text> ().text = "";
		itemView.transform.FindChild ("KahouEffectLabel").GetComponent<Text> ().text = "";
		GameObject.Find ("GetMoneyValue").GetComponent<Text> ().text = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            itemView.transform.FindChild ("ItemNameValue").GetComponent<Text> ().text = "Select Item";
        }else {
            itemView.transform.FindChild("ItemNameValue").GetComponent<Text>().text = "家宝選択";
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
			GameObject.Destroy(n.gameObject);
		}

		//Common Prametor
		char[] delimiterChars = {','};
		KahouStatusGet kahouStatus = new KahouStatusGet ();

		//availableBugu				
		string availableBuguString = PlayerPrefs.GetString("availableBugu");
		if(availableBuguString != null && availableBuguString !=""){
			string[] availableBugu_list = availableBuguString.Split (delimiterChars);

			for(int i=0; i<availableBugu_list.Length; i++){
				string kahouId = availableBugu_list[i];
				string kahouTyp = "bugu";
				string kahouTypId = kahouTyp + kahouId;
				string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

				//Kahou Status
				List<string> kahouInfoList = new List<string> ();
				kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
				kahouIcon.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
				kahouIcon.GetComponent<KahouInfo>().kahouType = kahouTyp;
				kahouIcon.GetComponent<KahouInfo>().kahouName = kahouInfoList[0];
				kahouIcon.GetComponent<KahouInfo>().kahouTarget = kahouInfoList[2];
				kahouIcon.GetComponent<KahouInfo>().kahouEffect = int.Parse(kahouInfoList[3]);
				kahouIcon.GetComponent<KahouInfo>().kahouUnit = kahouInfoList[4];
				kahouIcon.GetComponent<KahouInfo>().kahouSell = int.Parse(kahouInfoList[6]);
				kahouIcon.name = kahouTypId;
			}
		}

		//availableKabuto				
		string availableKabutoString = PlayerPrefs.GetString("availableKabuto");
		if(availableKabutoString != null && availableKabutoString !=""){
			string[] availableKabuto_list = availableKabutoString.Split (delimiterChars);
			
			for(int i=0; i<availableKabuto_list.Length; i++){
				string kahouId = availableKabuto_list[i];
				string kahouTyp = "kabuto";
				string kahouTypId = kahouTyp + kahouId;
				string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

				//Kahou Status
				List<string> kahouInfoList = new List<string> ();
				kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
				kahouIcon.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
				kahouIcon.GetComponent<KahouInfo>().kahouType = kahouTyp;
				kahouIcon.GetComponent<KahouInfo>().kahouName = kahouInfoList[0];
				kahouIcon.GetComponent<KahouInfo>().kahouTarget = kahouInfoList[2];
				kahouIcon.GetComponent<KahouInfo>().kahouEffect = int.Parse(kahouInfoList[3]);
				kahouIcon.GetComponent<KahouInfo>().kahouUnit = kahouInfoList[4];
				kahouIcon.GetComponent<KahouInfo>().kahouSell = int.Parse(kahouInfoList[6]);
				kahouIcon.name = kahouTypId;
			}
		}

		//availableGusoku				
		string availableGusokuString = PlayerPrefs.GetString("availableGusoku");
		if(availableGusokuString != null && availableGusokuString !=""){
			string[] availableGusoku_list = availableGusokuString.Split (delimiterChars);
			
			for(int i=0; i<availableGusoku_list.Length; i++){
				string kahouId = availableGusoku_list[i];
				string kahouTyp = "gusoku";
				string kahouTypId = kahouTyp + kahouId;
				string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

				//Kahou Status
				List<string> kahouInfoList = new List<string> ();
				kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
				kahouIcon.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
				kahouIcon.GetComponent<KahouInfo>().kahouType = kahouTyp;
				kahouIcon.GetComponent<KahouInfo>().kahouName = kahouInfoList[0];
				kahouIcon.GetComponent<KahouInfo>().kahouTarget = kahouInfoList[2];
				kahouIcon.GetComponent<KahouInfo>().kahouEffect = int.Parse(kahouInfoList[3]);
				kahouIcon.GetComponent<KahouInfo>().kahouUnit = kahouInfoList[4];
				kahouIcon.GetComponent<KahouInfo>().kahouSell = int.Parse(kahouInfoList[6]);
				kahouIcon.name = kahouTypId;

			}
		}

		//availableMeiba				
		string availableMeibaString = PlayerPrefs.GetString("availableMeiba");
		if(availableMeibaString != null && availableMeibaString !=""){
			string[] availableMeiba_list = availableMeibaString.Split (delimiterChars);
			
			for(int i=0; i<availableMeiba_list.Length; i++){
				string kahouId = availableMeiba_list[i];
				string kahouTyp = "meiba";
				string kahouTypId = kahouTyp + kahouId;
				string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

				//Kahou Status
				List<string> kahouInfoList = new List<string> ();
				kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
				kahouIcon.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
				kahouIcon.GetComponent<KahouInfo>().kahouType = kahouTyp;
				kahouIcon.GetComponent<KahouInfo>().kahouName = kahouInfoList[0];
				kahouIcon.GetComponent<KahouInfo>().kahouTarget = kahouInfoList[2];
				kahouIcon.GetComponent<KahouInfo>().kahouEffect = int.Parse(kahouInfoList[3]);
				kahouIcon.GetComponent<KahouInfo>().kahouUnit = kahouInfoList[4];
				kahouIcon.GetComponent<KahouInfo>().kahouSell = int.Parse(kahouInfoList[6]);
				kahouIcon.name = kahouTypId;

			}
		}

		//availableCyadougu				
		string availableCyadouguString = PlayerPrefs.GetString("availableCyadougu");
		if(availableCyadouguString != null && availableCyadouguString !=""){
			string[] availableCyadougu_list = availableCyadouguString.Split (delimiterChars);
			
			for(int i=0; i<availableCyadougu_list.Length; i++){
				string kahouId = availableCyadougu_list[i];
				string kahouTyp = "cyadougu";
				string kahouTypId = kahouTyp + kahouId;
				string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

				//Kahou Status
				List<string> kahouInfoList = new List<string> ();
				kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
				kahouIcon.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
				kahouIcon.GetComponent<KahouInfo>().kahouType = kahouTyp;
				kahouIcon.GetComponent<KahouInfo>().kahouName = kahouInfoList[0];
				kahouIcon.GetComponent<KahouInfo>().kahouTarget = kahouInfoList[2];
				kahouIcon.GetComponent<KahouInfo>().kahouEffect = int.Parse(kahouInfoList[3]);
				kahouIcon.GetComponent<KahouInfo>().kahouUnit = kahouInfoList[4];
				kahouIcon.GetComponent<KahouInfo>().kahouSell = int.Parse(kahouInfoList[6]);
				kahouIcon.name = kahouTypId;

			}
		}

		//availableHeihousyo				
		string availableHeihousyoString = PlayerPrefs.GetString("availableHeihousyo");
		if(availableHeihousyoString != null && availableHeihousyoString !=""){
			string[] availableHeihousyo_list = availableHeihousyoString.Split (delimiterChars);
			
			for(int i=0; i<availableHeihousyo_list.Length; i++){
				string kahouId = availableHeihousyo_list[i];
				string kahouTyp = "heihousyo";
				string kahouTypId = kahouTyp + kahouId;
				string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

				//Kahou Status
				List<string> kahouInfoList = new List<string> ();
				kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
				kahouIcon.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
				kahouIcon.GetComponent<KahouInfo>().kahouType = kahouTyp;
				kahouIcon.GetComponent<KahouInfo>().kahouName = kahouInfoList[0];
				kahouIcon.GetComponent<KahouInfo>().kahouTarget = kahouInfoList[2];
				kahouIcon.GetComponent<KahouInfo>().kahouEffect = int.Parse(kahouInfoList[3]);
				kahouIcon.GetComponent<KahouInfo>().kahouUnit = kahouInfoList[4];
				kahouIcon.GetComponent<KahouInfo>().kahouSell = int.Parse(kahouInfoList[6]);
				kahouIcon.name = kahouTypId;

			}
		}

		//availableChishikisyo				
		string availableChishikisyoString = PlayerPrefs.GetString("availableChishikisyo");
		if(availableChishikisyoString != null && availableChishikisyoString !=""){
			string[] availableChishikisyo_list = availableChishikisyoString.Split (delimiterChars);
			
			for(int i=0; i<availableChishikisyo_list.Length; i++){
				string kahouId = availableChishikisyo_list[i];
				string kahouTyp = "chishikisyo";
				string kahouTypId = kahouTyp + kahouId;
				string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
				GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
				kahouIcon.transform.SetParent(content.transform);
				kahouIcon.transform.localScale = new Vector2 (1, 1);
				kahouIcon.transform.localPosition = new Vector3 (0, 0, 0);

				//Kahou Status
				List<string> kahouInfoList = new List<string> ();
				kahouInfoList = kahouStatus.getKahouInfo(kahouTyp,int.Parse(kahouId));
				kahouIcon.GetComponent<KahouInfo>().kahouId = int.Parse(kahouId);
				kahouIcon.GetComponent<KahouInfo>().kahouType = kahouTyp;
				kahouIcon.GetComponent<KahouInfo>().kahouName = kahouInfoList[0];
				kahouIcon.GetComponent<KahouInfo>().kahouTarget = kahouInfoList[2];
				kahouIcon.GetComponent<KahouInfo>().kahouEffect = int.Parse(kahouInfoList[3]);
				kahouIcon.GetComponent<KahouInfo>().kahouUnit = kahouInfoList[4];
				kahouIcon.GetComponent<KahouInfo>().kahouSell = int.Parse(kahouInfoList[6]);
				kahouIcon.name = kahouTypId;

			}
		}

	}
}
