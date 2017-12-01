using UnityEngine;
using System;
using System.Collections;
using System.Xml;
using HtmlAgilityPack;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class VersionChecker : MonoBehaviour {
    string _storeUrl = "";

    void Start() {
        CurrentVersionCheck();

        if (LoadInvalidVersionUpCheck()) {
            return;
        }
        #if UNITY_IOS
            StartCoroutine(VersionCheckIOS());
        #elif UNITY_ANDROID
            StartCoroutine(VersionCheckAndroid());
        #endif
    }

    const string INVALID_VERSION_UP_CHECK_KEY = "InvalidVersionUpCheck";

    bool LoadInvalidVersionUpCheck() {
        return PlayerPrefs.GetInt(INVALID_VERSION_UP_CHECK_KEY, 0) != 0;
    }

    void SaveInvalidVersionUpCheck(bool invalid) {
        PlayerPrefs.SetInt(INVALID_VERSION_UP_CHECK_KEY, invalid ? 1 : 0);
    }

    const string CURRENT_VERSION_CHECK_KEY = "CurrentVersionCheck";

    void CurrentVersionCheck() {
        var version = PlayerPrefs.GetString(CURRENT_VERSION_CHECK_KEY, "");
        if (version != Application.version) {
            PlayerPrefs.SetString(CURRENT_VERSION_CHECK_KEY, Application.version);
            SaveInvalidVersionUpCheck(false);
        }
    }

    IEnumerator VersionCheckIOS() {
        var url = string.Format("https://itunes.apple.com/lookup?bundleId={0}", Application.identifier);
        WWW www = new WWW(url);
        yield return www;

        if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text)) {
            var lookupData = JsonUtility.FromJson<AppLookupData>(www.text);
            if (lookupData.resultCount > 0 && lookupData.results.Length > 0) {
                var result = lookupData.results[0];
                if (VersionComparative(result.version)) {
                    ShowUpdatePopup(result.trackViewUrl);
                }
            }
        }
    }

    IEnumerator VersionCheckAndroid() {
        var url = string.Format("https://play.google.com/store/apps/details?id={0}", Application.identifier);

        WWW www = new WWW(url);
        yield return www;
        if (string.IsNullOrEmpty(www.error) && !string.IsNullOrEmpty(www.text)) {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(www.text);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@itemprop=\"softwareVersion\"]");
            if (node != null) {
                if (VersionComparative(node.InnerText)) {
                    ShowUpdatePopup(url);
                }
            }
        }
    }

    bool VersionComparative(string storeVersionText) {
        if (string.IsNullOrEmpty(storeVersionText)) {
            return false;
        }
        try {
            var storeVersion = new Version(storeVersionText);
            var currentVersion = new Version(Application.version);

            if (storeVersion.CompareTo(currentVersion) > 0) {
                return true;
            }
        }
        catch (Exception e) {
            Debug.LogErrorFormat("{0} VersionComparative Exception caught.", e);
        }

        return false;
    }

    void ShowUpdatePopup(string url) {
        if (string.IsNullOrEmpty(url)) {
            return;
        }
        _storeUrl = url;
#if !UNITY_EDITOR
        int langId = PlayerPrefs.GetInt("langId");
        if (langId == 2) {
            string title = "There is an update of the application";
            string message = "Do you want to update the application?";
            string yes = "Yes";
            string no = "No";
            MobileNativeDialog dialog = new MobileNativeDialog(title, message, yes, no);

            dialog.OnComplete += OnPopUpClose;   
        }else {    
            string title = "「合戦-戦国絵巻-」の最新バージョンがあります";
            string message = "更新しますか？";
            string yes = "はい";
            string no = "いいえ";
            MobileNativeDialog dialog = new MobileNativeDialog(title, message, yes, no);
            dialog.OnComplete += OnPopUpClose;
        }
#endif
    }


    private void OnPopUpClose(MNDialogResult result) {
        switch (result) {
            case MNDialogResult.YES:
                Application.OpenURL(_storeUrl);
                break;
            case MNDialogResult.NO:
                SaveInvalidVersionUpCheck(true);
                break;
        }
    }
}


[Serializable]
public class AppLookupData {
    public int resultCount;
    public AppLookupResult[] results;

}

[Serializable]
public class AppLookupResult {
    public string version;
    public string trackViewUrl;
}