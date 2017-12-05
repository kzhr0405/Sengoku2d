using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
#if UNITY_IOS
using System;
using UnityEngine.iOS;
#endif

public class ReviewManager : MonoBehaviour
{
    private static string appleId = "1076867716";
    private static GameObject reviewAlert;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        reviewAlert = Resources.Load<GameObject>("Prefabs/Review/ReviewAlert");
    }

    // 10%の確率でレビュー依頼をする関数
    // レビュー以来ができる相手かどうかも判断する
    public static void Request10Parcent(Transform parentCanvas)
    {
        bool canRequest = CanReviewRequest();
        bool random = UnityEngine.Random.Range(0, 10) == 0;
        Debug.Log(canRequest + " : " + random);
        if (canRequest && random)
        {
            ShowReviewAlert(parentCanvas);
        }
    }

    public static void Request()
    {
#if UNITY_EDITOR
        string url = "http://samurai_wars.a-wiki.net/";
        Application.OpenURL(url);
#elif UNITY_IOS
        Version iosVersion = new Version(Device.systemVersion);
        Version minVersion = new Version("10.3");
        if (iosVersion >= minVersion)
        {
            iOSReviewRequest.Request();
        }
        else
        {
            string url = "itms-apps://itunes.apple.com/jp/app/id" + appleId + "?mt=8&action=write-review";
            Application.OpenURL(url);
        }
#elif UNITY_ANDROID
        string url = "market://details?id=" + Application.identifier;
        Application.OpenURL(url);
#endif
    }

    // レビュー依頼のアラートを表示する関数
    private static void ShowReviewAlert(Transform parentCanvas)
    {
        GameObject revAlert = Instantiate<GameObject>(reviewAlert);
        revAlert.transform.SetParent(parentCanvas, false);
    }

    // レビュー依頼できる相手かどうか（断っていないかつレビューしていない）
    private static bool CanReviewRequest(){
        bool flag = PlayerPrefs.GetBool("reviewRequestFlag", true);
        return flag;
    }

    // レビュー依頼をできなくする関数
    public static void SetReviewRequestFalse(){
        PlayerPrefs.SetBool("reviewRequestFlag", false);
    }

    // レビュー依頼が再度可能になる関数
    public static void SetReviewRequestTrue()
    {
        PlayerPrefs.SetBool("reviewRequestFlag", true);
    }
}