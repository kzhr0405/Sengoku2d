using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class EnemySenryokuCalc : MonoBehaviour {


	public int EnemyBusyoQtyCalc (int myKuniQty ,int enemyKuniQty, int minusBusyoQty) {

        bool hardFlg = PlayerPrefs.GetBool("hardFlg");
        int busyoQty = 0;

		//Get Player Syutujin Busyo Qty
		int jinkeiBusyoQty = PlayerPrefs.GetInt ("jinkeiBusyoQty");
        if (myKuniQty >= enemyKuniQty) {
			busyoQty = jinkeiBusyoQty;
		} else {
			busyoQty = jinkeiBusyoQty + (enemyKuniQty - myKuniQty)/2;
		}
        //Adjust
        if ((float)jinkeiBusyoQty * 1.5f < busyoQty){
            busyoQty = Mathf.CeilToInt(jinkeiBusyoQty * 1.5f);
        }
        //Adjust 
        List<float> randomPercent = new List<float> { 0.6f, 0.8f, 1.0f, 1.2f, 1.3f};
        int rmd = UnityEngine.Random.Range(0, randomPercent.Count);
        float rdmPwr = randomPercent[rmd];
        busyoQty = Mathf.CeilToInt(busyoQty * rdmPwr);

        //Adjust
        if(!hardFlg) {
            busyoQty = busyoQty - minusBusyoQty;
        }else {
            busyoQty = busyoQty + 1;
        }

        if (busyoQty > 12) {
			busyoQty = 12;
		}else if(busyoQty < 1){
            busyoQty = 3;
        }

		return busyoQty;
	}


	public int EnemyBusyoLvCalc (int senryokuRatio) {

        bool hardFlg = PlayerPrefs.GetBool("hardFlg");
        int busyoLv = PlayerPrefs.GetInt ("jinkeiAveLv");
        float temp1 = busyoLv * senryokuRatio;
		float temp2 = temp1 / 100;
		busyoLv = (int)temp2;
        
        //Adjust
        List<float> randomPercent;
        if (!hardFlg) {
            randomPercent = new List<float> { 0.8f, 0.9f, 1.0f, 1.2f, 1.3f, 1.5f, 1.8f};
        }else {
            randomPercent = new List<float> { 1.0f, 1.1f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f };
        }
        int rmd = UnityEngine.Random.Range(0, randomPercent.Count);
        float rdmPwr = randomPercent[rmd];
        busyoLv = Mathf.CeilToInt(busyoLv * rdmPwr);

        if (busyoLv == 0) {
			busyoLv = 1;
		} else if (busyoLv > 100) {
			busyoLv = 100;
		}
        return busyoLv;
	}


	public int EnemyButaiQtyCalc (int enemyKuniQty, int myKuniQty) {

        bool hardFlg = PlayerPrefs.GetBool("hardFlg");
        int myButaiQty = PlayerPrefs.GetInt("jinkeiAveChQty");
       
        int butaiQty = 0;

        if(myKuniQty >= enemyKuniQty) {
            int kuniGap = myKuniQty - enemyKuniQty;
            butaiQty = myButaiQty - kuniGap;
        }else {
            int kuniGap = enemyKuniQty - myKuniQty;
            butaiQty = myButaiQty + kuniGap;
        }
        
        //Adjust 
        if(butaiQty>5) {
            if (myButaiQty*2 < butaiQty){
                butaiQty = myButaiQty * 2;
            }
        }

        //Adjust 
        List<float> randomPercent;
        if (!hardFlg) {
            randomPercent = new List<float> { 0.8f, 1.0f, 1.2f, 1.3f, 1.5f };
        }else {
            randomPercent = new List<float> { 1.0f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f };
        }
        int rmd = UnityEngine.Random.Range(0, randomPercent.Count);
        float rdmPwr = randomPercent[rmd];
        butaiQty = Mathf.CeilToInt(butaiQty * rdmPwr);
        
        //Final Adjsut
        if (butaiQty <= 5){
            butaiQty = 5;
        }else if (butaiQty >= 20){
            butaiQty = 20;
        }

        return butaiQty;

    }

	public int EnemyButaiLvCalc (int senryokuRatio) {

        bool hardFlg = PlayerPrefs.GetBool("hardFlg");
        int butaiLv  = PlayerPrefs.GetInt ("jinkeiAveChLv");
        float temp1 = butaiLv * senryokuRatio;
		float temp2 = temp1 / 100;
		butaiLv = (int)temp2;
        
        //Adjust
        List<float> randomPercent;
        if (!hardFlg) {
            randomPercent = new List<float> { 0.8f, 1.0f, 1.2f, 1.3f, 1.5f, 1.8f };
        }else {
            randomPercent = new List<float> { 1.0f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f };
        }
        int rmd = UnityEngine.Random.Range(0, randomPercent.Count);
        float rdmPwr = randomPercent[rmd];
        butaiLv = Mathf.CeilToInt(butaiLv * rdmPwr);
        
        if (butaiLv == 0) {
			butaiLv = 1;
		} else if (butaiLv > 100) {
			butaiLv = 100;
		}
		return butaiLv;
	}

}
