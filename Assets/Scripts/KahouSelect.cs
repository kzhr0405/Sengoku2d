using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class KahouSelect : MonoBehaviour {
	

	public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		GameObject mainController = GameObject.Find ("GameScene");
		string kahouType = this.name;

		//Back Cover
		string backPath = "Prefabs/Busyo/back";
		GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
		back.transform.SetParent(GameObject.Find ("Panel").transform);
		back.transform.localScale = new Vector2 (1, 1);
		RectTransform backTransform = back.GetComponent<RectTransform> ();
		backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

		//Popup Screen
		string popupPath = "Prefabs/Busyo/board";
		GameObject popup = Instantiate (Resources.Load (popupPath)) as GameObject;
		popup.transform.SetParent(GameObject.Find ("Panel").transform);
		popup.transform.localScale = new Vector2 (1, 1);
		RectTransform popupTransform = popup.GetComponent<RectTransform> ();
		popupTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

		//Get & Show Kaho Data
		if (kahouType == "NoBugu") {
			//Get Available Kahou List
			string availableBuguString = PlayerPrefs.GetString ("availableBugu");
			string target = "Bugu";

			if (availableBuguString == null || availableBuguString == "") {
				createMessage (target);
			} else {
				//Set Scroll View
				createScroll (target, availableBuguString);
			}
			mainController.GetComponent<NowOnButton> ().onKahouButton = this.transform.parent.gameObject.name;

		} else if (kahouType == "NoKabuto") {
			//Get Available Kahou List
			string availableKabutoString = PlayerPrefs.GetString ("availableKabuto");
			string target = "Kabuto";

			if (availableKabutoString == null || availableKabutoString == "") {
				createMessage (target);
			} else {
				//Set Scroll View
				createScroll (target, availableKabutoString);
			}
			mainController.GetComponent<NowOnButton> ().onKahouButton  = this.transform.parent.gameObject.name;

		} else if (kahouType == "NoGusoku") {
			//Get Available Kahou List
			string availableGusokuString = PlayerPrefs.GetString ("availableGusoku");
			string target = "Gusoku";
			
			if (availableGusokuString == null || availableGusokuString == "") {
				createMessage (target);
			} else {
				//Set Scroll View
				createScroll (target, availableGusokuString);
			}
			mainController.GetComponent<NowOnButton> ().onKahouButton  = this.transform.parent.gameObject.name;

		} else if (kahouType == "NoMeiba") {
			//Get Available Kahou List
			string availableMeibaString = PlayerPrefs.GetString ("availableMeiba");
			string target = "Meiba";
			
			if (availableMeibaString == null || availableMeibaString == "") {
				createMessage (target);
			} else {
				//Set Scroll View
				createScroll (target, availableMeibaString);
			}
			mainController.GetComponent<NowOnButton> ().onKahouButton  = this.transform.parent.gameObject.name;

		} else if (kahouType == "NoCyadougu") {
			//Get Available Kahou List
			string availableCyadouguString = PlayerPrefs.GetString ("availableCyadougu");
			string target = "Cyadougu";

			if (availableCyadouguString == null || availableCyadouguString == "") {
				createMessage (target);
			} else {
				//Set Scroll View
				createScroll (target, availableCyadouguString);
			}
			mainController.GetComponent<NowOnButton> ().onKahouButton  = this.transform.parent.gameObject.name;

		} else if (kahouType == "NoHeihousyo") {
			//Get Available Kahou List
			string availableHeihousyoString = PlayerPrefs.GetString ("availableHeihousyo");
			string target = "Heihousyo";
			if (availableHeihousyoString == null || availableHeihousyoString == "") {
				createMessage (target);
			} else {
				//Set Scroll View
				createScroll (target, availableHeihousyoString);
			}
			mainController.GetComponent<NowOnButton> ().onKahouButton  = this.transform.parent.gameObject.name;

		} else if (kahouType == "NoChishikisyo") {
			//Get Available Kahou List
			string availableChishikisyoString = PlayerPrefs.GetString ("availableChishikisyo");
			string target = "Chishikisyo";
			
			if (availableChishikisyoString == null || availableChishikisyoString == "") {
				createMessage (target);
			} else {
				//Set Scroll View
				createScroll (target, availableChishikisyoString);
			}
			mainController.GetComponent<NowOnButton> ().onKahouButton  = this.transform.parent.gameObject.name;
		}
	}

	public void createMessage(string target){
		string messagePath = "Prefabs/Busyo/Message";
		GameObject message = Instantiate (Resources.Load (messagePath)) as GameObject;
		message.transform.SetParent(GameObject.Find ("board(Clone)").transform);
		message.transform.localScale = new Vector2 (0.3f, 0.3f);
		RectTransform messageTransform = message.GetComponent<RectTransform> ();
		messageTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
        int langId = PlayerPrefs.GetInt("langId");
        string targetText =returnKahouName (target);

        //Text Modification
        string messageContent = "";
        if (langId == 2) {
            messageContent = "My lord, you don't have any " + targetText + ".";
        }else if(langId==3) {
            messageContent = "主公，没有" + targetText + "。";
        }
        else {
            messageContent = "御館様、" + targetText + "は御座りませぬ。";
        }
        //messageContent = messageContent.Replace("A", targetText);
		message.GetComponent<Text>().text = messageContent;
	}
	public string returnKahouName(string target){

        int langId = PlayerPrefs.GetInt("langId");
        string targetText = "";
        Message Message = new Message();
        targetText = "家宝";
        if (target == "Bugu") {
            targetText = Message.getMessage(255,langId);
        }else if (target == "Kabuto") {
            targetText = Message.getMessage(257, langId);
        }else if (target == "Gusoku") {
            targetText = Message.getMessage(256, langId);
        }else if (target == "Meiba") {
            targetText = Message.getMessage(258, langId);
        }else if (target == "Cyadougu") {
            targetText = Message.getMessage(259, langId);
        }else if (target == "Heihousyo") {
            targetText = Message.getMessage(261, langId);
        }else if (target == "Chishikisyo") {
            targetText = Message.getMessage(260, langId);
        }
        
		return targetText;
	}

	public void createScroll(string target, string availableKahou){
		/*Common*/
		//Set Scroll View
		string kahouScrollPath = "Prefabs/Busyo/KahouScrollView";
		GameObject kahouScroll = Instantiate (Resources.Load (kahouScrollPath)) as GameObject;
		kahouScroll.transform.SetParent(GameObject.Find ("board(Clone)").transform);
		kahouScroll.transform.localScale = new Vector2 (1, 1);
		kahouScroll.name = "KahouScrollView";
		RectTransform kahouScrollTransform = kahouScroll.GetComponent<RectTransform> ();
		kahouScrollTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
        int langId = PlayerPrefs.GetInt("langId");

        //Text Modification
        string targetText =returnKahouName (target);
        if (langId == 2) {
            GameObject.Find ("ScrollText").GetComponent<Text> ().text = targetText + " List";
        }else if(langId==3) {
            GameObject.Find("ScrollText").GetComponent<Text>().text = targetText + "一览";
        }else {
            GameObject.Find("ScrollText").GetComponent<Text>().text = targetText + "一覧";
        }

		if (0 <= availableKahou.IndexOf(",")){
			//more than 2
			
			char[] delimiterChars = {','};

			string[] available_list = availableKahou.Split (delimiterChars);

			for(int i=0;i<available_list.Length;i++){
				int kahouId = int.Parse(available_list[i]);
				string kahouTypId = target + kahouId.ToString();
				
				string kahouPath = "Prefabs/Busyo/KahouSlot";
				GameObject kahouSlot = Instantiate (Resources.Load (kahouPath)) as GameObject;
				kahouSlot.transform.SetParent(GameObject.Find ("KahouContent").transform);
				kahouSlot.transform.localScale = new Vector2 (1, 1);                
                
                if (target =="Bugu"){
					Entity_kahou_bugu_mst buguKahouMst  = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
                    string kahouName = "";
                    string kahouTarget = "";
                    if (langId == 2) {
                        kahouName = buguKahouMst.param [kahouId-1].kahouNameEng;
					    kahouTarget = buguKahouMst.param [kahouId-1].kahouTargetEng;
                    }else if(langId==3) {
                        kahouName = buguKahouMst.param[kahouId - 1].kahouNameSChn;
                        kahouTarget = buguKahouMst.param[kahouId - 1].kahouTargetSChn;
                    }
                    else {
                        kahouName = buguKahouMst.param[kahouId - 1].kahouName;
                        kahouTarget = buguKahouMst.param[kahouId - 1].kahouTarget;
                    }
                    int kahouEffect = buguKahouMst.param [kahouId-1].kahouEffect;
					string kahouUnit = buguKahouMst.param [kahouId-1].unit;
					kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
					kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
					kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text = "+" + kahouEffect.ToString() + kahouUnit;
					viewKahouIcon(kahouSlot,kahouTypId);
					setKahouInfo(kahouSlot,target,kahouId, kahouName, kahouTarget, kahouEffect, kahouUnit);
				}else if(target =="Kabuto"){
					Entity_kahou_kabuto_mst kabutoKahouMst  = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
                    string kahouName = "";
                    string kahouTarget = "";
                    if (langId == 2) {
                        kahouName = kabutoKahouMst.param[kahouId - 1].kahouNameEng;
                        kahouTarget = kabutoKahouMst.param[kahouId - 1].kahouTargetEng;
                    }
                    else if (langId == 3) {
                        kahouName = kabutoKahouMst.param[kahouId - 1].kahouNameSChn;
                        kahouTarget = kabutoKahouMst.param[kahouId - 1].kahouTargetSChn;
                    }
                    else {
                        kahouName = kabutoKahouMst.param[kahouId - 1].kahouName;
                        kahouTarget = kabutoKahouMst.param[kahouId - 1].kahouTarget;
                    }
                    int kahouEffect = kabutoKahouMst.param [kahouId-1].kahouEffect;
					string kahouUnit = kabutoKahouMst.param [kahouId-1].unit;
					kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
					kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
					kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text = "+" + kahouEffect.ToString() + kahouUnit;
					viewKahouIcon(kahouSlot,kahouTypId);
					setKahouInfo(kahouSlot,target,kahouId, kahouName, kahouTarget, kahouEffect, kahouUnit);

				}else if (target =="Gusoku"){
					Entity_kahou_gusoku_mst gusokuKahouMst  = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
                    string kahouName = "";
                    string kahouTarget = "";
                    if (langId == 2) {
                        kahouName = gusokuKahouMst.param[kahouId - 1].kahouNameEng;
                        kahouTarget = gusokuKahouMst.param[kahouId - 1].kahouTargetEng;
                    }
                    else if (langId == 3) {
                        kahouName = gusokuKahouMst.param[kahouId - 1].kahouNameSChn;
                        kahouTarget = gusokuKahouMst.param[kahouId - 1].kahouTargetSChn;
                    }
                    else {
                        kahouName = gusokuKahouMst.param[kahouId - 1].kahouName;
                        kahouTarget = gusokuKahouMst.param[kahouId - 1].kahouTarget;
                    }
                    int kahouEffect = gusokuKahouMst.param [kahouId-1].kahouEffect;
					string kahouUnit = gusokuKahouMst.param [kahouId-1].unit;
					kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
					kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
					kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text =  "+" + kahouEffect.ToString() + kahouUnit;
					viewKahouIcon(kahouSlot,kahouTypId);
					setKahouInfo(kahouSlot,target,kahouId, kahouName, kahouTarget, kahouEffect, kahouUnit);

				}else if (target =="Meiba"){
					Entity_kahou_meiba_mst meibaKahouMst  = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
                    string kahouName = "";
                    string kahouTarget = "";
                    if (langId == 2) {
                        kahouName = meibaKahouMst.param[kahouId - 1].kahouNameEng;
                        kahouTarget = meibaKahouMst.param[kahouId - 1].kahouTargetEng;
                    }
                    else if (langId == 3) {
                        kahouName = meibaKahouMst.param[kahouId - 1].kahouNameSChn;
                        kahouTarget = meibaKahouMst.param[kahouId - 1].kahouTargetSChn;
                    }
                    else {
                        kahouName = meibaKahouMst.param[kahouId - 1].kahouName;
                        kahouTarget = meibaKahouMst.param[kahouId - 1].kahouTarget;
                    }
                    int kahouEffect = meibaKahouMst.param [kahouId-1].kahouEffect;
					string kahouUnit = meibaKahouMst.param [kahouId-1].unit;
					kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
					kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
					kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text =  "+" + kahouEffect.ToString() + kahouUnit;
					viewKahouIcon(kahouSlot,kahouTypId);
					setKahouInfo(kahouSlot,target,kahouId, kahouName, kahouTarget, kahouEffect, kahouUnit);
				
				}else if (target =="Cyadougu"){
					Entity_kahou_cyadougu_mst cyadouguKahouMst  = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
                    string kahouName = "";
                    string kahouTarget = "";
                    if (langId == 2) {
                        kahouName = cyadouguKahouMst.param[kahouId - 1].kahouNameEng;
                        kahouTarget = cyadouguKahouMst.param[kahouId - 1].kahouTargetEng;
                    }
                    else if (langId == 3) {
                        kahouName = cyadouguKahouMst.param[kahouId - 1].kahouNameSChn;
                        kahouTarget = cyadouguKahouMst.param[kahouId - 1].kahouTargetSChn;

                    }
                    else {
                        kahouName = cyadouguKahouMst.param[kahouId - 1].kahouName;
                        kahouTarget = cyadouguKahouMst.param[kahouId - 1].kahouTarget;
                    }
                    int kahouEffect = cyadouguKahouMst.param [kahouId-1].kahouEffect;
					string kahouUnit = cyadouguKahouMst.param [kahouId-1].unit;
					kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
					kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
					kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text =  "+" + kahouEffect.ToString() + kahouUnit;
					viewKahouIcon(kahouSlot,kahouTypId);
					setKahouInfo(kahouSlot,target,kahouId, kahouName, kahouTarget, kahouEffect, kahouUnit);
				
				}else if (target =="Heihousyo"){
					Entity_kahou_heihousyo_mst heihousyoKahouMst  = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
                    string kahouName = "";
                    string kahouTarget = "";
                    if (langId == 2) {
                        kahouName = heihousyoKahouMst.param[kahouId - 1].kahouNameEng;
                        kahouTarget = heihousyoKahouMst.param[kahouId - 1].kahouTargetEng;
                    }
                    else if (langId == 3) {
                        kahouName = heihousyoKahouMst.param[kahouId - 1].kahouNameSChn;
                        kahouTarget = heihousyoKahouMst.param[kahouId - 1].kahouTargetSChn;
                    }
                    else {
                        kahouName = heihousyoKahouMst.param[kahouId - 1].kahouName;
                        kahouTarget = heihousyoKahouMst.param[kahouId - 1].kahouTarget;
                    }
                    int kahouEffect = heihousyoKahouMst.param [kahouId-1].kahouEffect;
					string kahouUnit = heihousyoKahouMst.param [kahouId-1].unit;
					kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
					kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
					kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text =  "+" + kahouEffect.ToString() + kahouUnit;
					viewKahouIcon(kahouSlot,kahouTypId);
					setKahouInfo(kahouSlot,target,kahouId, kahouName, kahouTarget, kahouEffect, kahouUnit);
				
				}else if (target =="Chishikisyo"){
					Entity_kahou_chishikisyo_mst chishikisyoKahouMst  = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
                    string kahouName = "";
                    string kahouTarget = "";
                    if (langId == 2) {
                        kahouName = chishikisyoKahouMst.param[kahouId - 1].kahouNameEng;
                        kahouTarget = chishikisyoKahouMst.param[kahouId - 1].kahouTargetEng;
                    }
                    else if (langId == 3) {
                        kahouName = chishikisyoKahouMst.param[kahouId - 1].kahouNameChn;
                        kahouTarget = chishikisyoKahouMst.param[kahouId - 1].kahouTargetChn;
                    }
                    else {
                        kahouName = chishikisyoKahouMst.param[kahouId - 1].kahouName;
                        kahouTarget = chishikisyoKahouMst.param[kahouId - 1].kahouTarget;
                    }
                    int kahouEffect = chishikisyoKahouMst.param [kahouId-1].kahouEffect;
					string kahouUnit = chishikisyoKahouMst.param [kahouId-1].unit;
					kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
					kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
					kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text =  "+" + kahouEffect.ToString() + kahouUnit;
					viewKahouIcon(kahouSlot,kahouTypId);
					setKahouInfo(kahouSlot,target,kahouId, kahouName, kahouTarget, kahouEffect, kahouUnit);
				}
			}
		}else{
			//only 1
			string kahouTypId = target + availableKahou;
			
			string kahouPath = "Prefabs/Busyo/KahouSlot";
			GameObject kahouSlot = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahouSlot.transform.SetParent(GameObject.Find ("KahouContent").transform);
			kahouSlot.transform.localScale = new Vector2 (1, 1);

			if(target =="Bugu"){
				Entity_kahou_bugu_mst buguKahouMst  = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
                string kahouName = "";
                string kahouTarget = "";
                if (langId == 2) {
                    kahouName = buguKahouMst.param [int.Parse(availableKahou)-1].kahouNameEng;
				    kahouTarget = buguKahouMst.param [int.Parse(availableKahou)-1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = buguKahouMst.param[int.Parse(availableKahou) - 1].kahouNameSChn;
                    kahouTarget = buguKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetSChn;

                }
                else {
                    kahouName = buguKahouMst.param[int.Parse(availableKahou) - 1].kahouName;
                    kahouTarget = buguKahouMst.param[int.Parse(availableKahou) - 1].kahouTarget;
                }
				int kahouEffect = buguKahouMst.param [int.Parse(availableKahou)-1].kahouEffect;	
				string kahouUnit = buguKahouMst.param [int.Parse(availableKahou)-1].unit;
				kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
				kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
				kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text =  "+" + kahouEffect.ToString() + kahouUnit;
				viewKahouIcon(kahouSlot,kahouTypId);
				setKahouInfo(kahouSlot,target,int.Parse(availableKahou), kahouName, kahouTarget, kahouEffect, kahouUnit);

			}else if(target =="Kabuto"){
				Entity_kahou_kabuto_mst kabutoKahouMst  = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
                string kahouName = "";
                string kahouTarget = "";
                if (langId == 2) {
                    kahouName = kabutoKahouMst.param[int.Parse(availableKahou) - 1].kahouNameEng;
                    kahouTarget = kabutoKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = kabutoKahouMst.param[int.Parse(availableKahou) - 1].kahouNameSChn;
                    kahouTarget = kabutoKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetSChn;

                }
                else {
                    kahouName = kabutoKahouMst.param[int.Parse(availableKahou) - 1].kahouName;
                    kahouTarget = kabutoKahouMst.param[int.Parse(availableKahou) - 1].kahouTarget;
                }
                int kahouEffect = kabutoKahouMst.param [int.Parse(availableKahou)-1].kahouEffect;
				string kahouUnit = kabutoKahouMst.param [int.Parse(availableKahou)-1].unit;
				kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
				kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
				kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text =  "+" + kahouEffect.ToString() + kahouUnit;
				viewKahouIcon(kahouSlot,kahouTypId);
				setKahouInfo(kahouSlot,target,int.Parse(availableKahou), kahouName, kahouTarget, kahouEffect, kahouUnit);

			}else if (target =="Gusoku"){
				Entity_kahou_gusoku_mst gusokuKahouMst  = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
                string kahouName = "";
                string kahouTarget = "";
                if (langId == 2) {
                    kahouName = gusokuKahouMst.param[int.Parse(availableKahou) - 1].kahouNameEng;
                    kahouTarget = gusokuKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = gusokuKahouMst.param[int.Parse(availableKahou) - 1].kahouNameSChn;
                    kahouTarget = gusokuKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetSChn;
                } else {
                    kahouName = gusokuKahouMst.param[int.Parse(availableKahou) - 1].kahouName;
                    kahouTarget = gusokuKahouMst.param[int.Parse(availableKahou) - 1].kahouTarget;
                }
                int kahouEffect = gusokuKahouMst.param [int.Parse(availableKahou)-1].kahouEffect;
				string kahouUnit = gusokuKahouMst.param [int.Parse(availableKahou)-1].unit;
				kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
				kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
				kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text = "+" + kahouEffect.ToString() + kahouUnit;
				viewKahouIcon(kahouSlot,kahouTypId);
				setKahouInfo(kahouSlot,target,int.Parse(availableKahou), kahouName, kahouTarget, kahouEffect, kahouUnit);

			}else if (target =="Meiba"){
				Entity_kahou_meiba_mst meibaKahouMst  = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
                string kahouName = "";
                string kahouTarget = "";
                if (langId == 2) {
                    kahouName = meibaKahouMst.param[int.Parse(availableKahou) - 1].kahouNameEng;
                    kahouTarget = meibaKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = meibaKahouMst.param[int.Parse(availableKahou) - 1].kahouNameSChn;
                    kahouTarget = meibaKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetSChn;
                }
                else {
                    kahouName = meibaKahouMst.param[int.Parse(availableKahou) - 1].kahouName;
                    kahouTarget = meibaKahouMst.param[int.Parse(availableKahou) - 1].kahouTarget;
                }
                int kahouEffect = meibaKahouMst.param [int.Parse(availableKahou)-1].kahouEffect;
				string kahouUnit = meibaKahouMst.param [int.Parse(availableKahou)-1].unit;
				kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
				kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
				kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text = "+" + kahouEffect.ToString() + kahouUnit;
				viewKahouIcon(kahouSlot,kahouTypId);
				setKahouInfo(kahouSlot,target,int.Parse(availableKahou), kahouName, kahouTarget, kahouEffect, kahouUnit);

			}else if (target =="Cyadougu"){
				Entity_kahou_cyadougu_mst cyadouguKahouMst  = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
                string kahouName = "";
                string kahouTarget = "";
                if (langId == 2) {
                    kahouName = cyadouguKahouMst.param[int.Parse(availableKahou) - 1].kahouNameEng;
                    kahouTarget = cyadouguKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = cyadouguKahouMst.param[int.Parse(availableKahou) - 1].kahouNameSChn;
                    kahouTarget = cyadouguKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetSChn;
                }
                else {
                    kahouName = cyadouguKahouMst.param[int.Parse(availableKahou) - 1].kahouName;
                    kahouTarget = cyadouguKahouMst.param[int.Parse(availableKahou) - 1].kahouTarget;
                }
                int kahouEffect = cyadouguKahouMst.param [int.Parse(availableKahou)-1].kahouEffect;
				string kahouUnit = cyadouguKahouMst.param [int.Parse(availableKahou)-1].unit;
				kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
				kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
				kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text = "+" + kahouEffect.ToString() + kahouUnit;
				viewKahouIcon(kahouSlot,kahouTypId);
				setKahouInfo(kahouSlot,target,int.Parse(availableKahou), kahouName, kahouTarget, kahouEffect, kahouUnit);
				
			}else if (target =="Heihousyo"){
				Entity_kahou_heihousyo_mst heihousyoKahouMst  = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
                string kahouName = "";
                string kahouTarget = "";
                if (langId == 2) {
                    kahouName = heihousyoKahouMst.param[int.Parse(availableKahou) - 1].kahouNameEng;
                    kahouTarget = heihousyoKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = heihousyoKahouMst.param[int.Parse(availableKahou) - 1].kahouNameSChn;
                    kahouTarget = heihousyoKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetSChn;
                }
                else {
                    kahouName = heihousyoKahouMst.param[int.Parse(availableKahou) - 1].kahouName;
                    kahouTarget = heihousyoKahouMst.param[int.Parse(availableKahou) - 1].kahouTarget;
                }
                int kahouEffect = heihousyoKahouMst.param [int.Parse(availableKahou)-1].kahouEffect;
				string kahouUnit = heihousyoKahouMst.param [int.Parse(availableKahou)-1].unit;
				kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
				kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
				kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text = "+" + kahouEffect.ToString() + kahouUnit;
				viewKahouIcon(kahouSlot,kahouTypId);
				setKahouInfo(kahouSlot,target,int.Parse(availableKahou), kahouName, kahouTarget, kahouEffect, kahouUnit);
				
			}else if (target =="Chishikisyo"){
				Entity_kahou_chishikisyo_mst chishikisyoKahouMst  = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
                string kahouName = "";
                string kahouTarget = "";
                if (langId == 2) {
                    kahouName = chishikisyoKahouMst.param[int.Parse(availableKahou) - 1].kahouNameEng;
                    kahouTarget = chishikisyoKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = chishikisyoKahouMst.param[int.Parse(availableKahou) - 1].kahouNameChn;
                    kahouTarget = chishikisyoKahouMst.param[int.Parse(availableKahou) - 1].kahouTargetChn;
                }
                else {
                    kahouName = chishikisyoKahouMst.param[int.Parse(availableKahou) - 1].kahouName;
                    kahouTarget = chishikisyoKahouMst.param[int.Parse(availableKahou) - 1].kahouTarget;
                }
                int kahouEffect = chishikisyoKahouMst.param [int.Parse(availableKahou)-1].kahouEffect;
				string kahouUnit = chishikisyoKahouMst.param [int.Parse(availableKahou)-1].unit;
				kahouSlot.transform.Find("KahouName").GetComponent<Text>().text = kahouName;
				kahouSlot.transform.Find("KahouName/KahouEffectLabel").GetComponent<Text>().text = kahouTarget;
				kahouSlot.transform.Find("KahouName/KahouEffectValue").GetComponent<Text>().text = "+" + kahouEffect.ToString() + kahouUnit;
				viewKahouIcon(kahouSlot,kahouTypId);
				setKahouInfo(kahouSlot,target,int.Parse(availableKahou), kahouName, kahouTarget, kahouEffect, kahouUnit);
				
			}
		}		
	}

	public void viewKahouIcon(GameObject obj,string kahouTypId){
		string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
		GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
		kahouIcon.transform.SetParent(obj.transform.Find("KahouName").transform);

		RectTransform iconTransform = kahouIcon.GetComponent<RectTransform> ();
		iconTransform.anchoredPosition3D = new Vector3 (0, -500, 0);
		iconTransform.sizeDelta = new Vector2 (40, 40);
		kahouIcon.transform.localScale = new Vector2 (15, 15);

		kahouIcon.GetComponent<Button> ().enabled = false;

		//Text Modification
		RectTransform rankTransform = kahouIcon.transform.Find ("Rank").GetComponent<RectTransform> ();
		rankTransform.anchoredPosition3D = new Vector3 (10, -10, 0);
		rankTransform.sizeDelta = new Vector2 (40, 40);
		rankTransform.transform.localScale = new Vector2 (0.1f, 0.1f);

	}
	public void setKahouInfo(GameObject obj, string kahouType, int kahouId, string kahouName, string kahouTypName, int kahouEffect, string kahouUnit) {

        EquipKahou EquipKahou = obj.transform.Find("KahouName/EquipButton").GetComponent<EquipKahou>();

        EquipKahou.kahouId = kahouId;
        EquipKahou.kahouType = kahouType;
        EquipKahou.kahouName = kahouName;
        EquipKahou.kahouTypeName = kahouTypName;
        EquipKahou.kahouEffect = kahouEffect;
        EquipKahou.kahouUnit = kahouUnit;
    }
}
