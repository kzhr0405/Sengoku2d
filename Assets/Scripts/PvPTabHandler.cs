using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PvPTabHandler : MonoBehaviour {

    public bool clicked = false;

    public void Start() {
        if(name == "Kassen") {
            OnClick();
        }
    }


	public void OnClick() {

        if(!clicked) {
            //Teb Changer
            Color pushedTabColor = new Color(118f / 255f, 103f / 255f, 16f / 255f, 255f / 255f);
            Color pushedTextColor = new Color(219f / 255f, 219f / 255f, 212f / 255f, 255f / 255f);
            Color normalTabColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            Color normalTextColor = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);

            GameObject UpperView = GameObject.Find("UpperView").gameObject;
            foreach (Transform obj in UpperView.transform) {
                obj.GetComponent<Image>().color = normalTabColor;
                obj.transform.FindChild("Text").GetComponent<Text>().color = normalTextColor;
                obj.GetComponent<PvPTabHandler>().clicked = false;
            }
            GetComponent<Image>().color = pushedTabColor;
            transform.FindChild("Text").GetComponent<Text>().color = pushedTextColor;
            clicked = true;
        }
    }
}
