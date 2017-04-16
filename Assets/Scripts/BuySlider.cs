using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class BuySlider : MonoBehaviour {
	
	private Slider slider;
	public GameObject buyQtyValue;
	public GameObject PayMoneyValue;
	public int unitPrice;
	
	// Use this for initialization
	void Start () {
		slider = this.GetComponent <Slider> ();
		
		slider.onValueChanged.AddListener((value) => {
			buyQtyValue.GetComponent<Text>().text= value.ToString();
			float calc = value * unitPrice;
			PayMoneyValue.GetComponent<Text>().text = calc.ToString();
		});
	}
}
