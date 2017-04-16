using UnityEngine;
using System.Collections;

public class FadeoutArrowMove : MonoBehaviour {
	public float fadeTime = 1f;
	public float currentRemainTime;
	private SpriteRenderer spRenderer;

	// Use this for initialization
	void Start () {
		currentRemainTime = fadeTime;
		spRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		currentRemainTime -= Time.deltaTime;
		if ( currentRemainTime <= 0f ) {
			// 残り時間が無くなったら
			Destroy(gameObject);
		}

		// フェードアウト
		float alpha = currentRemainTime / fadeTime;
		var color = spRenderer.color;
		color.a = alpha;
		spRenderer.color = color;
	}
}
