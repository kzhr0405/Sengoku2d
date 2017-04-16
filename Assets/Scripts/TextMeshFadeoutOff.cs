using UnityEngine;
using System.Collections;

public class TextMeshFadeoutOff : MonoBehaviour {
	public float fadeTime = 1f;
	
	public float currentRemainTime;
	private TextMesh textMesh;
	
	// Use this for initialization
	void Start () {
		// 初期化
		currentRemainTime = fadeTime;
		textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		// 残り時間を更新
		currentRemainTime -= Time.deltaTime;
		
		if ( currentRemainTime <= 0f ) {
			// 残り時間が無くなったら
			GetComponent<TextMeshFadeoutOff>().currentRemainTime = fadeTime;
			GetComponent<MeshRenderer>().enabled = false;
			GetComponent<TextMeshFadeoutOff>().enabled = false;
			return;
		}
		
		// フェードアウト
		float alpha = currentRemainTime / fadeTime;
		var color = textMesh.color;
		color.a = alpha;
		textMesh.color = color;
	}
}