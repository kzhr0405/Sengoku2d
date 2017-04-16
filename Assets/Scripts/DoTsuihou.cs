using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class DoTsuihou : MonoBehaviour {

	public int busyoId;
	public string busyoName;
	
	public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();

		if (name == "YesButton") {
			/*Tsuihou*/
			//Limit Check
			int myBusyoQty = PlayerPrefs.GetInt("myBusyoQty");

			if (myBusyoQty == 1) {
				//Error
				audioSources [4].Play ();
				Message msg = new Message(); 
				msg.makeMessage (msg.getMessage(91));
				
			} else {
                    
                audioSources [4].Play ();
				//Delete Data
				//myBusyo
				List<string> myBusyo_list = new List<string> ();
				string myBusyoString = PlayerPrefs.GetString ("myBusyo");
				char[] delimiterChars = {','};
				myBusyo_list.AddRange (myBusyoString.Split (delimiterChars));
				myBusyo_list.Remove(busyoId.ToString());
				string newMyBusyoString = "";
				string newOnBusyo = myBusyo_list[0];
				for(int i=0; i<myBusyo_list.Count;i++){
					newMyBusyoString = newMyBusyoString + myBusyo_list[i] + ",";
				}
				newMyBusyoString = newMyBusyoString.TrimEnd(',');


				//kahou
				List<string> kahou_list = new List<string> ();
				string kahou = "kahou" + busyoId;
				string kahouString = PlayerPrefs.GetString (kahou);
				kahou_list.AddRange(kahouString.Split (delimiterChars));

				string availableBugu = PlayerPrefs.GetString ("availableBugu");
				string availableKabuto = PlayerPrefs.GetString ("availableKabuto");
				string availableGusoku = PlayerPrefs.GetString ("availableGusoku");
				string availableMeiba =  PlayerPrefs.GetString ("availableMeiba");
				string availableCyadougu = PlayerPrefs.GetString ("availableCyadougu");
				string availableHeihousyo = PlayerPrefs.GetString ("availableHeihousyo");
				string availableChishikisyo = PlayerPrefs.GetString ("availableChishikisyo");

				for(int j=0; j<kahou_list.Count;j++){
					string kahouId = kahou_list[j];

					if(j==0){
						//Bugu
						if(kahouId !="0"){
							if(availableBugu != "" && availableBugu != null){
								availableBugu = availableBugu + "," + kahouId;
							}else{
								availableBugu = kahouId;
							}
						}

					}else if(j==1){
						//Kabuto
						if(kahouId !="0"){
							if(availableKabuto != "" && availableKabuto != null){
								availableKabuto = availableKabuto + "," + kahouId;
							}else{
								availableKabuto = kahouId;
							}
						}

					}else if(j==2){
						//Gusoku
						if(kahouId !="0"){
							if(availableGusoku != "" && availableGusoku != null){
								availableGusoku = availableGusoku + "," + kahouId;
							}else{
								availableGusoku = kahouId;
							}
						}
					
					}else if(j==3){
						//Meiba
						if(kahouId !="0"){
							if(availableMeiba != "" && availableMeiba != null){
								availableMeiba = availableMeiba + "," + kahouId;
							}else{
								availableMeiba = kahouId;
							}
						}

					}else if(j==4){
						//Cyadougu1
						if(kahouId !="0"){
							if(availableCyadougu != "" && availableCyadougu != null){
								availableCyadougu = availableCyadougu + "," + kahouId;
							}else{
								availableCyadougu = kahouId;
							}
						}

					}else if(j==5){
						//Cyadougu2
						if(kahouId !="0"){
							if(availableCyadougu != "" && availableCyadougu != null){
								availableCyadougu = availableCyadougu + "," + kahouId;
							}else{
								availableCyadougu = kahouId;
							}
						}

					}else if(j==6){
						//Heihousyo
						if(kahouId !="0"){
							if(availableHeihousyo != "" && availableHeihousyo != null){
								availableHeihousyo = availableHeihousyo + "," + kahouId;
							}else{
								availableHeihousyo = kahouId;
							}
						}

					}else if(j==7){
						//Chishikisyo
						if(kahouId !="0"){
							if(availableChishikisyo != "" && availableChishikisyo != null){
								availableChishikisyo = availableChishikisyo + "," + kahouId;
							}else{
								availableChishikisyo = kahouId;
							}
						}
					}
				}

				//Kanni
				string kanniTmp = "kanni" + busyoId;
				if(PlayerPrefs.HasKey(kanniTmp)){
					DoRemoveKanni removeKanni = new DoRemoveKanni();
					removeKanni.removeKanni(busyoId.ToString());
				}

                //Jyosyu
                for (int i = 1; i < 66; i++) {
                    string jyosyuTemp = "jyosyu" + i.ToString();
                    int jyosyu = PlayerPrefs.GetInt(jyosyuTemp);
                    if(jyosyu == busyoId) {
                        PlayerPrefs.DeleteKey(jyosyuTemp);
                        break;
                    }
                }


                //OK
                PlayerPrefs.SetString("myBusyo",newMyBusyoString);
				PlayerPrefs.DeleteKey(busyoId.ToString());
				string hei = "hei" + busyoId;
				PlayerPrefs.DeleteKey(hei);
				string senpou = "senpou" + busyoId;
				PlayerPrefs.DeleteKey(senpou);
				string saku = "saku" + busyoId;
				PlayerPrefs.DeleteKey(saku);
                string jyosyuHei = "jyosyuHei" + busyoId;
                PlayerPrefs.DeleteKey(jyosyuHei);
                string jyosyuBusyo = "jyosyuBusyo" + busyoId;
                PlayerPrefs.DeleteKey(jyosyuBusyo);
                if (availableBugu != null) PlayerPrefs.SetString ("availableBugu",availableBugu);
				if(availableKabuto != null) PlayerPrefs.SetString ("availableKabuto",availableKabuto);
				if(availableGusoku != null) PlayerPrefs.SetString ("availableGusoku",availableGusoku);
				if(availableMeiba != null) PlayerPrefs.SetString ("availableMeiba",availableMeiba);
				if(availableCyadougu != null) PlayerPrefs.SetString ("availableCyadougu",availableCyadougu);
				if(availableHeihousyo != null) PlayerPrefs.SetString ("availableHeihousyo",availableHeihousyo);
				if(availableChishikisyo != null) PlayerPrefs.SetString ("availableChishikisyo",availableChishikisyo);
				PlayerPrefs.DeleteKey(kahou);
				string exp = "exp" + busyoId;
				PlayerPrefs.DeleteKey(exp);
				string gokui = "gokui" + busyoId;
				PlayerPrefs.DeleteKey(gokui);

				//jinkei 1map1 ~ 4map25
				int oyaId = 1;
				for(int k=oyaId; k<5; k++){
					int koId = 1;

					for(int l=koId; l<26; l++){
						string mapKey = k.ToString() + "map" + l.ToString();
						int mapBusyo = PlayerPrefs.GetInt (mapKey);

						if(mapBusyo == busyoId){
							//Delete
							PlayerPrefs.DeleteKey(mapKey);
						}
					}
				}
				myBusyoQty = myBusyoQty - 1;
				PlayerPrefs.SetInt("myBusyoQty",myBusyoQty);
				PlayerPrefs.Flush();

				//Back & Update
				Destroy(GameObject.Find("TsuihouConfirm"));
				Destroy(GameObject.Find("Back(Clone)"));

				MessageBusyo msg = new MessageBusyo();
                string tsuihouText = "";
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    tsuihouText = "Banished "+ busyoName + ".";
                }else {
                    tsuihouText = busyoName + "を追放しました。";
                }
				string type = "tsuihou"; 
				msg.makeMessage(tsuihouText,busyoId, type);

				//Now On Busyo Mod.
				GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyo = newOnBusyo;
				StatusGet sts = new StatusGet();
				GameObject.Find ("GameScene").GetComponent<NowOnBusyo>().OnBusyoName = sts.getBusyoName(int.Parse(newOnBusyo)); 

				/*Initialization*/
				//BusyoView
				RonkouScene ronkou = new RonkouScene();
				SyoguScene syogu = new SyoguScene();
				//Delete
				foreach ( Transform n in GameObject.Find("BusyoView").transform ){
					//Busyo Serihu
					GameObject.Destroy(n.gameObject);
				}
				//Create
				ronkou.createBusyoView(newOnBusyo.ToString());

				//BusyoStatus
				syogu.createSyoguView(newOnBusyo.ToString());

				//Scroll View
				//Delete
				foreach ( Transform n in GameObject.Find("Content").transform ){
					GameObject.Destroy(n.gameObject);
				}

				//Create
				ArrayList myBusyoList = new ArrayList();
				GameObject mainController = GameObject.Find ("GameScene");
				string minBusyoId = "";
				minBusyoId = ronkou.createScrollView(myBusyoList,minBusyoId,mainController,false);
			    
            }

		}else if(name == "NoButton"){
			//Back
			audioSources [1].Play ();
			Destroy(GameObject.Find("TsuihouConfirm"));
			Destroy(GameObject.Find("Back(Clone)"));
		}
    }




    
}
