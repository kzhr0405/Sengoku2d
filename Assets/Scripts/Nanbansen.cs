using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Nanbansen : MonoBehaviour {

	public bool boughtFlg;

	public string itemTPCd = "";
	public int itemTPId = 0;
	public int itemTPQty = 0;
	public float itemTPPrice = 0;
	public string itemTPExp = "";

	public string itemSakuCd = "";
	public int itemSakuId = 0;
	public float itemSakuPrice = 0;
	public string itemSakuExp = "";
	
	public string itemKahouCd = "";
	public int itemKahouId = 0;
	public float itemKahouPrice = 0;
	public string itemKahouExp = "";
	
	public void OnClick(){
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

        //Adjustment
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            board.transform.FindChild ("GrpValue").GetComponent<Text> ().text = "Western Ship";
		    board.transform.FindChild ("Name").transform.FindChild ("NameValue").GetComponent<Text> ().text = "You can buy only 1 item.";
        }else {
            board.transform.FindChild("GrpValue").GetComponent<Text>().text = "南蛮船";
            board.transform.FindChild("Name").transform.FindChild("NameValue").GetComponent<Text>().text = "一品ダケ購入デキマス";
        }
		board.transform.FindChild ("Image").GetComponent<Image> ().sprite = gameObject.GetComponent<Image> ().sprite;
		board.transform.FindChild ("Rank").transform.FindChild ("RankValue").GetComponent<Text> ().enabled = false;
		board.transform.FindChild ("Serihu").transform.FindChild ("SerihuValue").GetComponent<Text> ().enabled = false;
		board.transform.FindChild ("Rank").GetComponent<Image> ().enabled = false;
		board.transform.FindChild ("Serihu").GetComponent<Image> ().enabled = false;
		board.transform.FindChild ("Qty").GetComponent<Text> ().enabled = false;

		//Products
		string scrollPath = "Prefabs/Naisei/Nanbansen/ScrollView";
		GameObject scroll = Instantiate (Resources.Load (scrollPath)) as GameObject;
		scroll.transform.SetParent (board.transform);
		scroll.transform.localScale = new Vector2 (1,1);
		scroll.transform.localPosition = new Vector3 (0,50,0);

		GameObject contact = scroll.transform.FindChild ("Content").gameObject;
		GameObject BuyBtn = board.transform.FindChild ("GetButton").gameObject;
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            BuyBtn.transform.FindChild ("Text").GetComponent<Text> ().text = "Buy";
        }else {
            BuyBtn.transform.FindChild("Text").GetComponent<Text>().text = "購入";
        }
		BuyBtn.GetComponent<TabibitoItemGetter> ().shipObj = gameObject;

		string expPath = "Prefabs/Naisei/Nanbansen/NanbanExp";
		GameObject exp = Instantiate (Resources.Load (expPath)) as GameObject;
		exp.transform.SetParent (board.transform);
		exp.transform.localScale = new Vector2 (1,1);
		exp.transform.localPosition = new Vector3 (0,-60,0);

		string moneyPath = "Prefabs/Naisei/Nanbansen/NanbanMoney";
		GameObject money = Instantiate (Resources.Load (moneyPath)) as GameObject;
		money.transform.SetParent (board.transform);
		money.transform.localScale = new Vector2 (1,1);
		money.transform.localPosition = new Vector3 (-80,-130,0);

		//TP
		GameObject TPSlot = contact.transform.FindChild ("TP").gameObject;
		string tpPath = "Prefabs/Item/CyouheiTP";
		GameObject TPObj = Instantiate (Resources.Load (tpPath)) as GameObject;
		TPObj.transform.SetParent (TPSlot.transform);
		TPObj.transform.localScale = new Vector2 (1,1);
		TPObj.transform.FindChild ("Qty").GetComponent<Text> ().text = itemTPQty.ToString();
        TPObj.GetComponent<Button>().enabled = false;
        RectTransform qty = TPObj.transform.FindChild ("Qty").GetComponent<RectTransform> ();
		qty.anchoredPosition3D = new Vector3 (-40,-30,0);

		Color lowColor = new Color (0f / 255f, 0f / 255f, 219f / 255f, 255f / 255f);
		Color midColor = new Color (94f / 255f, 0f / 255f, 0f / 255f, 255f / 255f);
		Color highColor = new Color (84f / 255f, 103f / 255f, 0f / 255f, 255f / 255f);
        if (itemTPId == 1) {
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                TPObj.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "Low";
            }else {
                TPObj.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "下";
            }
            TPObj.GetComponent<Image>().color = lowColor;
        } else if (itemTPId == 2) {
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                TPObj.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "Mid";
            }else {
                TPObj.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "中";
            }
			TPObj.GetComponent<Image>().color = midColor;
		}else if(itemTPId == 3){
            if (Application.systemLanguage != SystemLanguage.Japanese) {
                TPObj.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "High";
            }else {
                TPObj.transform.FindChild("CyouheiRank").GetComponent<Text>().text = "上";
            }
			TPObj.GetComponent<Image>().color = highColor;
		}


		TPSlot.GetComponent<NanbanProductSelect> ().Exp = exp;
		TPSlot.GetComponent<NanbanProductSelect> ().Money = money;
		TPSlot.GetComponent<NanbanProductSelect> ().BuyBtn = BuyBtn;
		TPSlot.GetComponent<NanbanProductSelect> ().Content = contact;
		TPSlot.GetComponent<NanbanProductSelect> ().itemTPCd = itemTPCd;
		TPSlot.GetComponent<NanbanProductSelect> ().itemTPId = itemTPId;
		TPSlot.GetComponent<NanbanProductSelect> ().itemTPQty = itemTPQty;
		TPSlot.GetComponent<NanbanProductSelect> ().price = itemTPPrice;
		TPSlot.GetComponent<NanbanProductSelect> ().itemTPExp = itemTPExp;

		//Saku
		GameObject SakuSlot = contact.transform.FindChild ("Saku").gameObject;
		string sakuPath = "Prefabs/Item/" + itemSakuCd;
		GameObject SakuObj = Instantiate (Resources.Load (sakuPath)) as GameObject;
		SakuObj.transform.SetParent (SakuSlot.transform);
		SakuObj.transform.localScale = new Vector2 (1,1);
		RectTransform rectImage = SakuObj.transform.FindChild("Image").GetComponent<RectTransform> ();
		rectImage.sizeDelta = new Vector2 (105,105);
		SakuSlot.GetComponent<NanbanProductSelect> ().Exp = exp;
		SakuSlot.GetComponent<NanbanProductSelect> ().Money = money;
		SakuSlot.GetComponent<NanbanProductSelect> ().BuyBtn = BuyBtn;
		SakuSlot.GetComponent<NanbanProductSelect> ().Content = contact;
		SakuSlot.GetComponent<NanbanProductSelect> ().itemSakuCd = itemSakuCd;
		SakuSlot.GetComponent<NanbanProductSelect> ().itemSakuId = itemSakuId;
		SakuSlot.GetComponent<NanbanProductSelect> ().price = itemSakuPrice;
		SakuSlot.GetComponent<NanbanProductSelect> ().itemSakuExp = itemSakuExp;

		//Kahou
		GameObject KahouSlot = contact.transform.FindChild ("Kahou").gameObject;
		string kahouPath = "Prefabs/Item/Kahou/" + itemKahouCd + itemKahouId;
		GameObject KahouObj = Instantiate (Resources.Load (kahouPath)) as GameObject;
		KahouObj.transform.SetParent (KahouSlot.transform);
		KahouObj.transform.localScale = new Vector2 (1,1);
		KahouObj.GetComponent<Button> ().enabled = false;
		RectTransform rectRank = KahouObj.transform.FindChild("Rank").GetComponent<RectTransform> ();
		KahouObj.transform.FindChild ("Rank").transform.localScale = new Vector2 (0.3f,0.3f);
		rectRank.anchoredPosition3D = new Vector3 (30,-30,0);
		KahouObj.transform.localScale = new Vector2 (1,1);
		KahouSlot.GetComponent<NanbanProductSelect> ().Exp = exp;
		KahouSlot.GetComponent<NanbanProductSelect> ().Money = money;
		KahouSlot.GetComponent<NanbanProductSelect> ().BuyBtn = BuyBtn;
		KahouSlot.GetComponent<NanbanProductSelect> ().Content = contact;
		KahouSlot.GetComponent<NanbanProductSelect> ().itemKahouCd = itemKahouCd;
		KahouSlot.GetComponent<NanbanProductSelect> ().itemKahouId = itemKahouId;
		KahouSlot.GetComponent<NanbanProductSelect> ().price = itemKahouPrice;
		KahouSlot.GetComponent<NanbanProductSelect> ().itemKahouExp = itemKahouExp;


        //Default Click
		TPSlot.GetComponent<NanbanProductSelect> ().OnClick ();
	}



}
