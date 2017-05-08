using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DataRegister : MonoBehaviour {

	void Start () {

        //Initial Data Update
        DataUserId DataUserId = GetComponent<DataUserId>();
        DataJinkei DataJinkei = GetComponent<DataJinkei>();

        if (!PlayerPrefs.HasKey("userId")) {
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
                string userId = PlayerPrefs.GetString("userId");
                DataUserId.UpdateUserId(userId);
                DataJinkei.UpdateJinkei(userId);
            }
        }

    }
}
