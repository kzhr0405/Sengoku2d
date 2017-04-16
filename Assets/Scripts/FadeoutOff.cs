using UnityEngine;
using System.Collections;

public class FadeoutOff : MonoBehaviour {
	public float fadeTime = 1f;
	public float currentRemainTime;
	private SpriteRenderer spRenderer;
	
	// Use this for initialization
	void Start () {
		// 初期化
		currentRemainTime = fadeTime;
		spRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		// 残り時間を更新
		currentRemainTime -= Time.deltaTime;
		if ( currentRemainTime <= 0f ) {
			// 残り時間が無くなったら

			//Init
			spRenderer.color = new Color (255f / 255f, 255f / 255f, 255f / 255f, 150f / 255f); //Blue

			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<Animator>().enabled = false;
			GetComponent<FadeoutOff>().enabled = false;

			return;
		}
		
		// フェードアウト
		float alpha = currentRemainTime / fadeTime;
		var color = spRenderer.color;
		color.a = alpha;
		spRenderer.color = color;
	}
}