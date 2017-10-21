using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class TabibitoNoticeBtn : MonoBehaviour {

	public string targetRank = "";
	public string targetGrp = "";
	public int targetGrpId = 0;
	public string targetName = "";
	public string targetExp = "";
	public string itemCd = "";
	public int itemId = 0;
	public int itemQty = 0;

	public void OnClick(){

        onOffTabibitoMove(true);
        int langId = PlayerPrefs.GetInt("langId");

        AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [0].Play ();

		string pathOfBack = "Prefabs/Busyo/Back";
		GameObject back = Instantiate(Resources.Load (pathOfBack)) as GameObject;
		back.transform.parent = GameObject.Find ("Panel").transform;
		back.transform.localScale = new Vector2 (1,1);
		back.transform.localPosition = new Vector2 (0,0);

		string pathOfBoard = "Prefabs/Naisei/Tabibito/TabibitoBoard";
		GameObject board = Instantiate(Resources.Load (pathOfBoard)) as GameObject;
		board.transform.parent = GameObject.Find ("Panel").transform;
		board.transform.localScale = new Vector2 (1,1);
		board.transform.localPosition = new Vector3 (0,0,0);

		board.transform.FindChild ("GrpValue").GetComponent<Text> ().text = targetGrp;
		board.transform.FindChild ("Name").transform.FindChild ("NameValue").GetComponent<Text> ().text = targetName;
		board.transform.FindChild ("Rank").transform.FindChild ("RankValue").GetComponent<Text> ().text = targetRank;
		board.transform.FindChild ("Serihu").transform.FindChild ("SerihuValue").GetComponent<Text> ().text = targetExp;
	
		board.transform.FindChild ("Image").GetComponent<Image> ().sprite = gameObject.transform.parent.gameObject.GetComponent<Image> ().sprite;
        board.transform.FindChild("close").GetComponent<CloseBoard>().tabibitoNoticeBtnFlg = true;
        GameObject.Find("NaiseiController").GetComponent<NaiseiController>().stopFlg = true;


        //Find Item Icon
        string addPath = itemCd;
		if (itemCd == "nanban") {
				itemCd = itemCd + itemId;
				addPath = itemCd;
		}

		bool kahouFlg = false;
		if (itemCd == "bugu"||itemCd == "kabuto"||itemCd == "gusoku"||itemCd == "meiba"||itemCd == "cyadougu"||itemCd == "chishikisyo"||itemCd == "heihousyo"){
			//kahou item
			addPath = "Kahou/" + itemCd + itemId; 
			kahouFlg = true;
		}

		string pathOfItem = "Prefabs/Item/" + addPath;
		GameObject item = Instantiate(Resources.Load (pathOfItem)) as GameObject;
		item.transform.SetParent(board.transform);
		if(!kahouFlg){
			item.transform.localScale = new Vector2 (1,1);
		}else{
			item.transform.localScale = new Vector2 (0.65f,0.65f);
			item.GetComponent<Button>().enabled = false;
		}
		item.transform.localPosition = new Vector3 (-60,-100,0);

		board.transform.FindChild ("Qty").GetComponent<Text> ().text = "x "+itemQty.ToString();


		/*Adjustment*/
		if (itemCd == "CyouheiTP") {
			Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
			Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
			Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);
			if(itemId==1){
                //Ge
                if (langId == 2) {
                    item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "Low";
                }else {
                    item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "下";
                }
				item.GetComponent<Image>().color = lowColor;
			}else if(itemId==2){
                //Cyu
                if (langId == 2) {
                    item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "Mid";
                }else {
                    item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "中";
                }
				item.GetComponent<Image>().color = midColor;
			}else if(itemId==3){
                //Jyo
                if (langId == 2) {
                    item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "High";
                }else {
                    item.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "上";
                }
				item.GetComponent<Image>().color = highColor;
			}
		}
		//Syoukaijyo
		if (itemCd == "cyoutei" || itemCd == "koueki") {
			item.transform.FindChild("Name").GetComponent<Text>().text = targetName;

			if(itemCd == "cyoutei"){
				if(targetRank == "S"){
					itemId = 3;
                    if (langId == 2) {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "High";
                    }else {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "上";
                    }
				}else if(targetRank == "A"){
					itemId = 2;
                    if (langId == 2) {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "Mid";
                    }else {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "中";
                    }
				}else if(targetRank == "B"){
					itemId = 1;
                    if (langId == 2) {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "Low";
                    }else {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "下";
                    }
				}
			}else if(itemCd == "koueki"){
				if(targetRank == "A"){
					itemId = 3;
                    if (langId == 2) {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "High";
                    }else {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "上";
                    }
				}else if(targetRank == "B"){
					itemId = 2;
                    if (langId == 2) {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "Mid";
                    }else {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "中";
                    }
				}else if(targetRank == "C"){
					itemId = 1;
                    if (langId == 2) {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "Low";
                    }else {
                        item.transform.FindChild("Rank").GetComponent<Text>().text = "下";
                    }
				}
			}
		}

		//Kengou
		if (itemCd == "kengou") {
			if(targetRank=="A"){
                if (langId == 2) {
                    item.transform.FindChild("Rank").GetComponent<Text>().text = "High";
                }else {
                    item.transform.FindChild("Rank").GetComponent<Text>().text = "上";
                }
			}else if(targetRank=="B"){
                if (langId == 2) {
                    item.transform.FindChild("Rank").GetComponent<Text>().text = "Mid";
                }else {
                    item.transform.FindChild("Rank").GetComponent<Text>().text = "中";
                }
			}else if(targetRank=="C"){
                if (langId == 2) {
                    item.transform.FindChild("Rank").GetComponent<Text>().text = "Low";
                }else {
                    item.transform.FindChild("Rank").GetComponent<Text>().text = "下";
                }
			}
		}


		//Btn Parametor Setting
		TabibitoItemGetter script = board.transform.FindChild ("GetButton").GetComponent<TabibitoItemGetter> ();
		script.itemCd = itemCd;
		script.itemId = itemId;
		script.itemQty = itemQty;
		script.popButton = gameObject;
		
	}

    public void onOffTabibitoMove(bool offFlg) {
        GameObject TabibitoView = GameObject.Find("TabibitoView").gameObject;


        foreach(Transform obj in TabibitoView.transform) {
            if(obj.GetComponent<TabibitoMove>()) {
                if (offFlg) {
                    obj.GetComponent<Rigidbody2D>().simulated = false;
                }else {
                    obj.GetComponent<Rigidbody2D>().simulated = true;
                }
            }
        }

    }


}
