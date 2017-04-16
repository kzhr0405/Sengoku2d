using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeoutImage : MonoBehaviour {
	public float fadeTime = 3f;
	
	private float currentRemainTime;
	private Image image;
	
	// Use this for initialization
	void Start () {
		// 初期化
		currentRemainTime = fadeTime;
		image = GetComponent<Image>();
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
		var color = image.color;
		color.a = alpha;
		image.color = color;
	}
}