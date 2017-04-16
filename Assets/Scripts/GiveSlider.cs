using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class GiveSlider : MonoBehaviour {

	private Slider moneySlider;
	public GameObject valueObj;

	void Start () {
		moneySlider = this.GetComponent <Slider> ();
		
		moneySlider.onValueChanged.AddListener((value) => {
			float calc = value * 1000;
			valueObj.GetComponent<Text>().text = calc.ToString();
		});
	}

}
