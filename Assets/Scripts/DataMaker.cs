using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class DataMaker : MonoBehaviour {

	public void Start () {
		PlayerPrefs.DeleteKey("jinkei");
		for (int i=1; i<26; i++) {
			string mapTemp = "map" + i.ToString();
			PlayerPrefs.DeleteKey(mapTemp);
		}
		for (int i=1; i<99; i++) {
			string busyoTemp = i.ToString();
			PlayerPrefs.DeleteKey(busyoTemp);

			string heiTemp = "hei" + i.ToString();
			PlayerPrefs.DeleteKey(heiTemp);

			string sakuTemp = "saku" + i.ToString();
			PlayerPrefs.DeleteKey(sakuTemp);

			string senpouTemp = "senpou" + i.ToString();
			PlayerPrefs.DeleteKey(senpouTemp);

			string koudouTemp = "koudou" + i.ToString();
			PlayerPrefs.DeleteKey(koudouTemp);

			string kahouTemp = "kahou" + i.ToString();
			PlayerPrefs.DeleteKey(kahouTemp);

			string expTemp = "exp" + i.ToString();
			PlayerPrefs.DeleteKey(expTemp);

			string jyosyuHeiTemp = "jyosyuHei" + i.ToString();
			PlayerPrefs.DeleteKey(jyosyuHeiTemp);

			string jyosyuBusyoTemp = "jyosyuBusyo" + i.ToString();
			PlayerPrefs.DeleteKey(jyosyuBusyoTemp);

		}
		for (int i=1; i<5; i++) {
			for(int k=1; k<26;k++){
				string jinkeiTemp = i.ToString() + "map" + k.ToString();
				PlayerPrefs.DeleteKey(jinkeiTemp);
			}
		}

		PlayerPrefs.DeleteKey("myBusyo");
		PlayerPrefs.DeleteKey("availableBugu");
		PlayerPrefs.DeleteKey("availableKabuto");
		PlayerPrefs.DeleteKey("availableGusoku");
		PlayerPrefs.DeleteKey("availableMeiba");
		PlayerPrefs.DeleteKey("availableCyadougu");
		PlayerPrefs.DeleteKey("availableHeihousyo");
		PlayerPrefs.DeleteKey("availableChishikisyo");
		PlayerPrefs.DeleteKey("activeStageMoney");
		PlayerPrefs.DeleteKey("activeStageExp");
		PlayerPrefs.DeleteKey("cyouheiYR");
		PlayerPrefs.DeleteKey("cyouheiKB");
		PlayerPrefs.DeleteKey("cyouheiTP");
		PlayerPrefs.DeleteKey("cyouheiYM");
		PlayerPrefs.DeleteKey("kanjyo");
		PlayerPrefs.DeleteKey("money");
		PlayerPrefs.DeleteKey("busyoDama");
		PlayerPrefs.DeleteKey("gacyaHst");
		PlayerPrefs.DeleteKey("touyouHst");
		PlayerPrefs.DeleteKey("kuniLv");
		PlayerPrefs.DeleteKey("kuniExp");
		PlayerPrefs.DeleteKey("jinkeiLimit");
		PlayerPrefs.DeleteKey("stockLimit");
		PlayerPrefs.DeleteKey("jinkeiAveLv");
		PlayerPrefs.DeleteKey("jinkeiAveChLv");
		PlayerPrefs.DeleteKey("jinkeiBusyoQty");
		PlayerPrefs.DeleteKey("jinkeiHeiryoku");
		PlayerPrefs.DeleteKey("soudaisyo1");
		PlayerPrefs.DeleteKey("soudaisyo2");
		PlayerPrefs.DeleteKey("soudaisyo3");
		PlayerPrefs.DeleteKey("soudaisyo4");
		PlayerPrefs.DeleteKey("myBusyoQty");
		PlayerPrefs.DeleteKey("hidensyoGe");
		PlayerPrefs.DeleteKey("hidensyoCyu");
		PlayerPrefs.DeleteKey("hidensyoJyo");
		PlayerPrefs.DeleteKey("shinobiGe");
		PlayerPrefs.DeleteKey("shinobiCyu");
		PlayerPrefs.DeleteKey("shinobiJyo");
		PlayerPrefs.DeleteKey("openKuni");
		PlayerPrefs.DeleteKey("clearedKuni");
		PlayerPrefs.DeleteKey("gameOverFlg");
		PlayerPrefs.DeleteKey("freeGacyaDate");
		PlayerPrefs.DeleteKey("freeGacyaCounter");
		PlayerPrefs.DeleteKey("cyouhou");
		PlayerPrefs.DeleteKey("cyoutekiDaimyo");

		for(int i=1; i<66; i++){
			string kuniTemp = "kuni" + i.ToString();
			PlayerPrefs.DeleteKey(kuniTemp);

			string naiseiTemp = "naisei" + i.ToString();
			PlayerPrefs.DeleteKey(naiseiTemp);

			string jyosyuTemp = "jyosyu" + i.ToString();
			PlayerPrefs.DeleteKey(jyosyuTemp);

			string naiseiLoginDateTemp = "naiseiLoginDate" + i.ToString();
			PlayerPrefs.DeleteKey(naiseiLoginDateTemp);

			string naiseiTabibitoCounterTemp = "naiseiTabibitoCounter" + i.ToString();
			PlayerPrefs.DeleteKey(naiseiTabibitoCounterTemp);

			string cyouhouTemp = "cyouhou" + i.ToString();
			PlayerPrefs.DeleteKey(cyouhouTemp);
		}

		//Item
		PlayerPrefs.DeleteKey("gokuiItem");
		PlayerPrefs.DeleteKey("kengouItem");
		PlayerPrefs.DeleteKey("nanbanItem");

		//Active Item
		PlayerPrefs.DeleteKey("activeItemType");
		PlayerPrefs.DeleteKey("activeItemId");
		PlayerPrefs.DeleteKey("activeItemRatio");
		PlayerPrefs.DeleteKey("activeItemQty");
		PlayerPrefs.DeleteKey("activeStageId");
		PlayerPrefs.DeleteKey("activeKuniId");
		PlayerPrefs.DeleteKey("kuniClearedFlg");

		PlayerPrefs.DeleteKey("activeKuniName");
		PlayerPrefs.DeleteKey("fromNaiseiFlg");
		PlayerPrefs.DeleteKey("yearSeason");
		PlayerPrefs.DeleteKey("hyourouMax");
		PlayerPrefs.DeleteKey("lastSeasonChangeTime");
		PlayerPrefs.DeleteKey("lasttime");
		PlayerPrefs.DeleteKey("doneCyosyuFlg");
		PlayerPrefs.DeleteKey("seiryoku");
		PlayerPrefs.DeleteKey("myDaimyo");
		PlayerPrefs.DeleteKey("initDataFlg");
		PlayerPrefs.DeleteKey("myDaimyoBusyo");
		PlayerPrefs.DeleteKey("activeBusyoQty");
		PlayerPrefs.DeleteKey("activeBusyoLv");
		PlayerPrefs.DeleteKey("activeButaiQty");
		PlayerPrefs.DeleteKey("activeButaiLv");
		PlayerPrefs.DeleteKey("activeBoubi");
		PlayerPrefs.DeleteKey("weather");
		PlayerPrefs.DeleteKey("mntMinusStatus");
		PlayerPrefs.DeleteKey("seaMinusStatus");
		PlayerPrefs.DeleteKey("rainMinusStatus");
		PlayerPrefs.DeleteKey("snowMinusStatus");

		//Kanni
		PlayerPrefs.DeleteKey("myKanniWithBusyo");
		PlayerPrefs.DeleteKey("cyouteiPoint");

		//Gaikou
		for(int x =2; x<47; x++ ){
			for(int y=2; y<47; y++){
				if(x != y){
					string temp = x.ToString() + "gaikou" + y.ToString();
					string temp2 = x.ToString() + "key" + y.ToString();
					PlayerPrefs.DeleteKey(temp);
					PlayerPrefs.DeleteKey(temp2);
				}
			}
		}

		PlayerPrefs.DeleteKey("keyHistory");
		PlayerPrefs.DeleteKey("metsubou");


		//Doumei
		PlayerPrefs.DeleteKey("doumei");
		for(int x=2; x<47; x++){
			string temp = "doumei" + x.ToString();
			PlayerPrefs.DeleteKey(temp);

			string metsubouTemp = "metsubou" + x.ToString();
			PlayerPrefs.DeleteKey(metsubouTemp);

		}

		PlayerPrefs.DeleteKey("playerEngunList");
		PlayerPrefs.DeleteKey("enemyEngunList");
		PlayerPrefs.DeleteKey("playerKyoutouList");
		PlayerPrefs.DeleteKey("tempKyoutouList");


		PlayerPrefs.DeleteKey("gameOverFlg");
		PlayerPrefs.DeleteKey("gameClearFlg");
		PlayerPrefs.DeleteKey("gameClearItemGetFlg");
		PlayerPrefs.DeleteKey("gameClearDaimyo");


		PlayerPrefs.Flush();
	}
}
