using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Gaikou : MonoBehaviour {

	Entity_gaikou_mst gaikouMst  = Resources.Load ("Data/gaikou_mst") as Entity_gaikou_mst;

	public int getGaikouValue (int srcDaimyo, int dstDaimyo) {
		int yuukouValue = 0;
		int startline = srcDaimyo - 1;
		object stslst = gaikouMst.param[startline];
		Type t = stslst.GetType();
		String param = "daimyo" + dstDaimyo;
		FieldInfo f = t.GetField(param);
		yuukouValue = (int)f.GetValue(stslst);

		return yuukouValue;
	}

	public void downGaikouByAttack(int srcDaimyo, int dstDaimyo){
		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");

		if (srcDaimyo == myDaimyo || dstDaimyo == myDaimyo) {
			//My Daimyo - Enemy Battle
			if(srcDaimyo == myDaimyo){
				//My Daimyo attacked Enemy
				string tempGaikou = "gaikou" + dstDaimyo.ToString();
				int myGaikouValue = PlayerPrefs.GetInt (tempGaikou);

				int newMyGaikouValue = 0;
				newMyGaikouValue = myGaikouValue - 25;
				if(newMyGaikouValue <= 0){
					newMyGaikouValue = 0;
				}
				PlayerPrefs.SetInt (tempGaikou,newMyGaikouValue);


			}else if(dstDaimyo == myDaimyo){
				//Enemy attacked My Daimyo
				string tempGaikou = "gaikou" + srcDaimyo.ToString();
				int myGaikouValue = PlayerPrefs.GetInt (tempGaikou);
				
				int newMyGaikouValue = 0;
				newMyGaikouValue = myGaikouValue - 25;
				if(newMyGaikouValue <= 0){
					newMyGaikouValue = 0;
				}
				PlayerPrefs.SetInt (tempGaikou,newMyGaikouValue);
			}
			
		}else{
			//Enemy - Enemy Battle
			string tempGaikou = "";
			if(srcDaimyo < dstDaimyo){
				tempGaikou = srcDaimyo.ToString() + "gaikou" + dstDaimyo.ToString();
			}else{
				tempGaikou = dstDaimyo.ToString() + "gaikou" + srcDaimyo.ToString();
			}
			int myGaikouValue = PlayerPrefs.GetInt (tempGaikou);
			int newMyGaikouValue = 0;
			newMyGaikouValue = myGaikouValue - 75;
			if(newMyGaikouValue <= 0){
				newMyGaikouValue = 0;
			}
			PlayerPrefs.SetInt (tempGaikou,newMyGaikouValue);
		}

		PlayerPrefs.Flush();
	}

	public int getExistGaikouValue(int srcDaimyo, int dstDaimyo){
		int myDaimyo = PlayerPrefs.GetInt ("myDaimyo");
		int yukoudo = 0;

		if (srcDaimyo == myDaimyo || dstDaimyo == myDaimyo) {
			//My Daimyo - Enemy Battle
			if(srcDaimyo == myDaimyo){
				//My Daimyo attacked Enemy
				string tempGaikou = "gaikou" + dstDaimyo.ToString();
				yukoudo = PlayerPrefs.GetInt (tempGaikou);
				
			}else if(dstDaimyo == myDaimyo){
				//Enemy attacked My Daimyo
				string tempGaikou = "gaikou" + srcDaimyo.ToString();
				yukoudo = PlayerPrefs.GetInt (tempGaikou);
			}
			
		}else{
			//Enemy - Enemy Battle
			string tempGaikou = "";
			if(srcDaimyo < dstDaimyo){
				tempGaikou = srcDaimyo.ToString() + "gaikou" + dstDaimyo.ToString();
			}else{
				tempGaikou = dstDaimyo.ToString() + "gaikou" + srcDaimyo.ToString();
			}
			if(PlayerPrefs.HasKey(tempGaikou)){
				yukoudo = PlayerPrefs.GetInt (tempGaikou);
			}else{
				yukoudo = getGaikouValue(srcDaimyo,dstDaimyo);
			}
		}
		
		return yukoudo;
	}

	public int getMyGaikou(int daimyoId){

		string tempGaikou = "gaikou" + daimyoId;
		int nowYukoudo = 0;
		if (PlayerPrefs.HasKey (tempGaikou)) {
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		} else {
			nowYukoudo = 50;
		}

		return nowYukoudo;
	}



	public int downMyGaikou(int daimyoId, int nowYukoudo, int maxValue){

		string tempGaikou = "gaikou" + daimyoId;

		int temp = maxValue + 1;
		int reduceYukoudo = UnityEngine.Random.Range(1,temp);

		int newYukoudo = nowYukoudo - reduceYukoudo;
		if(newYukoudo<0){
			newYukoudo = 0;
		}
		PlayerPrefs.SetInt (tempGaikou, newYukoudo);
		PlayerPrefs.Flush ();

		return newYukoudo;
	}


	public int getOtherGaikouValue(int srcDaimyo, int dstDaimyo){
		
		int yukoudo = 0;

		//Enemy - Enemy Battle
		string tempGaikou = "";
		if(srcDaimyo < dstDaimyo){
			tempGaikou = srcDaimyo.ToString() + "gaikou" + dstDaimyo.ToString();
		}else{
			tempGaikou = dstDaimyo.ToString() + "gaikou" + srcDaimyo.ToString();
		}
		if(PlayerPrefs.HasKey(tempGaikou)){
			yukoudo = PlayerPrefs.GetInt (tempGaikou);
		}else{
			yukoudo = getGaikouValue(srcDaimyo,dstDaimyo);
		}

		return yukoudo;
	}


	public void upOtherGaikouValue(int daimyoId1, int daimyoId2, int upValue){

		//Get Current yukoudo
		int nowYukoudo = 0;
		string tempGaikou = "";
		if(daimyoId1 < daimyoId2){
			tempGaikou = daimyoId1.ToString() + "gaikou" + daimyoId2.ToString();
		}else{
			tempGaikou = daimyoId2.ToString() + "gaikou" + daimyoId1.ToString();
		}
		if(PlayerPrefs.HasKey(tempGaikou)){
			nowYukoudo = PlayerPrefs.GetInt (tempGaikou);
		}else{
			nowYukoudo = getGaikouValue(daimyoId1,daimyoId2);
		}

		//Up yukoudo
		int newYukoudo = 0;
		newYukoudo = nowYukoudo + upValue;
		if (newYukoudo > 100) {
			newYukoudo = 100;
		}
		PlayerPrefs.SetInt (tempGaikou, newYukoudo);
		PlayerPrefs.Flush ();

	}



}
