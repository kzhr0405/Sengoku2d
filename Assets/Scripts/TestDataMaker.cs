using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.Collections.Generic;

public class TestDataMaker : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //PlayerPrefs.SetInt("8", 199);
        //PlayerPrefs.SetInt("addlv8", 100);

        PlayerPrefs.SetInt("money", 0);
        PlayerPrefs.SetInt("busyoDama", 99999);
        PlayerPrefs.SetString("cyoutei", "100,100,100");
        PlayerPrefs.SetString("koueki", "100,100,100");
        PlayerPrefs.SetString("nanbanItem", "100,100,100");
        /*
        PlayerPrefs.SetInt("movieCount",15000);
        //PlayerPrefs.SetString ("seiryoku","1,1,3,1,1,1,17,1,3,1,1,1,1,1,17,1,1,17,8,17,17,28,8,8,8,8,19,19,19,28,28,28,28,28,28,28,28,1,1,1,1,1,38,38,31,38,38,38,31,38,31,31,31,31,31,38,31,38,45,45,45,45,45,45,41");
        //PlayerPrefs.SetInt ("hyourou",100);
        //PlayerPrefs.SetString("cyoutei", "1000,1000,1000");
        PlayerPrefs.SetInt ("money",200000000);
        PlayerPrefs.SetInt ("busyoDama",9999999);
        PlayerPrefs.SetString ("gokuiItem","100,110,120,130,140");
        PlayerPrefs.SetString ("cyouheiYR","200,200,200");
        PlayerPrefs.SetString ("cyouheiKB","200,200,200");
        PlayerPrefs.SetString ("cyouheiYM","200,200,200");
        PlayerPrefs.SetString ("cyouheiTP","200,200,200");
        PlayerPrefs.SetInt ("hidensyoGe",999);
        PlayerPrefs.SetInt ("hidensyoCyu",999);
        PlayerPrefs.SetInt ("hidensyoJyo",999);
        //PlayerPrefs.SetInt ("19",99);
        //PlayerPrefs.SetInt("4", 199);
        //PlayerPrefs.SetInt ("exp19", 55143342);
        //PlayerPrefs.SetInt("exp4", 1830143342);
        //PlayerPrefs.SetInt("addlv4", 100);
        PlayerPrefs.SetInt("addlv19", 100);
        //PlayerPrefs.SetString("gacyaHst", "4,19,108");
        //PlayerPrefs.SetInt ("72",99);
        //PlayerPrefs.SetInt ("exp72",55143300);

        //PlayerPrefs.DeleteKey ("usedBusyo");
        PlayerPrefs.SetInt ("shinobiGe",99);
        PlayerPrefs.SetInt ("shinobiCyu",99);
        PlayerPrefs.SetInt ("shinobiJyo",100);
        PlayerPrefs.SetString("kengouItem","10,10,10,10,10,10,10,10,10,10");
        PlayerPrefs.SetString("nanbanItem","100,100,100");
        //PlayerPrefs.DeleteKey ("bakuhuTobatsuDaimyoId");
        //PlayerPrefs.DeleteKey ("soubujireiFlg");
        //		PlayerPrefs.SetInt ("saku19",19);
        //		PlayerPrefs.SetInt ("saku4",11);
		PlayerPrefs.SetString ("cyouhou",cyouhou);
        PlayerPrefs.SetString("kanjyo", "100,100,100");
        PlayerPrefs.SetString("shiro", "100,100,100,100");
        //PlayerPrefs.SetString ("doumei2","3");
        //PlayerPrefs.SetString ("doumei3","2");
        //PlayerPrefs.SetInt ("2gaikou3",100);
        //PlayerPrefs.SetString ("keyHistory","2-1");
        //PlayerPrefs.SetString ("2-1","11/7/2015 8:55:16 PM,2,1,斎藤義龍,織田信長,8000,19,7,left,True,3,3000,13-1-15-1");
        //PlayerPrefs.DeleteKey ("doneCyosyuFlg");
        //PlayerPrefs.SetString ("yearSeason","1560,1");
        //PlayerPrefs.SetString ("lastSeasonChanSetime","11/21/2015 2:50:00 AM");
        //PlayerPrefs.DeleteKey ("naiseiTabibitoCounter1");
        //PlayerPrefs.SetInt ("kuniLv",99);
        //PlayerPrefs.SetInt("kuniExp", 50131711);
        //PlayerPrefs.SetBool ("gameClearFlg",true);

        //PlayerPrefs.SetString ("clearedKuni","1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65");

        //Oda
        PlayerPrefs.SetString ("seiryoku","1,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1");
        PlayerPrefs.SetString("clearedKuni", "1,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65");
        PlayerPrefs.SetString("openKuni", "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65");
        PlayerPrefs.SetString("kuni2", "1,2,3,4,5,6,7,8,9");
        //PlayerPrefs.SetString("seiryoku", "31,31,31,4,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,31,1,1,1,1,1,1,1,1,1,1,1,1,1,1");
        //PlayerPrefs.SetString("seiryoku", "17,17,17,4,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,17,1,1,1,1,1,1,1,1,1,1,1,1,1,1");
        //PlayerPrefs.SetString("seiryoku", "10,10,10,4,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,1,1,1,1,1,1,1,1,1,1,1,1,1,1");

        //PlayerPrefs.SetString("seiryoku", "14,14,14,4,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,14,1,1,1,1,1,1,1,1,1,1,1,1,1,1");
        //PlayerPrefs.SetString ("seiryoku","5,2,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5");
        //PlayerPrefs.SetBool ("gameOverFlg",true);    
        //PlayerPrefs.SetString ("keyHistory","2-1");
        //PlayerPrefs.SetString ("2-1","12/5/2015 11:19:47 PM,2,1,斎藤義龍,織田信長,2800,-375,-160,right,False,,,");
        //PlayerPrefs.DeleteKey ("gameClearItemSetFlg");
        //PlayerPrefs.SetString("freeGacyaDate","12/15/2015 12:00:00 AM");
        //PlayerPrefs.SetInt("transferTP",100);
        //PlayerPrefs.SetInt("transferKB",100);
        //PlayerPrefs.SetInt("transferSNB",100);
        PlayerPrefs.SetString("cyoutei","20,30,40");
        PlayerPrefs.SetString("koueki","50,60,70");
        PlayerPrefs.SetString("gameClearDaimyo", "1,2,3,4,5");
        PlayerPrefs.SetString("gameClearDaimyoHard","2");
        //PlayerPrefs.SetString("gacyaHst", "16,107,108");
        //PlayerPrefs.SetString("gacyaHst","1,107,108");
        int myDaimyo = PlayerPrefs.GetInt("myDaimyo");
        PlayerPrefs.SetInt ("syogunDaimyoId",myDaimyo);
        //PlayerPrefs.SetBool ("gameClearFlg",true);
        //PlayerPrefs.DeleteKey("gameClearItemSetFlg");
        PlayerPrefs.SetInt("meisei",100);
        //PlayerPrefs.SetString("zukanBusyoHst","1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100");
        //PlayerPrefs.SetInt("kuniLv", 49);
        //PlayerPrefs.SetInt ("kuniExp",470553);




        //PlayerPrefs.SetString("myBusyo", "19,4,139,158,80,8,58,62,25,63,14,77,70,203,141,205,7,66,132,134,79,118,119,65,103,94,210,206,46,78,41,136,114,75,6,129,126,125,113,61,207,201,138,42,85");
        PlayerPrefs.SetInt("kuniLv", 53);
        PlayerPrefs.SetInt("kuniExp", 670013);
        PlayerPrefs.SetInt("myBusyoQty", 70);
        PlayerPrefs.SetInt("myDaimyo", 45);
        PlayerPrefs.SetString("openKuni", "64,62,61,63,63,64,57,59,54,56,58,60");
        PlayerPrefs.SetString("clearedKuni", "63,64,62,61,57,59");
        PlayerPrefs.SetString("seiryoku", "1,2,3,4,5,6,7,8,3,4,6,10,12,11,13,14,15,16,3,17,18,17,19,8,19,19,20,21,22,23,24,25,26,27,28,29,30,31,31,11,33,34,38,35,36,37,38,38,38,38,31,31,31,39,40,41,45,41,45,41,45,45,45,45,46");
        PlayerPrefs.SetInt("jinkei",4);
        PlayerPrefs.SetString("myBusyo", "19,4,139,158,80,8,58,62,25,63,14,77,70,203,141,205,7,66,132,134,79,118,119,65,103,94,210,206,46,78,41,136,114,75,6,129,126,125,113,61,207,201,138,42,85");
        PlayerPrefs.SetInt("jinkeiLimit", 6);
        PlayerPrefs.SetInt("jinkeiBusyoQty",1);
        PlayerPrefs.SetInt("jinkeiAveLv", 425);
        PlayerPrefs.SetInt("jinkeiAveChLv", 122);
        PlayerPrefs.SetInt("jinkeiHeiryoku", 86045);
        PlayerPrefs.SetInt("stockLimit", 76);


        List<string> busyoList = new List<string>() { "19","201","9","138","69","114","65","78","80","24","103","119","62","39","7","79","41","51","34","45","4","16","28","14","113","63","136","139","137","77","90","23","127","125","64","1","52","176","66","142","171","179","202","8","115","210","205","206","26","84","184","185","191","197","153","5","56","120","60","149","147","58","83","38","131","177","180","132","150","133" };
        StatusSet stat = new StatusSet();
        //string busyoList = "";
        for(int i=0; i<busyoList.Count; i++) {
            string busyoId = busyoList[i];
        
            PlayerPrefs.SetInt(busyoId, 1);

            string hei = "hei" + busyoId;

            string heisyu = stat.SetHeisyu(int.Parse(busyoId));
            string heiValue = heisyu + ":1:1:1";
            PlayerPrefs.SetString(hei, heiValue);

            string senpou = "senpou" + busyoId;
            PlayerPrefs.SetInt(senpou, 1); //Lv

            string saku = "saku" + busyoId;
            PlayerPrefs.SetInt(saku, 1); //Lv

            string kahou = "kahou" + busyoId;
            PlayerPrefs.SetString(kahou, "0,0,0,0,0,0,0,0");

            string exp = "exp" + busyoId;
            PlayerPrefs.SetInt(exp, 0);
        }
        //PlayerPrefs.SetString("myBusyo", busyoList);

        PlayerPrefs.SetString("loginDate", "02/20/2017 00:00:00");
        PlayerPrefs.SetBool("gameClearFlg",false);
        PlayerPrefs.SetInt("kuniLv", 29);
        PlayerPrefs.SetInt("kuniExp", 65242);
        PlayerPrefs.SetInt("myBusyoQty", 15);
        PlayerPrefs.SetInt("myDaimyo",1);
        PlayerPrefs.SetString("openKuni", "2,3,4,1,8,9,6,10,11,12,13,14,7,15,5,21,16,38,39,17,40,18,20,41,42,43,44");
        PlayerPrefs.SetString("clearedKuni", "1,4,10,11,7,5,21,12,16,38,3,14,13,15,40,2,6,18,41,17");
        PlayerPrefs.SetString("seiryoku", "1,1,1,1,1,1,1,19,19,1,1,1,1,1,1,1,1,1,19,19,1,19,8,8,19,19,20,17,17,23,23,27,26,27,28,29,29,1,31,1,1,31,38,38,36,38,38,38,38,38,31,31,31,41,31,38,41,41,41,41,41,41,41,45,41");
        PlayerPrefs.SetString("doumei", "8,17,38,41,36");
        //PlayerPrefs.SetString("keyHistory");
        PlayerPrefs.SetInt("jinkei",2);

        List<string> myBusyoJinkei_list = new List<string>() { "0","0","0","0","0","0","14","0","0","0","19","0","0","0","0","0","16","0","0","0","0","0","0","0","0" };
        for (int i = 1; i < 26; i++) {
            string mapId = "2map" + i.ToString();
            int count = i - 1;
            PlayerPrefs.SetInt(mapId, int.Parse(myBusyoJinkei_list[count]));
        }

        
        PlayerPrefs.SetInt("jinkeiLimit",4);
        PlayerPrefs.SetInt("jinkeiBusyoQty",1);
        PlayerPrefs.SetInt("jinkeiAveLv",182);
        PlayerPrefs.SetInt("jinkeiAveChLv",46);
        PlayerPrefs.SetInt("jinkeiHeiryoku",29510);
        PlayerPrefs.SetInt("stockLimit",43);

        //busyo
        string myBusyoStatus = "9;TP:4:21:23;2;1;25,0,4,6,44,0,11,0;1642394&16;YR:4:15:15;3;1;4,10,19,13,46,43,2,3;2745151&94;TP:1:2:2;1;1;0,0,0,0,0,0,0,0;2601&121;TP:3:16:17;1;1;25,9,0,0,0,41,5,0;477720&210;TP:1:1:1;1;1;0,0,0,0,0,0,0,0;0&129;KB:2:10:11;2;1;8,0,0,0,0,48,0,0;43910&114;YR:1:1:1;1;1;21,0,0,0,0,0,0,0;2231&134;YR:2:16:16;1;1;0,0,0,0,0,0,0,0;91620&14;YR:4:10:10;2;1;28,3,5,3,9,38,8,0;570415&33;0;1;0;0,0,0,0,0,0,0,0;19755&62;KB:1:1:1;1;1;0,0,0,0,0,0,0,0;0&209;YR:1:1:1;1;1;9,0,0,0,0,0,0,0;0&38;TP:1:1:1;1;1;0,0,0,0,0,0,0,0;0&56;YR:1:1:1;1;1;0,0,0,0,0,0,0,0;0&120;TP:1:1:1;1;1;0,0,0,0,0,0,0,0;0";
        List<string> myBusyoStatusList = new List<string>();
        char[] delimiterChars = { '&' };
        myBusyoStatusList = new List<string>(myBusyoStatus.Split(delimiterChars));

        char[] delimiterChars2 = { ';' };
        for (int i = 0; i < myBusyoStatusList.Count; i++) {
            string myBusyoUnitStatus = myBusyoStatusList[i];
            List<string> myBusyoUnitStatusList = new List<string>();
            myBusyoUnitStatusList = new List<string>(myBusyoUnitStatus.Split(delimiterChars2));

            
            string busyoId = myBusyoUnitStatusList[0];
            string tempHei = "hei" + busyoId;
            string tempSenpou = "senpou" + busyoId;
            string tempSaku = "saku" + busyoId;
            string tempKahou = "kahou" + busyoId;
            string tempExp = "exp" + busyoId;
            
            PlayerPrefs.SetString(tempHei, myBusyoUnitStatusList[1]);
            PlayerPrefs.SetInt(tempSenpou, int.Parse(myBusyoUnitStatusList[2]));
            PlayerPrefs.SetInt(tempSaku, int.Parse(myBusyoUnitStatusList[3]));
            PlayerPrefs.SetString(tempKahou, myBusyoUnitStatusList[4]);
            
            

        }
        

        //busyo
        //PlayerPrefs.SetInt("19", 90);
        //PlayerPrefs.SetInt("exp19", 22526737);
        //PlayerPrefs.SetString("hei19", "TP:20:98:107");
        //PlayerPrefs.SetString("kahou19", "12,20,13,3,80,80,12,12");
        */

        //PlayerPrefs.DeleteKey("PvPName");
        PlayerPrefs.Flush();
        //Application.LoadLevel("souko");
 
        Application.LoadLevel("mainStage");
		//Application.LoadLevel("clearOrGameOver");
	}	
}
