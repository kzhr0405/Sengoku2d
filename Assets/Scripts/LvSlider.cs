using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class LvSlider : MonoBehaviour {

	private Slider lvSlider;
	private int level;
	public GameObject toLv;
	public GameObject hp;
	public GameObject atk;
	public GameObject dfc;
	public GameObject requiredMoney;
	public List<int> moneyList = new List<int>();
	public List<int> statusList = new List<int>();
	public int pa_hp;

	// Use this for initialization
	void Start () {
		lvSlider = this.GetComponent <Slider> ();

		lvSlider.onValueChanged.AddListener((value) => {
			toLv.GetComponent<Text>().text=value.ToString();
			requiredMoney.GetComponent<Text>().text = moneyList[(int)value-1].ToString();
			hp.GetComponent<Text>().text = ((statusList[(int)value-1] - pa_hp/2)*10).ToString();
			atk.GetComponent<Text>().text = statusList[(int)value-1].ToString();
			dfc.GetComponent<Text>().text = statusList[(int)value-1].ToString();
		});
	}
}
