using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class SellSlider : MonoBehaviour {

	private Slider lvSlider;
	public GameObject SellQtyValue;
	public GameObject GetMoneyValue;
	public int unitPrice;

	// Use this for initialization
	void Start () {
		lvSlider = this.GetComponent <Slider> ();
		
		lvSlider.onValueChanged.AddListener((value) => {
			SellQtyValue.GetComponent<Text>().text=value.ToString();
			float calc = value * unitPrice;
			GetMoneyValue.GetComponent<Text>().text = "+" + calc.ToString();
		});
	}
}
