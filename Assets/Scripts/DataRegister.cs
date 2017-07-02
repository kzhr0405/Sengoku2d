using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataRegister : MonoBehaviour {

    public string userId;
    public bool initDataDoneFlg = false;

    private void Start() {

        //Init Data Maker
        userId = PlayerPrefs.GetString("userId", "");
        initDataDoneFlg = PlayerPrefs.GetBool("initDataFlg",false);
        if (!initDataDoneFlg) {
            if(userId == "" || userId == null) {
                InitDataMaker initData = transform.FindChild("InitDataMaker").GetComponent<InitDataMaker>();
                initData.makeInitData();
            }
        }

        //User Data Update
        DataUserId DataUserId = GetComponent<DataUserId>();
        DataJinkei DataJinkei = GetComponent<DataJinkei>();

        if (userId == "" ) {
            //New UserId
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
            //Update UserId
            if (Application.internetReachability != NetworkReachability.NotReachable) {
                DataUserId.UpdateUserId(userId);
                DataJinkei.UpdateJinkei(userId);

                //Reward
                DataReward DataReward = GetComponent<DataReward>();
                DataReward.GetRewardMaster(userId);
            }
        }

    }    
}
