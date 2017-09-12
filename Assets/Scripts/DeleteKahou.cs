using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DeleteKahou : MonoBehaviour {

	public string kahouType;
	public int kahouId;
	public string selectedButton;
    public string kahouName;
    public string kahouTypeName;
    public int kahouEffect;
    public string kahouUnit;

    // Use this for initialization
    public void OnClick() {
		AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [4].Play ();

		string kahouTypeTemp = "";

		/* unEquip Case*/
		//Add Available Kahou
		if(kahouType=="bugu"){
			kahouTypeTemp ="Bugu";
		}else if(kahouType=="kabuto"){
			kahouTypeTemp ="Kabuto";
		}else if(kahouType=="gusoku"){
			kahouTypeTemp ="Gusoku";
		}else if(kahouType=="meiba"){
			kahouTypeTemp ="Meiba";
		}else if(kahouType=="cyadougu"){
			kahouTypeTemp ="Cyadougu";
		}else if(kahouType=="heihousyo"){
			kahouTypeTemp ="Heihousyo";
		}else if(kahouType=="chishikisyo"){
			kahouTypeTemp ="Chishikisyo";
		}

		string temp = "available" + kahouTypeTemp;
		string availableKahou = PlayerPrefs.GetString (temp);

		if (availableKahou == null || availableKahou == "") {
			availableKahou = kahouId.ToString();
		} else {
			availableKahou = availableKahou + "," + kahouId.ToString();
		}

		/*Reduce Busyo Kaho Start*/
		GameObject mainController = GameObject.Find ("GameScene");
		string busyoId = mainController.GetComponent<NowOnBusyo>().OnBusyo;
		string tempBusyo = "kahou" + busyoId;
		string busyoKahou = PlayerPrefs.GetString (tempBusyo);

		int target = 999999;
		if(kahouTypeTemp=="Bugu"){
			target = 0;
		}else if(kahouTypeTemp=="Kabuto"){
			target = 1;
		}else if(kahouTypeTemp=="Gusoku"){
			target = 2;
		}else if(kahouTypeTemp=="Meiba"){
			target = 3;
		}else if(kahouTypeTemp=="Cyadougu"){
			if(mainController.GetComponent<NowOnButton>().onKahouButton == "ItemCyadougu1"){
				target = 4;
			}else{
				target = 5;
			}
		}else if(kahouTypeTemp=="Heihousyo"){
			target = 6;
		}else if(kahouTypeTemp=="Chishikisyo"){
			target = 7;
		}
		string kahouBusyoId="";
		char[] delimiterChars = {','};
		string[] busyoKahouList = busyoKahou.Split (delimiterChars);
		for(int k=0; k<busyoKahouList.Length;k++){
			string previousKahou = busyoKahouList[k];

			if(k == target){
				//Replace
				if(k != busyoKahouList.Length-1){
					kahouBusyoId = kahouBusyoId + "0,";
				}else{
					kahouBusyoId = kahouBusyoId + "0";
				}
			}else{
				//Add directoly
				if(k != busyoKahouList.Length-1){
					kahouBusyoId = kahouBusyoId + previousKahou + ",";
					
				}else{
					kahouBusyoId = kahouBusyoId + previousKahou;
				}
			}
		}
		/*Reduce Busyo Kaho End*/


		//Data Registration of Available Kaho & Busyo Kahou
		PlayerPrefs.SetString (temp,availableKahou);
		PlayerPrefs.SetString (tempBusyo,kahouBusyoId);
		PlayerPrefs.Flush();

		//ReCall as Initialization
		KahouScene kaho = new KahouScene();
		kaho.createKahouStatusView(busyoId);
		
		//Close Board
		CloseBoard close = new CloseBoard ();
		close.onClick();

        //Adjust jinkei hp
        if (kahouType == "kabuto") {
            Jinkei Jinkei = new Jinkei();
            int baseHP = GameObject.Find("GameScene").GetComponent<NowOnBusyo>().HP;
            int var = (baseHP * kahouEffect) / 100;
            Jinkei.jinkeiHpUpda(false, var);
        }

        //Message
        Message msg = new Message();
        string msgTxt = kahouName + "\n" + kahouTypeName + " -" + kahouEffect.ToString() + kahouUnit;
        msg.makeMessage(msgTxt);

    }	
}
