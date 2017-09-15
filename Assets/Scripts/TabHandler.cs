using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour {

	void Start () {
		GameObject.Find ("Ronkou").GetComponent<RonkouScene> ().OnClick ();
	}

	public void tabButtonColor(string buttonName){
		Color pushedTabColor = new Color (118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
		Color pushedTextColor = new Color (219f / 255f, 219f / 255f, 212f / 255f, 200f / 255f);

		Color normalTabColor = new Color (255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
		Color normalTextColor = new Color (255f / 255f, 255f / 255f, 0f / 255f, 200f / 255f);

		//Clear Color
		GameObject ronkou = GameObject.Find ("Ronkou");
		GameObject senpou = GameObject.Find ("Senpou");
		GameObject kahou = GameObject.Find ("Kahou");
		GameObject syogu = GameObject.Find ("Syogu");

		ronkou.GetComponent<Image> ().color = normalTabColor;
		senpou.GetComponent<Image> ().color = normalTabColor;
		kahou.GetComponent<Image> ().color = normalTabColor;
		syogu.GetComponent<Image> ().color = normalTabColor;

		ronkou.transform.FindChild ("Text").GetComponent<Text> ().color = normalTextColor;
		senpou.transform.FindChild ("Text").GetComponent<Text> ().color = normalTextColor;
		kahou.transform.FindChild ("Text").GetComponent<Text> ().color = normalTextColor;
		syogu.transform.FindChild ("Text").GetComponent<Text> ().color = normalTextColor;


		//Change selected Tab Color
		if(buttonName == "Ronkou"){
			ronkou.GetComponent<Image> ().color = pushedTabColor;
			ronkou.transform.FindChild ("Text").GetComponent<Text> ().color = pushedTextColor;
		}else if(buttonName == "Senpou"){
			senpou.GetComponent<Image> ().color = pushedTabColor;
			senpou.transform.FindChild ("Text").GetComponent<Text> ().color = pushedTextColor;
		}else if(buttonName == "Kahou"){
			kahou.GetComponent<Image> ().color = pushedTabColor;
			kahou.transform.FindChild ("Text").GetComponent<Text> ().color = pushedTextColor;
		}else if(buttonName == "Syogu"){
			syogu.GetComponent<Image> ().color = pushedTabColor;
			syogu.transform.FindChild ("Text").GetComponent<Text> ().color = pushedTextColor;
		}
	}



}
