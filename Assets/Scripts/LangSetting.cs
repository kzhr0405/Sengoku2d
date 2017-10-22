using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class LangSetting : MonoBehaviour {

	public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        audioSources[2].Play();

        //Confirm Button
        string backPath = "Prefabs/Busyo/back";
        GameObject back = Instantiate(Resources.Load(backPath)) as GameObject;
        back.transform.SetParent(GameObject.Find("Panel").transform);
        back.transform.localScale = new Vector2(1, 1);
        RectTransform backTransform = back.GetComponent<RectTransform>();
        backTransform.anchoredPosition = new Vector3(0, 0, 0);

        GameObject parentObj = transform.parent.gameObject;
        int langId = 1;
        if (name == "JPN") {
            langId = 1;
        }else if (name == "ENG") {
            langId = 2;
        }else if (name == "CHN") {
            langId = 3;
        }
        
        //Message Box
        string msgPath = "Prefabs/Common/LangSettingConfirm";
        GameObject msg = Instantiate(Resources.Load(msgPath)) as GameObject;
        msg.transform.SetParent(GameObject.Find("Panel").transform);
        msg.transform.localScale = new Vector2(0.8f, 1.0f);
        RectTransform msgTransform = msg.GetComponent<RectTransform>();
        msgTransform.anchoredPosition = new Vector3(0, 0, 0);
        msgTransform.name = "LangSettingConfirm";

        msg.transform.FindChild("YesButton").GetComponent<LangSettingConfirm>().back = back;
        msg.transform.FindChild("YesButton").GetComponent<LangSettingConfirm>().langId = langId;
        msg.transform.FindChild("YesButton").GetComponent<LangSettingConfirm>().confirm = msg;
        msg.transform.FindChild("NoButton").GetComponent<LangSettingConfirm>().back = back;
        msg.transform.FindChild("NoButton").GetComponent<LangSettingConfirm>().confirm = msg;

        
    }

    public void ChangeButtonColorByConfig(int LangId, GameObject parentObj) {

        Color onBtnColor = new Color(85f / 255f, 85f / 255f, 85f / 255f, 160f / 255f);
        Color onTxtColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 40f / 255f);
        Color offBtnColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 200f / 255f);
        Color offTxtColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 175f / 255f);

        if (LangId == 1) {
            GameObject offObj = parentObj.transform.FindChild("JPN").gameObject;
            offObj.GetComponent<Image>().color = onBtnColor;
            offObj.transform.FindChild("Text").GetComponent<Text>().color = onTxtColor;
            offObj.GetComponent<Button>().enabled = false;

            /*
            GameObject onObj = parentObj.transform.FindChild("CHN").gameObject;
            onObj.GetComponent<Image>().color = offBtnColor;
            onObj.transform.FindChild("Text").GetComponent<Text>().color = offTxtColor;
            onObj.GetComponent<Button>().enabled = true;
            */

            GameObject onObj2 = parentObj.transform.FindChild("ENG").gameObject;
            onObj2.GetComponent<Image>().color = offBtnColor;
            onObj2.transform.FindChild("Text").GetComponent<Text>().color = offTxtColor;
            onObj2.GetComponent<Button>().enabled = true;

        }else if (LangId == 2) {
            GameObject offObj = parentObj.transform.FindChild("ENG").gameObject;
            offObj.GetComponent<Image>().color = onBtnColor;
            offObj.transform.FindChild("Text").GetComponent<Text>().color = onTxtColor;
            offObj.GetComponent<Button>().enabled = false;

            /*
            GameObject onObj = parentObj.transform.FindChild("CHN").gameObject;
            onObj.GetComponent<Image>().color = offBtnColor;
            onObj.transform.FindChild("Text").GetComponent<Text>().color = offTxtColor;
            onObj.GetComponent<Button>().enabled = true;
            */

            GameObject onObj2 = parentObj.transform.FindChild("JPN").gameObject;
            onObj2.GetComponent<Image>().color = offBtnColor;
            onObj2.transform.FindChild("Text").GetComponent<Text>().color = offTxtColor;
            onObj2.GetComponent<Button>().enabled = true;

        } else if (LangId == 3) {
            GameObject offObj = parentObj.transform.FindChild("CHN").gameObject;
            offObj.GetComponent<Image>().color = onBtnColor;
            offObj.transform.FindChild("Text").GetComponent<Text>().color = onTxtColor;
            offObj.GetComponent<Button>().enabled = false;

            GameObject onObj = parentObj.transform.FindChild("JPN").gameObject;
            onObj.GetComponent<Image>().color = offBtnColor;
            onObj.transform.FindChild("Text").GetComponent<Text>().color = offTxtColor;
            onObj.GetComponent<Button>().enabled = true;

            GameObject onObj2 = parentObj.transform.FindChild("ENG").gameObject;
            onObj2.GetComponent<Image>().color = offBtnColor;
            onObj2.transform.FindChild("Text").GetComponent<Text>().color = offTxtColor;
            onObj2.GetComponent<Button>().enabled = true;

        }
    }
}
