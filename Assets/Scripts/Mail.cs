using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class Mail : MonoBehaviour {

    private const string NEW_LINE_STRING = "\n";
    private const string CAUTION_STATEMENT = "---------Keep below info.---------" + NEW_LINE_STRING;
    private const string CAUTION_STATEMENT2 = "---------Keep above info.---------" + NEW_LINE_STRING;

    public void OnClick(){
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [5].Play ();

		string email    =   "zeimoter@gmail.com";
        string subject = "";
        string body = "";

        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            subject = "【The Samurai Wars】";
		     body = "Body of the letter";
        }else if(langId==3) {
            subject = "【合战-战国绘卷-】咨询";
            body = "请输入咨询内容。";
        }
        else {
            subject = "【合戦-戦国絵巻-】お問い合わせ";
            body = "お問い合わせ内容をご記載下さい。";
        }

        //App version
        string versionNo = Application.version;
        body += NEW_LINE_STRING + CAUTION_STATEMENT + NEW_LINE_STRING;
        body += "Version  : " + versionNo + NEW_LINE_STRING;
        body += "OS       : " + SystemInfo.operatingSystem + NEW_LINE_STRING;
        body += "User Id  : " + PlayerPrefs.GetString("userId") + NEW_LINE_STRING;

        //Data
        body += CAUTION_STATEMENT2;

        //エスケープ処理
        body = System.Uri.EscapeDataString(body);
		subject =System.Uri.EscapeDataString(subject);
		Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
	}
}
