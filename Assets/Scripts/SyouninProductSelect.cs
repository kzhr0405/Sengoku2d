using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SyouninProductSelect : MonoBehaviour {

	//Common
	public string menuName = "";
	public GameObject Content;
	public GameObject Money;
	public GameObject Btn;
	public int price = 0;

	//Kahou
	public string kahouName = "";
	public string kahouEffectLabel = "";
	public string kahouEffectValue = "";
	public string kahouCd = "";
	public int kahouId = 0;

	//Busshi
	public string busshiCd = "";
	public int busshiQty = 0;

	public void OnClick(){

		//Change Color
		Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
		Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
		
		foreach (Transform obj in Content.transform) {
			obj.GetComponent<Image>().color = unSelect;
		}
		GetComponent<Image> ().color = Select;

		if (menuName == "Kahou") {
			GameObject Info = GameObject.Find ("Info").gameObject;
			Info.transform.Find ("Name").GetComponent<Text> ().text = kahouName;
			Info.transform.Find ("EffectLabel").GetComponent<Text> ().text = kahouEffectLabel;
			Info.transform.Find ("EffectValue").GetComponent<Text> ().text = "+" + kahouEffectValue + "%";

			Money.GetComponent<Text> ().text = price.ToString ();

			//Set value in button
			Btn.GetComponent<DoSyouninMenu> ().Money = Money;
			Btn.GetComponent<DoSyouninMenu> ().kahouCd = kahouCd;
			Btn.GetComponent<DoSyouninMenu> ().kahouId = kahouId;
		
		} else if (menuName == "Busshi") {

			int totalPrice = price * busshiQty;
			Money.GetComponent<Text> ().text = totalPrice.ToString ();

			Btn.GetComponent<DoSyouninMenu> ().Money = Money;
			Btn.GetComponent<DoSyouninMenu> ().busshiCd = busshiCd;
			Btn.GetComponent<DoSyouninMenu> ().busshiQty = busshiQty;
		}
	}
}
