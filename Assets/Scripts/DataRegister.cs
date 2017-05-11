using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataRegister : MonoBehaviour {

    public string userId;
    public bool initDataDoneFlg = false;

    private void Awake() {
        userId = PlayerPrefs.GetString("userId","");
        initDataDoneFlg = PlayerPrefs.GetBool("initDataFlg",false);
        InitDataMaker initData = new InitDataMaker();
        if (!initDataDoneFlg && userId == "") {
            initData.makeInitData();
        }       
    }

    void Start () {

        //Initial Data Update
        DataUserId DataUserId = GetComponent<DataUserId>();
        DataJinkei DataJinkei = GetComponent<DataJinkei>();
        
        if (PlayerPrefs.HasKey("userId")) {
            string randomA = StringUtils.GeneratePassword(10);
            System.DateTime now = System.DateTime.Now;
            string randomB = now.ToString("yyyyMMddHHmmss");
            string userId = randomA + randomB;
            PlayerPrefs.SetString("userId", userId);
            PlayerPrefs.Flush();
            if (Application.internetReachability != NetworkReachability.NotReachable) {
                DataUserId.InsertUserId(userId);
            }
        }else {
            if (Application.internetReachability != NetworkReachability.NotReachable) {
                DataUserId.UpdateUserId(userId);
                DataJinkei.UpdateJinkei(userId);
            }
        }

    }
}
