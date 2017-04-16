using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StaminaController : MonoBehaviour {
	public GameObject staminaBar;            // スタミナバーのゲームオブジェクト
	public  int recoveryTimePerStamina = 10; // 1スタミナ回復するのに必要な時間[秒]
	public  int maxStaminaNum;               // スタミナの上限数
	public  int nowStaminaNum;               // 現在のスタミナ数
	private int restStaminaTime;             // スタミナが1回復するまでの残り時間
	public int spendStaminaNum;	
	
	void Start () {
		this.restStaminaTime = recoveryTimePerStamina;
		this.setLastAccess(System.DateTime.Now);		
		this.updateStamina();		
	}
	
	private void updateStamina() {		
		// スタミナ回復残り時間を分と秒で表示するための変数
		int minutes = this.restStaminaTime / 60;
		int seconds = this.restStaminaTime % 60;
		// 表示するテキスト
		string staminaText = "スタミナ　あと" + String.Format("{0:D2}",minutes) + ":" + String.Format("{0:D2}",seconds) + "   ";
		staminaText       += nowStaminaNum + "/" + this.maxStaminaNum;
		// スタミナがマックスの場合
		if(nowStaminaNum == this.maxStaminaNum) {			
			this.staminaMax();
			return;
		}
		// 最後の00:00の部分は見せないため、00:01秒になったら時間をリセットする
		if (this.restStaminaTime == 1) {
			this.resetRestStaminaTime(staminaText);
			return;
		}
		this.showStaminaText(staminaText);
		this.restStaminaTime -= 1;
		Invoke("updateStamina",1);
	}
	
	private void resetRestStaminaTime(string staminaText) {
		this.restStaminaTime = recoveryTimePerStamina;			
		this.showStaminaText(staminaText);
		nowStaminaNum += 1;		
		Invoke("updateStamina",1);
	}
	
	// スタミナがマックスの状態の時
	private void staminaMax() {
		// 時間は表示しない
		string staminaText = "スタミナ" +nowStaminaNum + "/" + this.maxStaminaNum;
		this.showStaminaText(staminaText);
		Invoke("updateStamina",1);
	}
	
	private void showStaminaText(string text) {
		this.GetComponent<TextMesh>().text = text;
		this.updateStaminaBar();
	}
	
	// 冒険に出るときのスタミナ消費するメソッド
	// スタミナが足りない場合はfalseを返す
	public bool spendStamina(int spendStaminaNum) {
		bool isSuccess = false;
		if (nowStaminaNum < spendStaminaNum) {
			Debug.Log("スタミナが足りません");
			return isSuccess;
		}
		nowStaminaNum -= spendStaminaNum;		
		isSuccess      = true;
		return isSuccess;
	}
	
	// ユーザが再びゲームを開始した時にアクセス時間の差を基にスタミナを回復させる
	public void setLastAccess(System.DateTime lastAccess) {	 	
		System.DateTime testLastAccess = new DateTime(2014, 9, 5, 12, 20, 0, DateTimeKind.Local);
		System.DateTime now    = System.DateTime.Now;
		// 現在の時刻とラストアクセス時間を元にその差分の時間を取得する
		System.TimeSpan diff   = now.Subtract(testLastAccess);
		// 2つの差分時間を整数で表す
		int diffTime           = (int)diff.TotalSeconds;
		// 差分時間によって回復できるスタミナ数と、その余りの時間
		int recoveryStaminaNum = diffTime / recoveryTimePerStamina;
		int rest               = diffTime % recoveryTimePerStamina;
		
		nowStaminaNum += recoveryStaminaNum;
		// スタミナ上限値よりも超えた場合はマックスにする。余りの時間も0にする
		if (this.maxStaminaNum < nowStaminaNum) {
			nowStaminaNum = this.maxStaminaNum;
			rest = 0;
		}
		// スタミナが1回復するまでの残り時間を余りの時間で引く
		this.restStaminaTime -= rest;
	}
	
	// ランクアップや魔法石を使ってスタミナを全回復させるときのメソッド
	public void recoverMax() {
		nowStaminaNum   = this.maxStaminaNum;
		this.restStaminaTime = this.recoveryTimePerStamina;
	}
	
	// スタミナバーへ現在のスタミナ数とマックスのスタミナ数を渡す
	private void updateStaminaBar() {
		ArrayList stamina = new ArrayList();
		stamina.Add(nowStaminaNum);
		stamina.Add(maxStaminaNum);
		staminaBar.SendMessage("setStamina",stamina);
	}
	
	// テストコード
	void OnGUI() {
		if(GUI.Button(new Rect(100,100,200,100),"スタミナ" + spendStaminaNum)) {
			this.spendStamina(spendStaminaNum);
		}
		if(GUI.Button(new Rect(500,100,200,100),"魔法石スタミナ回復")) {
			this.recoverMax();
		}
	}
}