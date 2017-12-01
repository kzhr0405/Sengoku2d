using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FadeOutSlotMessage : MonoBehaviour {

	public GameObject content;
	public List<GameObject> contentList;
	public float fadeTime = 4f;
	private float currentRemainTime;

	void Start () {
		content = transform.Find ("ScrollView").transform.Find ("Content").gameObject;
		currentRemainTime = fadeTime;
	}

	void Update () {
		currentRemainTime -= Time.deltaTime;

		if ( currentRemainTime <= 0f ) {
			// 残り時間が無くなったら自分自身を消滅
			GameObject.Destroy(gameObject);
			return;
		}

		// フェードアウト
		float alpha = currentRemainTime / fadeTime;
		foreach(GameObject contentObj in contentList){
			var color = contentObj.GetComponent<Image> ().color;
			color.a = alpha;
			contentObj.GetComponent<Image> ().color = color;
		}


	}
}
