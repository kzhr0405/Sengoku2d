using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class AutoSetting : MonoBehaviour {

	public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[2].Play();

        GameObject parentObj = transform.parent.gameObject;
        if (name == "2") {
            ChangeButtonColorByConfig(false, parentObj);
            PlayerPrefs.SetBool("Auto2Flg",true);
        }
        else if(name == "4"){
            ChangeButtonColorByConfig(true, parentObj);
            PlayerPrefs.SetBool("Auto2Flg", false);

        }
        PlayerPrefs.Flush();
    }

    public void ChangeButtonColorByConfig(bool OffFlg, GameObject parentObj) {

        Color onBtnColor = new Color(85f / 255f, 85f / 255f, 85f / 255f, 160f / 255f);
        Color onTxtColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 40f / 255f);
        Color offBtnColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);
        Color offTxtColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 175f / 255f);

        if (OffFlg) {
            GameObject offObj = parentObj.transform.FindChild("4").gameObject;
            offObj.GetComponent<Image>().color = onBtnColor;
            offObj.transform.FindChild("Text").GetComponent<Text>().color = onTxtColor;
            offObj.GetComponent<Button>().enabled = false;

            GameObject onObj = parentObj.transform.FindChild("2").gameObject;
            onObj.GetComponent<Image>().color = offBtnColor;
            onObj.transform.FindChild("Text").GetComponent<Text>().color = offTxtColor;
            onObj.GetComponent<Button>().enabled = true;

        }
        else {
            GameObject onObj = parentObj.transform.FindChild("2").gameObject;
            onObj.GetComponent<Image>().color = onBtnColor;
            onObj.transform.FindChild("Text").GetComponent<Text>().color = onTxtColor;
            onObj.GetComponent<Button>().enabled = false;

            GameObject offObj = parentObj.transform.FindChild("4").gameObject;
            offObj.GetComponent<Image>().color = offBtnColor;
            offObj.transform.FindChild("Text").GetComponent<Text>().color = offTxtColor;
            offObj.GetComponent<Button>().enabled = true;

        }
    }
}
