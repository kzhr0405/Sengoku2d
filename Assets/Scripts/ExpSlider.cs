using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ExpSlider: MonoBehaviour {
	
	Slider slider;
	public List<float> maxExpArray;
	public float nowExp;
	public float newExp;
	public float targetMaxExp;
	public int i;
	public int startLv; //
	public float kanjyoExp;
	float countExp;
	Text maxExpText;
	Text nowExpText;

	void Start () {
		// スライダーを取得する
		slider = this.GetComponent<Slider>();
		maxExpText = GameObject.Find ("NextLvExpValue").GetComponent<Text> ();
		nowExpText = GameObject.Find ("CurrentExpValue").GetComponent<Text> ();
	}
	void Update () {
		//Increase
		float targetExp = maxExpArray[i];
		nowExp += targetExp/20;

		if (i != maxExpArray.Count-1) {
			if (nowExp > targetExp) {

				//Lvl up
				startLv = startLv + 1; 
				GameObject.Find ("PopLvValue").GetComponent<Text> ().text = startLv.ToString ();

				//back to 0
				nowExp = 0;

				//Count
				i = i + 1;
				slider.maxValue = maxExpArray [i];
				maxExpText.text = slider.maxValue.ToString();
				countExp = countExp + slider.maxValue;
			}
			//set
			slider.value = nowExp;
			nowExpText.text = slider.value.ToString();

		}else{

			//Stop
			if(countExp>=kanjyoExp){

				slider.value = maxExpArray [i] - (targetMaxExp - newExp);
				nowExpText.text = (maxExpArray [i] -(targetMaxExp - newExp)).ToString();
				GameObject.Find ("DoKakyuKanjyo").GetComponent<Button>().enabled = true;
				GameObject.Find ("DoCyukyuKanjyo").GetComponent<Button>().enabled = true;
				GameObject.Find ("DoJyokyuKanjyo").GetComponent<Button>().enabled = true;
				GameObject.Find ("close").GetComponent<Button>().enabled = true;

				this.GetComponent<ExpSlider>().enabled = false;
			}else{
				//set
				slider.value = nowExp;
				nowExpText.text = slider.value.ToString();
				countExp = countExp + slider.value;
			}
		}
	}
}