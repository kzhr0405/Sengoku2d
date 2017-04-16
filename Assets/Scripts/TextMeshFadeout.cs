using UnityEngine;
using System.Collections;

public class TextMeshFadeout : MonoBehaviour {
	public float fadeTime = 1f;
	
	private float currentRemainTime;
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
			// 残り時間が無くなったら自分自身を消滅
			GameObject.Destroy(gameObject);
			return;
		}
		
		// フェードアウト
		float alpha = currentRemainTime / fadeTime;
		var color = textMesh.color;
		color.a = alpha;
		textMesh.color = color;
	}
}