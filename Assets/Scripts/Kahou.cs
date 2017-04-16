using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class Kahou : MonoBehaviour {

	public string getRamdomKahou(int kahouType, int kahouRank){
		//kahouType Bugu,Kabuto,Gusoku,Meiba,Cyadougu,Heihousyo,Chishikisyo=1,2,3,4,5,6,7
		//kahouRank S,A,B,C=1,2,3,4

		int kahouId = 0;
		string kahouName = "";

		if (kahouType == 1) {
			Entity_kahou_bugu_mst Mst = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;

			List<int> kahouList = new List<int> ();
			if(kahouRank==1){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="S"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==2){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="A"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==3){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="B"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==4){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="C"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}


			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];
				registerBugu(kahouId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    kahouName = Mst.param[kahouId-1].kahouNameEng;
                }else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                }
			}

		} else if (kahouType == 2) {
			Entity_kahou_kabuto_mst Mst = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank==1){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="S"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==2){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="A"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==3){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="B"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==4){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="C"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			

			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];
				registerKabuto(kahouId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    kahouName = Mst.param[kahouId-1].kahouNameEng;
                }else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                }

			}
		} else if (kahouType == 3) {
			Entity_kahou_gusoku_mst Mst = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank==1){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="S"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==2){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="A"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==3){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="B"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==4){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="C"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];
				registerGusoku(kahouId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    kahouName = Mst.param[kahouId-1].kahouNameEng;
                }else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                }

			}

		} else if (kahouType == 4) {
			Entity_kahou_meiba_mst Mst = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank==1){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="S"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==2){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="A"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==3){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="B"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==4){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="C"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];
				registerMeiba(kahouId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    kahouName = Mst.param[kahouId-1].kahouNameEng;
                }else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                }
			}
		} else if (kahouType == 5) {
			Entity_kahou_cyadougu_mst Mst = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank==1){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="S"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==2){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="A"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==3){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="B"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==4){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="C"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];
				registerCyadougu(kahouId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    kahouName = Mst.param[kahouId-1].kahouNameEng;
                }else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                }

			}
		} else if (kahouType == 6) {
			Entity_kahou_heihousyo_mst Mst = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank==1){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="S"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==2){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="A"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==3){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="B"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==4){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="C"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];
				registerHeihousyo(kahouId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    kahouName = Mst.param[kahouId-1].kahouNameEng;
                }else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                }
			}
		} else if (kahouType == 7) {
			Entity_kahou_chishikisyo_mst Mst = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank==1){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="S"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==2){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="A"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==3){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="B"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank==4){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="C"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];
				registerChishikisyo(kahouId);
                if (Application.systemLanguage != SystemLanguage.Japanese) {
                    kahouName = Mst.param[kahouId-1].kahouNameEng;
                }else {
                    kahouName = Mst.param[kahouId - 1].kahouName;
                }
			}
		}

		return kahouName;
	}
	
	public void registerBugu(int kahouId){
		string availableBugu = PlayerPrefs.GetString ("availableBugu");
		if (availableBugu != null && availableBugu != "") {
			availableBugu = availableBugu + "," + kahouId.ToString ();
		} else {
			availableBugu = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("availableBugu",availableBugu);

		string zukanHst = PlayerPrefs.GetString ("zukanBuguHst");
		if (zukanHst != null && zukanHst != "") {
			zukanHst = zukanHst + "," + kahouId.ToString ();
		} else {
			zukanHst = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("zukanBuguHst",zukanHst);

		PlayerPrefs.Flush ();	
	}

	public void registerKabuto(int kahouId){
		string availableKabuto = PlayerPrefs.GetString ("availableKabuto");
		if (availableKabuto != null && availableKabuto != "") {
			availableKabuto = availableKabuto + "," + kahouId.ToString ();
		} else {
			availableKabuto = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("availableKabuto",availableKabuto);

		string zukanHst = PlayerPrefs.GetString ("zukanKabutoHst");
		if (zukanHst != null && zukanHst != "") {
			zukanHst = zukanHst + "," + kahouId.ToString ();
		} else {
			zukanHst = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("zukanKabutoHst",zukanHst);
		PlayerPrefs.Flush ();	
	}

	public void registerGusoku(int kahouId){
		string availableGusoku = PlayerPrefs.GetString ("availableGusoku");
		if (availableGusoku != null && availableGusoku != "") {
			availableGusoku = availableGusoku + "," + kahouId.ToString ();
		} else {
			availableGusoku = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("availableGusoku",availableGusoku);

		string zukanHst = PlayerPrefs.GetString ("zukanGusokuHst");
		if (zukanHst != null && zukanHst != "") {
			zukanHst = zukanHst + "," + kahouId.ToString ();
		} else {
			zukanHst = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("zukanGusokuHst",zukanHst);
		PlayerPrefs.Flush ();	
	}

	public void registerMeiba(int kahouId){
		string availableMeiba = PlayerPrefs.GetString ("availableMeiba");
		if (availableMeiba != null && availableMeiba != "") {
			availableMeiba = availableMeiba + "," + kahouId.ToString ();
		} else {
			availableMeiba = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("availableMeiba",availableMeiba);

		string zukanHst = PlayerPrefs.GetString ("zukanMeibaHst");
		if (zukanHst != null && zukanHst != "") {
			zukanHst = zukanHst + "," + kahouId.ToString ();
		} else {
			zukanHst = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("zukanMeibaHst",zukanHst);

		PlayerPrefs.Flush ();	
	}

	public void registerCyadougu(int kahouId){
		string availableCyadougu = PlayerPrefs.GetString ("availableCyadougu");
		if (availableCyadougu != null && availableCyadougu != "") {
			availableCyadougu = availableCyadougu + "," + kahouId.ToString ();
		} else {
			availableCyadougu = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("availableCyadougu",availableCyadougu);

		string zukanHst = PlayerPrefs.GetString ("zukanCyadouguHst");
		if (zukanHst != null && zukanHst != "") {
			zukanHst = zukanHst + "," + kahouId.ToString ();
		} else {
			zukanHst = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("zukanCyadouguHst",zukanHst);
		PlayerPrefs.Flush ();	
	}

	public void registerHeihousyo(int kahouId){
		string availableHeihousyo = PlayerPrefs.GetString ("availableHeihousyo");
		if (availableHeihousyo != null && availableHeihousyo != "") {
			availableHeihousyo = availableHeihousyo + "," + kahouId.ToString ();
		} else {
			availableHeihousyo = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("availableHeihousyo",availableHeihousyo);

		string zukanHst = PlayerPrefs.GetString ("zukanHeihousyoHst");
		if (zukanHst != null && zukanHst != "") {
			zukanHst = zukanHst + "," + kahouId.ToString ();
		} else {
			zukanHst = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("zukanHeihousyoHst",zukanHst);
		PlayerPrefs.Flush ();	
	}

	public void registerChishikisyo(int kahouId){
		string availableChishikisyo = PlayerPrefs.GetString ("availableChishikisyo");
		if (availableChishikisyo != null && availableChishikisyo != "") {
			availableChishikisyo = availableChishikisyo + "," + kahouId.ToString ();
		} else {
			availableChishikisyo = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("availableChishikisyo",availableChishikisyo);

		string zukanHst = PlayerPrefs.GetString ("zukanChishikisyoHst");
		if (zukanHst != null && zukanHst != "") {
			zukanHst = zukanHst + "," + kahouId.ToString ();
		} else {
			zukanHst = kahouId.ToString ();
		}
		PlayerPrefs.SetString ("zukanChishikisyoHst",zukanHst);
		PlayerPrefs.Flush ();	
	}




	public int getRamdomKahouId(string kahouType, string kahouRank){
		//kahouType bugu,kabuto,gusoku,meiba,cyadougu,heihousyo,chishikisyo
		//kahouRank S,A,B,C

		int kahouId = 0;

		if (kahouType == "bugu") {
			Entity_kahou_bugu_mst Mst = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
			
			List<int> kahouList = new List<int> ();
			if(kahouRank=="S"){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="A"){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="B"){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank=="B"){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="C"){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];

			}
			
		} else if (kahouType == "kabuto") {
			Entity_kahou_kabuto_mst Mst = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank=="S"){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="A"){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="B"){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="C"){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			
			
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];

			}
		} else if (kahouType == "gusoku") {
			Entity_kahou_gusoku_mst Mst = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank=="S"){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="A"){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="B"){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="C"){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];

			}
			
		} else if (kahouType == "meiba") {
			Entity_kahou_meiba_mst Mst = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank=="S"){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="A"){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="B"){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="C"){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];

				
			}
		} else if (kahouType == "cyadougu") {
			Entity_kahou_cyadougu_mst Mst = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank=="S"){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="A"){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="B"){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="C"){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];

				
			}
		} else if (kahouType == "heihousyo") {
			Entity_kahou_heihousyo_mst Mst = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank=="S"){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="A"){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="B"){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="C"){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];

				
			}
		} else if (kahouType == "chishikisyo") {
			Entity_kahou_chishikisyo_mst Mst = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
			List<int> kahouList = new List<int> ();
			if(kahouRank=="S"){
				//S
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="A"){
				//A
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="B"){
				//B
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}else if(kahouRank=="C"){
				//C
				for(int i=0; i<Mst.param.Count;i++){
					string tempKahouRank = Mst.param[i].kahouRank;
					if(tempKahouRank==kahouRank){
						kahouList.Add(Mst.param[i].id);
					}
				}
			}
			if(kahouList.Count !=0){
				int rdmId = UnityEngine.Random.Range(0,kahouList.Count);
				kahouId = kahouList[rdmId];

			}
		}
		
		return kahouId;
	}


	public string getKahouRank(string kahouType, int kahouId){
		string kahouRank = "";

		if (kahouType == "cyadougu") {
			Entity_kahou_cyadougu_mst Mst = Resources.Load ("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
			kahouRank = Mst.param [kahouId - 1].kahouRank;
		} else if (kahouType == "heihousyo") {
			Entity_kahou_heihousyo_mst Mst = Resources.Load ("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
			kahouRank = Mst.param [kahouId - 1].kahouRank;
		} else if (kahouType == "chishikisyo") {
			Entity_kahou_chishikisyo_mst Mst = Resources.Load ("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
			kahouRank = Mst.param [kahouId - 1].kahouRank;
		} else if (kahouType == "bugu") {
			Entity_kahou_bugu_mst Mst = Resources.Load ("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
			kahouRank = Mst.param [kahouId - 1].kahouRank;
		} else if (kahouType == "meiba") {
			Entity_kahou_meiba_mst Mst = Resources.Load ("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
			kahouRank = Mst.param [kahouId - 1].kahouRank;
		} else if (kahouType == "gusoku") {
			Entity_kahou_gusoku_mst Mst = Resources.Load ("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
			kahouRank = Mst.param [kahouId - 1].kahouRank;
		}else if (kahouType == "kabuto") {
			Entity_kahou_kabuto_mst Mst = Resources.Load ("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
			kahouRank = Mst.param [kahouId - 1].kahouRank;
		}

		return kahouRank;
	}

    public string getKahouName(string kahouType, int kahouId) {
        string kahouName = "";
        if (Application.systemLanguage != SystemLanguage.Japanese) {
            if (kahouType == "cyadougu") {
                Entity_kahou_cyadougu_mst Mst = Resources.Load("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
                kahouName = Mst.param[kahouId - 1].kahouNameEng;
            }
            else if (kahouType == "heihousyo") {
                Entity_kahou_heihousyo_mst Mst = Resources.Load("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
                kahouName = Mst.param[kahouId - 1].kahouNameEng;
            }
            else if (kahouType == "chishikisyo") {
                Entity_kahou_chishikisyo_mst Mst = Resources.Load("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
                kahouName = Mst.param[kahouId - 1].kahouNameEng;
            }
            else if (kahouType == "bugu") {
                Entity_kahou_bugu_mst Mst = Resources.Load("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
                kahouName = Mst.param[kahouId - 1].kahouNameEng;
            }
            else if (kahouType == "meiba") {
                Entity_kahou_meiba_mst Mst = Resources.Load("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
                kahouName = Mst.param[kahouId - 1].kahouNameEng;
            }
            else if (kahouType == "gusoku") {
                Entity_kahou_gusoku_mst Mst = Resources.Load("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
                kahouName = Mst.param[kahouId - 1].kahouNameEng;
            }
            else if (kahouType == "kabuto") {
                Entity_kahou_kabuto_mst Mst = Resources.Load("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
                kahouName = Mst.param[kahouId - 1].kahouNameEng;
            }
        }else {
            if (kahouType == "cyadougu") {
                Entity_kahou_cyadougu_mst Mst = Resources.Load("Data/kahou_cyadougu_mst") as Entity_kahou_cyadougu_mst;
                kahouName = Mst.param[kahouId - 1].kahouName;
            }
            else if (kahouType == "heihousyo") {
                Entity_kahou_heihousyo_mst Mst = Resources.Load("Data/kahou_heihousyo_mst") as Entity_kahou_heihousyo_mst;
                kahouName = Mst.param[kahouId - 1].kahouName;
            }
            else if (kahouType == "chishikisyo") {
                Entity_kahou_chishikisyo_mst Mst = Resources.Load("Data/kahou_chishikisyo_mst") as Entity_kahou_chishikisyo_mst;
                kahouName = Mst.param[kahouId - 1].kahouName;
            }
            else if (kahouType == "bugu") {
                Entity_kahou_bugu_mst Mst = Resources.Load("Data/kahou_bugu_mst") as Entity_kahou_bugu_mst;
                kahouName = Mst.param[kahouId - 1].kahouName;
            }
            else if (kahouType == "meiba") {
                Entity_kahou_meiba_mst Mst = Resources.Load("Data/kahou_meiba_mst") as Entity_kahou_meiba_mst;
                kahouName = Mst.param[kahouId - 1].kahouName;
            }
            else if (kahouType == "gusoku") {
                Entity_kahou_gusoku_mst Mst = Resources.Load("Data/kahou_gusoku_mst") as Entity_kahou_gusoku_mst;
                kahouName = Mst.param[kahouId - 1].kahouName;
            }
            else if (kahouType == "kabuto") {
                Entity_kahou_kabuto_mst Mst = Resources.Load("Data/kahou_kabuto_mst") as Entity_kahou_kabuto_mst;
                kahouName = Mst.param[kahouId - 1].kahouName;
            }
        }
        return kahouName;
    }



    public List<string> getMyRandomKahouCdId(){
		List<string> kahouCdId = new List<string> ();
		List<string> myKahouMst = new List<string> ();

		string buguTemp = PlayerPrefs.GetString ("availableBugu");
		if (buguTemp != null && buguTemp != "") {
			myKahouMst.Add ("bugu");
		}

		string kabutoTemp = PlayerPrefs.GetString ("availableKabuto");
		if (kabutoTemp != null && kabutoTemp != "") {
			myKahouMst.Add ("kabuto");
		}

		string gusokuTemp = PlayerPrefs.GetString ("availableGusoku");
		if (gusokuTemp != null && gusokuTemp != "") {
			myKahouMst.Add ("gusoku");
		}

		string meibaTemp = PlayerPrefs.GetString ("availableMeiba");
		if (meibaTemp != null && meibaTemp != "") {
			myKahouMst.Add ("meiba");
		}

		string cyadouguTemp = PlayerPrefs.GetString ("availableCyadougu");
		if (cyadouguTemp != null && cyadouguTemp != "") {
			myKahouMst.Add ("cyadougu");
		}

		string heihousyoTemp = PlayerPrefs.GetString ("availableHeihousyo");
		if (heihousyoTemp != null && heihousyoTemp != "") {
			myKahouMst.Add ("heihousyo");
		}

		string chishikisyoTemp = PlayerPrefs.GetString ("availableChishikisyo");
		if (chishikisyoTemp != null && chishikisyoTemp != "") {
			myKahouMst.Add ("chishikisyo");
		}

		if (myKahouMst.Count != 0) {
			int rdm = UnityEngine.Random.Range (0, myKahouMst.Count);
			kahouCdId.Add(myKahouMst [rdm]);
			string kahouId = "";
			string kahouDataCd = "";
			if (myKahouMst [rdm] == "bugu") {
				kahouId = getMyRandomKahouId (buguTemp);
				kahouDataCd = "availableBugu";
			} else if (myKahouMst [rdm] == "kabuto") {
				kahouId = getMyRandomKahouId (kabutoTemp);
				kahouDataCd = "availableKabuto";
			} else if (myKahouMst [rdm] == "gusoku") {
				kahouId = getMyRandomKahouId (gusokuTemp);
				kahouDataCd = "availableGusoku";
			}else if (myKahouMst [rdm] == "meiba") {
				kahouId = getMyRandomKahouId (meibaTemp);
				kahouDataCd = "availableMeiba";
			} else if (myKahouMst [rdm] == "cyadougu") {
				kahouId = getMyRandomKahouId (cyadouguTemp);
				kahouDataCd = "availableCyadougu";
			}else if (myKahouMst [rdm] == "heihousyo") {
				kahouId = getMyRandomKahouId (heihousyoTemp);
				kahouDataCd = "availableHeihousyo";
			} else if (myKahouMst [rdm] == "chishikisyo") {
				kahouId = getMyRandomKahouId (chishikisyoTemp);
				kahouDataCd = "availableChishikisyo";
			}
			kahouCdId.Add(kahouId);
			kahouCdId.Add(kahouDataCd);
		}

		return kahouCdId;
	}

	public string getMyRandomKahouId(string temp){
		string kahouId = "";
		List<string> myKahouList = new List<string> ();
		char[] delimiterChars = {','};
		if (temp != null && temp != "") {
			if (temp.Contains (",")) {
				myKahouList = new List<string> (temp.Split (delimiterChars));
			} else {
				myKahouList.Add (temp);
			}
		}
		int rdm = UnityEngine.Random.Range (0, myKahouList.Count);
		kahouId = myKahouList [rdm];

		return kahouId;
	}

}
