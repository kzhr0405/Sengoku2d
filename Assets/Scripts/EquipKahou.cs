using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class EquipKahou : MonoBehaviour {

	public string kahouType;
	public int kahouId;
	public string selectedButton;
    public string kahouName;
    public string kahouTypeName;
    public int kahouEffect;
    public string kahouUnit;

    public void OnClick () {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [3].Play ();

		/* New Equip Case*/
		//Reduce Available Kahou
		string temp = "available" + kahouType;
		string availableKahou = PlayerPrefs.GetString (temp);
		char[] delimiterChars = {','};
		string[] available_list = availableKahou.Split (delimiterChars);

		ArrayList newAvailableList = new ArrayList ();
		bool flag = false;

		for(int i=0;i<available_list.Length;i++){
			int tempKahouId = int.Parse(available_list[i]);

			if(kahouId==tempKahouId){
				if(flag==false){
					flag = true;
				}else{
					newAvailableList.Add(tempKahouId);
				}
			}else{
				newAvailableList.Add(tempKahouId);
			}
		}

		//Set String
		string kahouForData="";
		for(int j=0;j<newAvailableList.Count;j++){
			if(j != newAvailableList.Count-1){
				kahouForData = kahouForData + newAvailableList[j] + ",";
			}else{
				kahouForData = kahouForData + newAvailableList[j];
			}
		}

		//Attach Kaho to Busyo
		GameObject mainController = GameObject.Find ("GameScene");
		string busyoId = mainController.GetComponent<NowOnBusyo>().OnBusyo;
        string tempBusyo = "kahou" + busyoId;
		string busyoKahou = PlayerPrefs.GetString (tempBusyo);

		//Sample "0,1,0,0,0,0,0,0"
		int target = 999999;
		if(kahouType=="Bugu"){
			target = 0;
		}else if(kahouType=="Kabuto"){
			target = 1;
		}else if(kahouType=="Gusoku"){
			target = 2;
		}else if(kahouType=="Meiba"){
			target = 3;
		}else if(kahouType=="Cyadougu"){
			if(mainController.GetComponent<NowOnButton>().onKahouButton == "ItemCyadougu1"){
				target = 4;
			}else{
				target = 5;
			}
		}else if(kahouType=="Heihousyo"){
			target = 6;
		}else if(kahouType=="Chishikisyo"){
			target = 7;
		}
		string kahouBusyoId="";
		string[] busyoKahouList = busyoKahou.Split (delimiterChars);
		for(int k=0; k<busyoKahouList.Length;k++){
			string previousKahou = busyoKahouList[k];

			if(k == target){
				//Replace
				if(k != busyoKahouList.Length-1){
					kahouBusyoId = kahouBusyoId + kahouId + ",";
				}else{
					kahouBusyoId = kahouBusyoId + kahouId;
				}
			}else{
				//Add directol y
				if(k != busyoKahouList.Length-1){
					kahouBusyoId = kahouBusyoId + previousKahou + ",";
			
				}else{
					kahouBusyoId = kahouBusyoId + previousKahou;
				}
			}
		}

		//Reduce Available Kaho & Busyo Kahoy

		PlayerPrefs.SetString (temp,kahouForData);
		PlayerPrefs.SetString (tempBusyo,kahouBusyoId);
		PlayerPrefs.SetBool ("questSpecialFlg7",true);

		PlayerPrefs.Flush();

		//ReCall as Initialization
		KahouScene kaho = new KahouScene();
		kaho.createKahouStatusView(busyoId);

		//Close Board
		CloseBoard close = new CloseBoard ();
		close.onClick();

        //Adjust jinkei hp
        if(kahouType== "Kabuto") {
            Jinkei Jinkei = new Jinkei();
            int baseHP = GameObject.Find("GameScene").GetComponent<NowOnBusyo>().HP;
            int var = (baseHP * kahouEffect) / 100;
            Jinkei.jinkeiHpUpda(true, var);
        }
        //Message
        Message msg = new Message();
        string msgTxt = kahouName + "\n" + kahouTypeName + " +" + kahouEffect.ToString() + kahouUnit;
        msg.makeMessage(msgTxt);

       
    }	
}
