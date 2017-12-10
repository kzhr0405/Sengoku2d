using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class KahouInfo : MonoBehaviour {

    //bugu,kabuto,gusoku,meiba,cyadougu,heihousyo,chisikisyo
    public int rank;
    public int kahouId;
	public string kahouType;
	public string kahouName;
	public string kahouTarget;
	public string kahouExp;
	public int kahouEffect;
	public string kahouUnit;
	public int kahouBuy;
	public int kahouSell;
	public float kahouRatio;
    public int qty;

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		if (Application.loadedLevelName == "souko") {

            //Slider
            GameObject.Find("Background").GetComponent<Image>().enabled = true;
            GameObject.Find("Fill").GetComponent<Image>().enabled = true;
            GameObject.Find("Handle").GetComponent<Image>().enabled = true;
            GameObject.Find("SellQty").GetComponent<Image>().enabled = true;

            GameObject SellSlider = GameObject.Find ("SellSlider");
            int maxSellQty = 9999999/ kahouSell;
            if(maxSellQty < qty) {
                qty = maxSellQty;
            }
            SellSlider.GetComponent<Slider>().maxValue = qty;
            SellSlider.GetComponent<Slider>().minValue = 1;
            SellSlider.GetComponent<SellSlider>().unitPrice = kahouSell;


            //Default value
            GameObject.Find("SellQtyValue").GetComponent<Text>().text = "1";
            GameObject.Find("GetMoneyValue").GetComponent<Text>().text = "+" + kahouSell;
            SellSlider.GetComponent<Slider>().value = 1;

            //Souko Scene
            GameObject.Find ("GetMoney").GetComponent<Image> ().enabled = true;
			GameObject sellBtn = GameObject.Find ("SellButton");
			sellBtn.GetComponent<Image> ().enabled = true;
			sellBtn.GetComponent<Button> ().enabled = true;
			sellBtn.transform.Find ("Text").GetComponent<Text> ().enabled = true;

			GameObject.Find ("ItemNameValue").GetComponent<Text> ().text = kahouName;
			GameObject.Find ("KahouEffectLabel").GetComponent<Text> ().text = kahouTarget;
			string effect = "+" + kahouEffect + kahouUnit;
			GameObject.Find ("KahouEffectValue").GetComponent<Text> ().text = effect;
			string sell = "+" + kahouSell;
			GameObject.Find ("GetMoneyValue").GetComponent<Text> ().text = sell;

			//Delete Previous Icon
			GameObject itemView = GameObject.Find ("ItemView");
			foreach (Transform n in itemView.transform) {
				if (n.tag == "Kahou") {
					GameObject.Destroy (n.gameObject);
				}
			}
			string kahouIconPath = "Prefabs/Item/Kahou/" + name;
			GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
			kahouIcon.transform.SetParent (itemView.transform);
			kahouIcon.transform.localScale = new Vector2 (1, 1);
			RectTransform kahouTransform = kahouIcon.GetComponent<RectTransform> ();
			kahouTransform.anchoredPosition3D = new Vector3 (0, 120, 0);

			kahouIcon.GetComponent<Button> ().enabled = false;

			//Sell Button Set
			sellBtn.GetComponent<DoSell> ().kahouId = kahouId;
			sellBtn.GetComponent<DoSell> ().kahouName = kahouName;
			sellBtn.GetComponent<DoSell> ().kahouType = kahouType;
			sellBtn.GetComponent<DoSell> ().kahouSell = kahouSell;


		} else if (Application.loadedLevelName == "busyo") {
			//Busyo Kahou Scene

			GameObject mainController = GameObject.Find ("GameScene");

			mainController.GetComponent<NowOnButton> ().onKahouButton = this.transform.parent.gameObject.name;

			/*Board with Current Kahou Info*/
			//Back Cover
			string backPath = "Prefabs/Busyo/back";
			GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
			back.transform.SetParent (GameObject.Find ("Panel").transform);
			back.transform.localScale = new Vector2 (1, 1);
			RectTransform backTransform = back.GetComponent<RectTransform> ();
			backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

			//Popup Screen
			string popupPath = "Prefabs/Busyo/equipedKahouBoard";
			GameObject popup = Instantiate (Resources.Load (popupPath)) as GameObject;
			popup.transform.SetParent (GameObject.Find ("Panel").transform);
			popup.transform.localScale = new Vector2 (1, 1);
			RectTransform popupTransform = popup.GetComponent<RectTransform> ();
			popupTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

            //Get Kahou Data
            int langId = PlayerPrefs.GetInt("langId");

            if (kahouType == "bugu") {
				Entity_kahou_bugu_mst Mst = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
                if (langId == 2) {
                    kahouName = Mst.param[kahouId - 1].kahouNameEng;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetEng;
                }else if(langId==3) {
                    kahouName = Mst.param[kahouId - 1].kahouNameSChn;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetSChn;
                } else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                    kahouTarget = Mst.param[kahouId - 1].kahouTarget;
                }				
				kahouEffect = Mst.param [kahouId - 1].kahouEffect;	
				kahouUnit = Mst.param [kahouId - 1].unit;
                if (langId == 2) {
                    kahouExp = Mst.param [kahouId - 1].kahouExpEng;
                }else if(langId==3) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpSChn;
                }
                else {
                    kahouExp = Mst.param[kahouId - 1].kahouExp;
                }

			} else if (kahouType == "kabuto") {
				Entity_kahou_kabuto_mst Mst = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
                if (langId == 2) {
                    kahouName = Mst.param[kahouId - 1].kahouNameEng;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = Mst.param[kahouId - 1].kahouNameSChn;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetSChn;
                }
                else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                    kahouTarget = Mst.param[kahouId - 1].kahouTarget;
                }
                kahouEffect = Mst.param [kahouId - 1].kahouEffect;	
				kahouUnit = Mst.param [kahouId - 1].unit;
                if (langId == 2) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpEng;
                }
                else if (langId == 3) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpSChn;
                }
                else {
                    kahouExp = Mst.param[kahouId - 1].kahouExp;
                }

            } else if (kahouType == "gusoku") {
				Entity_kahou_gusoku_mst Mst = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
                if (langId == 2) {
                    kahouName = Mst.param[kahouId - 1].kahouNameEng;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = Mst.param[kahouId - 1].kahouNameSChn;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetSChn;
                }
                else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                    kahouTarget = Mst.param[kahouId - 1].kahouTarget;
                }
                kahouEffect = Mst.param [kahouId - 1].kahouEffect;	
				kahouUnit = Mst.param [kahouId - 1].unit;
                if (langId == 2) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpEng;
                }
                else if (langId == 3) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpSChn;
                }
                else {
                    kahouExp = Mst.param[kahouId - 1].kahouExp;
                }

            } else if (kahouType == "meiba") {
				Entity_kahou_meiba_mst Mst = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
                if (langId == 2) {
                    kahouName = Mst.param[kahouId - 1].kahouNameEng;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = Mst.param[kahouId - 1].kahouNameSChn;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetSChn;
                }
                else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                    kahouTarget = Mst.param[kahouId - 1].kahouTarget;
                }
                kahouEffect = Mst.param [kahouId - 1].kahouEffect;	
				kahouUnit = Mst.param [kahouId - 1].unit;
                if (langId == 2) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpEng;
                }
                else if (langId == 3) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpSChn;
                }
                else {
                    kahouExp = Mst.param[kahouId - 1].kahouExp;
                }

            } else if (kahouType == "cyadougu") {
				Entity_kahou_cyadougu_mst Mst = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
                if (langId == 2) {
                    kahouName = Mst.param[kahouId - 1].kahouNameEng;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = Mst.param[kahouId - 1].kahouNameSChn;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetSChn;
                }
                else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                    kahouTarget = Mst.param[kahouId - 1].kahouTarget;
                }
                kahouEffect = Mst.param [kahouId - 1].kahouEffect;	
				kahouUnit = Mst.param [kahouId - 1].unit;
                if (langId == 2) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpEng;
                }
                else if (langId == 3) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpSChn;
                }
                else {
                    kahouExp = Mst.param[kahouId - 1].kahouExp;
                }

            } else if (kahouType == "heihousyo") {
				Entity_kahou_heihousyo_mst Mst = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
                if (langId == 2) {
                    kahouName = Mst.param[kahouId - 1].kahouNameEng;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = Mst.param[kahouId - 1].kahouNameSChn;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetSChn;
                }
                else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                    kahouTarget = Mst.param[kahouId - 1].kahouTarget;
                }
                kahouEffect = Mst.param [kahouId - 1].kahouEffect;	
				kahouUnit = Mst.param [kahouId - 1].unit;
                if (langId == 2) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpEng;
                }
                else if (langId == 3) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpSChn;
                }
                else {
                    kahouExp = Mst.param[kahouId - 1].kahouExp;
                }

            } else if (kahouType == "chishikisyo") {
				Entity_kahou_chishikisyo_mst Mst = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
                if (langId == 2) {
                    kahouName = Mst.param[kahouId - 1].kahouNameEng;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetEng;
                }
                else if (langId == 3) {
                    kahouName = Mst.param[kahouId - 1].kahouNameChn;
                    kahouTarget = Mst.param[kahouId - 1].kahouTargetChn;
                }
                else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                    kahouTarget = Mst.param[kahouId - 1].kahouTarget;
                }
                kahouEffect = Mst.param [kahouId - 1].kahouEffect;	
				kahouUnit = Mst.param [kahouId - 1].unit;
                if (langId == 2) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpEng;
                }
                else if (langId == 3) {
                    kahouExp = Mst.param[kahouId - 1].kahouExpChn;
                }
                else {
                    kahouExp = Mst.param[kahouId - 1].kahouExp;
                }

            }		

			//Refrect Kahou info.
			GameObject.Find ("equipedKahouName").GetComponent<Text> ().text = kahouName;
			GameObject.Find ("equipedKahouEffectLabel").GetComponent<Text> ().text = kahouTarget;
			GameObject.Find ("equipedKahouEffectValue").GetComponent<Text> ().text = "+" + kahouEffect + kahouUnit;
			GameObject.Find ("equipedKahouExpValue").GetComponent<Text> ().text = kahouExp;
			string kahouTypId = kahouType + kahouId;
			string kahouPath = "Prefabs/Item/Kahou/" + kahouTypId;
			GameObject kahou = Instantiate (Resources.Load (kahouPath)) as GameObject;
			kahou.transform.SetParent (GameObject.Find ("equipedKahouBoard(Clone)").transform);

			kahou.transform.localScale = new Vector3 (1.2f, 1.2f, 0);
			RectTransform kahouTransform = kahou.GetComponent<RectTransform> ();
			kahouTransform.anchoredPosition3D = new Vector3 (-360, 0, 0);

			kahou.GetComponent<Button> ().enabled = false;

            //Send Param Delete Button
            DeleteKahou DeleteKahou = GameObject.Find("DeleteButton").GetComponent<DeleteKahou>();

            DeleteKahou.kahouType = kahouType;
            DeleteKahou.kahouId = kahouId;
            DeleteKahou.kahouName = kahouName;
            DeleteKahou.kahouTypeName = kahouTarget;
            DeleteKahou.kahouEffect = kahouEffect;
            DeleteKahou.kahouUnit = kahouUnit;

        } else if (Application.loadedLevelName == "zukan") {

			string pathOfBack = "Prefabs/Common/TouchBack";
			GameObject back = Instantiate (Resources.Load (pathOfBack)) as GameObject;
			back.transform.parent = GameObject.Find ("Panel").transform;
			back.transform.localScale = new Vector2 (1, 1);
			back.transform.localPosition = new Vector2 (0, 0);
			
			string pathOfPop = "Prefabs/Zukan/kahouPop";
			GameObject pop = Instantiate (Resources.Load (pathOfPop)) as GameObject;
			pop.transform.parent = GameObject.Find ("Panel").transform;
			pop.transform.localScale = new Vector2 (1, 1);
			pop.transform.localPosition = new Vector2 (0, 0);

			//Get Data
			string kahouTypId = kahouType + kahouId.ToString();
			string kahouIconPath = "Prefabs/Item/Kahou/" + kahouTypId;
			GameObject kahouIcon = Instantiate (Resources.Load (kahouIconPath)) as GameObject;
			kahouIcon.transform.SetParent(pop.transform);
			kahouIcon.transform.localScale = new Vector2 (1.5f, 1.5f);
			RectTransform rectIcon = kahouIcon.GetComponent<RectTransform>();
			rectIcon.anchoredPosition3D = new Vector3(-130,90,0);
			kahouIcon.GetComponent<Button>().enabled = false;

			KahouStatusGet kahou = new KahouStatusGet();
			List<string> kahouInfoList = new List<string> ();
			kahouInfoList = kahou.getKahouInfo(kahouType,kahouId);

			GameObject.Find("kahouNameValue").GetComponent<Text>().text = kahouInfoList[0];
			string kahouTypeName = "";
            int langId = PlayerPrefs.GetInt("langId");
            Message Message = new Message();
            if (kahouType == "bugu") {
                kahouTypeName = Message.getMessage(255,langId);
            }else if (kahouType == "gusoku") {
                kahouTypeName = Message.getMessage(256,langId);
            }else if (kahouType == "kabuto") {
                kahouTypeName = Message.getMessage(257,langId);
            }else if (kahouType == "meiba") {
                kahouTypeName =Message.getMessage(258,langId);
            }else if (kahouType == "cyadougu") {
                kahouTypeName = Message.getMessage(259,langId);
            }else if (kahouType == "chishikisyo") {
                kahouTypeName =Message.getMessage(260,langId);
            }else if (kahouType == "heihousyo") {
                kahouTypeName =Message.getMessage(261,langId);
            }
            
			GameObject.Find("kahouTypValue").GetComponent<Text>().text = kahouTypeName;
			GameObject.Find("EffectTitle").GetComponent<Text>().text = kahouInfoList[2];
			string effect = "+" + kahouInfoList[3] + kahouInfoList[4];
			GameObject.Find("EffectValue").GetComponent<Text>().text = effect;
			GameObject.Find("ExpValue").GetComponent<Text>().text = kahouInfoList[1];
			

		}
	}
}	
