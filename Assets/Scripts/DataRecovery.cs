using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DataRecovery : MonoBehaviour {

    //common
    public AudioSource[] audioSources;
    public Text textScript;
    public RecoveryDataStore RecoveryDataStore;
    public string inputUserId;
    public bool Fetched1 = false;
    public bool Fetched2 = false;
    public bool Fetched3 = false;

    public void Start() {
        RecoveryDataStore = GameObject.Find("RecoveryDataStore").GetComponent<RecoveryDataStore>();
        audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();

    }

    public void OnClick() {
        
        Message msg = new Message();
        inputUserId = textScript.text;

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            audioSources[4].Play();
            msg.makeMessage(msg.getMessage(5));
        }else {
            if (inputUserId == "") {
                audioSources[4].Play();
                msg.makeMessage(msg.getMessage(145));                
            } else {
                //Check user Id not equal current
                string userId = PlayerPrefs.GetString("userId");
                //string userId = "";//test
                if (inputUserId == userId) {
                    audioSources[4].Play();
                    msg.makeMessage(msg.getMessage(146));                
                }else {
                    //Check 24 digit
                    if(inputUserId.Length != 24) {
                        audioSources[4].Play();
                        msg.makeMessage(msg.getMessage(147));
                    } else {
                        //Start
                        //Get All user stored data(dataStore or userId+jinkeiPvP)
                        audioSources[0].Play();
                        RecoveryDataStore.GetDataStore(inputUserId);
                    }
                }                
            }
        }
    }

    void Update() {
        if (RecoveryDataStore.userIdCount != -1 && RecoveryDataStore.dataStore_userId != -1 && !Fetched1) {
            //Check never recovered in server
            //RecoveryDataStore.GetDataRecoveryCount(inputUserId);

            Fetched1 = true;
        }

        if (RecoveryDataStore.userIdCount != -1 && RecoveryDataStore.dataStore_userId != -1 && Fetched1 && !Fetched2) {
            RecoveryDataStore.GetPvP(inputUserId);
            Fetched2 = true;
        }
        
        //Create confirm Board
        if (RecoveryDataStore.userIdCount != -1 && RecoveryDataStore.dataStore_userId != -1 && Fetched1 && Fetched2 && !Fetched3) {

            audioSources[0].Play();

            //Confirm Button
            string backPath = "Prefabs/Busyo/back";
            GameObject back = Instantiate(Resources.Load(backPath)) as GameObject;
            GameObject panel = GameObject.Find("Panel").gameObject;
            back.transform.SetParent(panel.transform);
            back.transform.localScale = new Vector2(1, 1);
            back.transform.localPosition = new Vector3(0, 0, 0);

            //Message Box
            string msgPath = "Prefabs/DataRecovery/DataRecoveryConfirm";
            GameObject msg = Instantiate(Resources.Load(msgPath)) as GameObject;
            msg.transform.SetParent(GameObject.Find("Panel").transform);
            msg.transform.localScale = new Vector2(1, 1);
            msg.transform.localPosition = new Vector3(0, 0, 0);
            
            msg.transform.FindChild("YesButton").GetComponent<DataRecoveryConfirm>().back = back;
            msg.transform.FindChild("YesButton").GetComponent<DataRecoveryConfirm>().msg = msg;
            msg.transform.FindChild("NoButton").GetComponent<DataRecoveryConfirm>().back = back;
            msg.transform.FindChild("NoButton").GetComponent<DataRecoveryConfirm>().msg = msg;

            Fetched3 = true;
        }
    }
}
