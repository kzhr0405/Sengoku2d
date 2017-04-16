using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class InitDataMaker : MonoBehaviour {

	public void makeInitData () {

		/*******************************/
		/***** MainStageController *****/
		/*******************************/

		//Basic status
		System.DateTime now = System.DateTime.Now;
		PlayerPrefs.SetString ("lasttime", now.ToString ());
		PlayerPrefs.SetInt ("kuniLv",1);
		PlayerPrefs.SetInt ("kuniExp",0);
		PlayerPrefs.SetInt ("money",10000);
		PlayerPrefs.SetInt ("busyoDama",0);
		PlayerPrefs.SetInt ("hyourouMax",100);
		PlayerPrefs.SetInt ("hyourou",100);
		PlayerPrefs.SetInt ("myBusyoQty",1);
		PlayerPrefs.SetInt ("syogunDaimyoId",14);

		//Busyo
		int myDaimyo = 1; //Oda Nobunaga
		PlayerPrefs.SetInt("myDaimyo",myDaimyo);
		PlayerPrefs.SetInt("myDaimyoBusyo",19);

		//Open Kuni
		PlayerPrefs.SetString("openKuni","1,2,3,4");
		PlayerPrefs.SetString("kuni1","1,2,3,4,5,6,7,8,9,10"); //Owari
		PlayerPrefs.SetString ("clearedKuni","1");

		//Seiryoku
		PlayerPrefs.SetString ("seiryoku","1,2,3,4,5,6,7,8,3,4,9,10,12,11,13,14,15,16,3,17,18,17,19,8,19,19,20,21,22,23,24,25,26,27,28,29,30,31,31,32,33,34,35,35,36,37,38,38,38,38,31,31,31,39,40,41,41,41,41,42,43,44,45,45,46");

		//Year & Season
		string newYearSeason = "1560,1";
		PlayerPrefs.SetString ("yearSeason", newYearSeason);

		/*******************************/
		/*****     JinkeiScene     *****/
		/*******************************/
		PlayerPrefs.SetInt ("jinkei",1);
		PlayerPrefs.SetString ("myBusyo","19");
		PlayerPrefs.SetInt ("jinkeiLimit",3);
		PlayerPrefs.SetInt ("map12", 19);
		PlayerPrefs.SetInt ("1map12", 19);
		PlayerPrefs.SetInt ("jinkeiBusyoQty", 1);
		PlayerPrefs.SetInt ("jinkeiAveLv", 1);
		PlayerPrefs.SetInt ("jinkeiAveChLv", 1);
		PlayerPrefs.SetInt ("jinkeiHeiryoku", 1100);
		PlayerPrefs.SetInt ("soudaisyo1", 19);


		/*******************************/
		/*****     Busyo Scene     *****/
		/*******************************/
		PlayerPrefs.SetInt ("stockLimit",10);
		PlayerPrefs.SetInt ("19",1);
		PlayerPrefs.SetString ("hei19","TP:1:1:1");
		PlayerPrefs.SetInt ("senpou19",1);
		PlayerPrefs.SetInt ("saku19",1);
		PlayerPrefs.SetString ("kahou19","0,0,0,0,0,0,0,0");
		PlayerPrefs.SetInt ("exp19",0);

		/*******************************/
		/*****     Zukan Scene     *****/
		/*******************************/
		//PlayerPrefs.SetString ("zukanBusyoHst","19");

		/*******************************/
		/*****    Cyouhei Scene    *****/
		/*******************************/
		PlayerPrefs.SetString ("cyouheiYR","0,0,0");
		PlayerPrefs.SetString ("cyouheiKB","0,0,0");
		PlayerPrefs.SetString ("cyouheiTP","0,0,0");
		PlayerPrefs.SetString ("cyouheiYM","0,0,0");
		PlayerPrefs.SetString ("kanjyo","0,0,0");


		/*******************************/
		/*****    Naisei Scene    *****/
		/*******************************/
		PlayerPrefs.SetString("naisei1","1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0");
		PlayerPrefs.SetString("naisei2","1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0");
		PlayerPrefs.SetString("naisei3","1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0");
		PlayerPrefs.SetString("naisei4","1,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0,0:0");
		PlayerPrefs.SetInt("transferTP",0);
		PlayerPrefs.SetInt("transferKB",0);
		PlayerPrefs.SetInt("transferSNB",0);


		/*******************************/
		/*****    	  Item         *****/
		/*******************************/
		PlayerPrefs.SetString("gokuiItem","0,0,0,0,0");
		PlayerPrefs.SetString("kengouItem","0,0,0,0,0,0,0,0,0,0");
		PlayerPrefs.SetString("nanbanItem","0,0,0");
		PlayerPrefs.SetString("cyoutei","0,0,0");
		PlayerPrefs.SetString("koueki","0,0,0");


		/*******************************/
		/*****     Gaikou Value    *****/
		/*******************************/
		Gaikou gaikou = new Gaikou ();
		for(int l=2; l<47; l++){
			int value = gaikou.getGaikouValue(myDaimyo,l);
			string temp = "gaikou" + l.ToString();
			PlayerPrefs.SetInt (temp, value);
		}

		/*******************************/
		/*****       Doumei        *****/
		/*******************************/
		PlayerPrefs.SetString ("doumei2","9");
		PlayerPrefs.SetString ("doumei9","2");
		PlayerPrefs.SetString ("doumei3","8,19");
		PlayerPrefs.SetString ("doumei8","3,19,26");
		PlayerPrefs.SetString ("doumei19","3,8,26");
		PlayerPrefs.SetString ("doumei26","8,19");
		PlayerPrefs.SetString ("doumei38","36,37,39");
		PlayerPrefs.SetString ("doumei36","38");
		PlayerPrefs.SetString ("doumei37","38");
		PlayerPrefs.SetString ("doumei39","38");
		PlayerPrefs.SetString ("doumei5","6");
		PlayerPrefs.SetString ("doumei6","5");

        /*******************************/
        /*****       Tutorial      *****/
        /*******************************/
        PlayerPrefs.SetInt("tutorialId", 1);

        /*******************************/
        /***** Init Common Process *****/
        /*******************************/
        //Change initDataFlg
        PlayerPrefs.SetBool ("initDataFlg",true);
		PlayerPrefs.Flush();
	}	
}
