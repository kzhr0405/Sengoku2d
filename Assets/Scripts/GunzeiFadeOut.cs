using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunzeiFadeOut : MonoBehaviour {
	private Image gunzei;
	private Image back;
	private Text text;

	private float time;
	public float fadetime = 0.3f;

	void Start () {
		time = fadetime;
		gunzei = GetComponent<Image>();
		back = gunzei.transform.FindChild ("MsgBack").GetComponent<Image>();
		text = back.transform.FindChild ("MsgText").GetComponent<Text>();
	}
	
	void Update () {
		time -= Time.deltaTime;//時間更新
		float a = time / fadetime;
		var color = gunzei.color;
		var backColor = back.color;
		var textColor = text.color;
		color.a = a;
		textColor.a = a;
		gunzei.color = color;
		back.color = backColor;
		text.color = textColor;
		if(time <= 0f ) {
			// 残り時間が無くなったら自分自身を消滅
			GameObject.Destroy(gameObject);
			return;
		}
		
	}
}