using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using UnityEngine.UI;

public class DoSpaceBuy : MonoBehaviour {

    public bool busyoDamaOKflg = false;
    public int paiedBusyoDama;
    public int buySpace;
    public GameObject touchBackObj;

    public void OnClick() {
        AudioSource[] audioSources = GameObject.Find("SEController").GetComponents<AudioSource>();
        Message msg = new Message();
        if (busyoDamaOKflg) {
            //minus
            int nowBusyoDama = PlayerPrefs.GetInt("busyoDama");
            int newBusyoDama = nowBusyoDama - paiedBusyoDama;
            PlayerPrefs.SetInt("busyoDama", newBusyoDama);
            int nowSpace = PlayerPrefs.GetInt("space");
            int newSpace = nowSpace + buySpace;
            PlayerPrefs.SetInt("space", newSpace);
            PlayerPrefs.Flush();            

            GameObject.Find("BusyoDamaValue").GetComponent<Text>().text = newBusyoDama.ToString();

            //Close
            touchBackObj.GetComponent<CloseOneBoard>().OnClick();

            audioSources[3].Play();
            msg.makeMessage(msg.getMessage(160));

        }else {
            //Message            
            audioSources[4].Play();            
            msg.makeMessage(msg.getMessage(2));
        }
    }
}
