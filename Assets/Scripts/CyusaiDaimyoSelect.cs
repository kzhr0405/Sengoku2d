using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class CyusaiDaimyoSelect : MonoBehaviour {

	public string daimyoId = "";
	public string daimyoName = "";
	public string daimyoId2 = "";
	public string daimyoName2 = "";
	public List<string> checkedDaimyoList;
	public GameObject uprContent;
	public GameObject btnContent;
	public bool bottomFlg;

	public void OnClick(){
        int langId = PlayerPrefs.GetInt("langId");
        int senarioId = PlayerPrefs.GetInt("senarioId");

        AudioSource[] audioSources = GameObject.Find ("SEController").GetComponents<AudioSource> ();
		audioSources [2].Play ();

		//Upper
		if (!bottomFlg) {
			//Color
			Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
			Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
			foreach (Transform obj in uprContent.transform) {
				obj.GetComponent<Image> ().color = unSelect;
			}
			GetComponent<Image> ().color = Select;

			//Reset
			foreach (Transform obj in btnContent.transform) {
				Destroy (obj.gameObject);
			}


			string btnSlot = "Prefabs/Bakuhu/CyusaiSlot";
			Daimyo daimyo = new Daimyo ();
			foreach (string dstDaimyoId in checkedDaimyoList) {
				if (dstDaimyoId != daimyoId) {
					GameObject btmSlot = Instantiate (Resources.Load (btnSlot)) as GameObject;
					btmSlot.transform.SetParent (btnContent.transform);
					btmSlot.transform.localScale = new Vector2 (1, 1);

					int daimyoBusyoId = daimyo.getDaimyoBusyoId (int.Parse (dstDaimyoId), senarioId);
					string daimyoName2 = daimyo.getName (int.Parse (dstDaimyoId),langId,senarioId);

					string daimyoBusyoPath = "Prefabs/Player/Sprite/unit" + daimyoBusyoId.ToString ();
					btmSlot.transform.Find ("Image").transform.Find ("Image").GetComponent<Image> ().sprite = 
						Resources.Load (daimyoBusyoPath, typeof(Sprite)) as Sprite;
					btmSlot.transform.Find ("DaimyoName").GetComponent<Text> ().text = daimyoName2;
					btmSlot.GetComponent<CyusaiDaimyoSelect> ().bottomFlg = true;
					btmSlot.GetComponent<CyusaiDaimyoSelect> ().uprContent = uprContent;
					btmSlot.GetComponent<CyusaiDaimyoSelect> ().btnContent = btnContent;
					btmSlot.GetComponent<CyusaiDaimyoSelect> ().daimyoId = daimyoId;
					btmSlot.GetComponent<CyusaiDaimyoSelect> ().daimyoId = daimyoId;
					btmSlot.GetComponent<CyusaiDaimyoSelect> ().daimyoId2 = dstDaimyoId;
					btmSlot.GetComponent<CyusaiDaimyoSelect> ().daimyoName = daimyoName;
					btmSlot.GetComponent<CyusaiDaimyoSelect> ().daimyoName2 = daimyoName2;
				}
			}
		} else {
			//Bottom
			//Color
			Color Select = new Color (76f / 255f, 50f / 255f, 18f / 255f, 80f / 255f);
			Color unSelect = new Color (255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
			foreach (Transform obj in btnContent.transform) {
				obj.GetComponent<Image> ().color = unSelect;
			}
			GetComponent<Image> ().color = Select;

			//Confirm Button
			//Back Cover
			string backPath = "Prefabs/Busyo/back";
			GameObject back = Instantiate (Resources.Load (backPath)) as GameObject;
			back.transform.SetParent(GameObject.Find ("Map").transform);
			back.transform.localScale = new Vector2 (1, 1);
			RectTransform backTransform = back.GetComponent<RectTransform> ();
			backTransform.anchoredPosition3D = new Vector3 (0, 0, 0);

			//Message Box
			string msgPath = "Prefabs/Bakuhu/CyusaiConfirm";
			GameObject msg = Instantiate (Resources.Load (msgPath)) as GameObject;
			msg.transform.SetParent(GameObject.Find ("Map").transform);
			msg.transform.localScale = new Vector2 (1, 1);
			RectTransform msgTransform = msg.GetComponent<RectTransform> ();
			msgTransform.anchoredPosition3D = new Vector3 (0, 0, 0);
			msgTransform.name = "CyusaiConfirm";
            if (langId == 2) {
                msg.transform.Find ("Text").GetComponent<Text> ().text = "Do you want to mediate between " + daimyoName + " and " + daimyoName2 + "? \n Friendship will increase.";
            }else if(langId==3) {
                msg.transform.Find("Text").GetComponent<Text>().text = "仲裁" + daimyoName + "和" + daimyoName2 + "吗？两大名的友好度上升了。";
            } else {
                msg.transform.Find("Text").GetComponent<Text>().text = daimyoName + "と" + daimyoName2 + "を仲裁しますか？\n二大名間の友好度が上がります。";
            }


			GameObject YesBtn = msg.transform.Find ("YesButton").gameObject;
			GameObject NoBtn = msg.transform.Find ("NoButton").gameObject;
			YesBtn.GetComponent<DoCyusai> ().daimyoId = daimyoId;
			YesBtn.GetComponent<DoCyusai> ().daimyoId2 = daimyoId2;
			YesBtn.GetComponent<DoCyusai> ().daimyoName = daimyoName;
			YesBtn.GetComponent<DoCyusai> ().daimyoName2 = daimyoName2;
			YesBtn.GetComponent<DoCyusai> ().confirm = msg;
			YesBtn.GetComponent<DoCyusai> ().back = back;
			NoBtn.GetComponent<DoCyusai> ().confirm = msg;
			NoBtn.GetComponent<DoCyusai> ().back = back;
			NoBtn.GetComponent<DoCyusai> ().btnContent = btnContent;
			NoBtn.GetComponent<DoCyusai> ().uprContent = uprContent;


		}






	}
}
