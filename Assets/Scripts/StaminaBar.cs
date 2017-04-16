using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StaminaBar : MonoBehaviour {
	private Vector2 afterScale;
	public  float framesNumber;
	public  float minReference;
	
	void Update () {
		updateStaminaBar();
	}
	
	void updateStaminaBar() {
		// 現在のスタミナバーの大きさを取得
		Vector2 currentScale = this.transform.localScale;
		bool scaleCondition  = (currentScale.x == this.afterScale.x);
		if (scaleCondition) {
			return;
		}
		currentScale.x += (this.afterScale.x - currentScale.x) / framesNumber;
		// 変化させる値の大きさ、絶対値
		float changeAmount = Mathf.Abs(this.afterScale.x - currentScale.x);
		// 変化させる量の起きさが小さい場合は、一気にゲージを増やす
		if (changeAmount < minReference) {
			currentScale.x = this.afterScale.x;
			this.transform.localScale = currentScale;
			return;
		}
		this.transform.localScale = currentScale;
	}
	
	// 現在のスタミナ数とマックスのスタミナ数をStaminaTextから取ってくる
	// SendMessageでは引数を1つしか渡せないため、配列として受け取る
	public void setStamina(ArrayList stamina) {
		float nowStaminaNum = (float)(int)stamina[0];
		float maxStaminaNum = (float)(int)stamina[1];
		this.afterScale.x = (float)(nowStaminaNum / maxStaminaNum);
	}
}	