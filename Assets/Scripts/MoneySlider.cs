using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoneySlider : MonoBehaviour {

	private Slider mSlider;
	private Text moneyText;
	public GameObject doBtn;

	// Use this for initialization
	void Start () {
		mSlider = this.GetComponent <Slider> ();
		moneyText = gameObject.transform.Find ("MoneyValue").GetComponent<Text> ();
		float paiedMoney = 0;

		mSlider.onValueChanged.AddListener((value) => {
			paiedMoney = value * 1000;
			moneyText.text = paiedMoney.ToString();
			doBtn.GetComponent<DoGaikou>().paiedMoney = (int)paiedMoney;

		});
	}
}
