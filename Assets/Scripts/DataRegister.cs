using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using NCMB;

public class DataRegister : MonoBehaviour {

    public string userId = "";
    public bool initDataDoneFlg = false;

    private void Start() {

        //Init Data Maker
        userId = PlayerPrefs.GetString("userId");
        //userId = "tst5f8j0n920170409081251"; PlayerPrefs.SetString("userId", userId);
        initDataDoneFlg = PlayerPrefs.GetBool("initDataFlg");
        if (!initDataDoneFlg) {
            if(userId == "" || userId == null) {
                InitDataMaker initData = transform.FindChild("InitDataMaker").GetComponent<InitDataMaker>();
                initData.makeInitData();
            }
        }

        int langId = PlayerPrefs.GetInt("langId");
        if(langId == 0) {
            /*
            if (Application.systemLanguage == SystemLanguage.Japanese) {
                PlayerPrefs.SetInt("langId",1);
            } else if(Application.systemLanguage == SystemLanguage.English) {
                PlayerPrefs.SetInt("langId", 2);
            }else if(Application.systemLanguage == SystemLanguage.Chinese) {
                PlayerPrefs.SetInt("langId", 3);
            }
            */
            if (Application.systemLanguage == SystemLanguage.Japanese) {
                PlayerPrefs.SetInt("langId", 1);
            }else {
                PlayerPrefs.SetInt("langId", 2);
            }
            PlayerPrefs.Flush();
        }

        //User Data Update
        DataUserId DataUserId = GetComponent<DataUserId>();        
        DataJinkei DataJinkei = GetComponent<DataJinkei>();
        DataPvP DataPvP = GetComponent<DataPvP>();
        bool tutorialDoneFlg = PlayerPrefs.GetBool("tutorialDoneFlg");
        if (tutorialDoneFlg) {
            if (userId == "" || userId == null) {
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
                    DataPvP.UpdatePvP(userId);

                    //Reward
                    DataReward DataReward = GetComponent<DataReward>();
                    DataReward.GetRewardMaster(userId);

                    //Delete Gunzei Data : Rescue
                    DataDelete DataDelete = GetComponent<DataDelete>();
                    DataDelete.GunzeiDelete(userId);
                }
            }
        }
    }    
}
