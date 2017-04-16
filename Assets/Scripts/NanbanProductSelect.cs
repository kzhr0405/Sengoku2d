using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class NanbanProductSelect : MonoBehaviour {


	public GameObject Content;
	public GameObject BuyBtn;

	public GameObject Exp;
	public GameObject Money;

	public float price = 0;

	public string itemTPCd = "";
	public int itemTPId = 0;
	public int itemTPQty = 0;
	public string itemTPExp = "";
	
	public string itemSakuCd = "";
	public int itemSakuId = 0;
	public string itemSakuExp = "";
	
	public string itemKahouCd = "";
	public int itemKahouId = 0;
	public string itemKahouExp = "";

	public void OnClick(){

		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		//Change Color
		Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
		Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);

		foreach (Transform obj in Content.transform) {
			obj.GetComponent<Image>().color = unSelect;
		}
		GetComponent<Image> ().color = Select;

		//money check
		int moeny = PlayerPrefs.GetInt ("money");
		bool moneyCheckFlg = false;
		if (moeny >= price) {
			moneyCheckFlg = true;
			BuyBtn.GetComponent<TabibitoItemGetter> ().moneyCheckFlg = moneyCheckFlg;
			BuyBtn.GetComponent<TabibitoItemGetter> ().isNanbansenFlg = true;
		} else {
			BuyBtn.GetComponent<TabibitoItemGetter> ().moneyCheckFlg = moneyCheckFlg;
		}

        //View
        if (itemTPCd != "") {
            Exp.transform.FindChild("NanbanExpValue").GetComponent<Text>().text = itemTPExp;
            Money.transform.FindChild("moneyValue").GetComponent<Text>().text = price.ToString();
            BuyBtn.GetComponent<TabibitoItemGetter>().itemCd = itemTPCd;
            BuyBtn.GetComponent<TabibitoItemGetter>().itemId = itemTPId;
            BuyBtn.GetComponent<TabibitoItemGetter>().itemQty = itemTPQty;
            BuyBtn.GetComponent<TabibitoItemGetter>().paiedMoney = (int)price;

        }
        else if (itemSakuCd != "") {
            Exp.transform.FindChild("NanbanExpValue").GetComponent<Text>().text = itemSakuExp;
            Money.transform.FindChild("moneyValue").GetComponent<Text>().text = price.ToString();
            BuyBtn.GetComponent<TabibitoItemGetter>().itemCd = itemSakuCd;
            BuyBtn.GetComponent<TabibitoItemGetter>().itemId = itemSakuId;
            BuyBtn.GetComponent<TabibitoItemGetter>().paiedMoney = (int)price;

        }
        else if (itemKahouCd != "") {
            Exp.transform.FindChild("NanbanExpValue").GetComponent<Text>().text = itemKahouExp;
            Money.transform.FindChild("moneyValue").GetComponent<Text>().text = price.ToString();
            BuyBtn.GetComponent<TabibitoItemGetter>().itemCd = itemKahouCd;
            BuyBtn.GetComponent<TabibitoItemGetter>().itemId = itemKahouId;
            BuyBtn.GetComponent<TabibitoItemGetter>().paiedMoney = (int)price;
        }


    }
}
