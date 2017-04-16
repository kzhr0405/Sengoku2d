using UnityEngine;
using System.Collections;

public class FpsMeasure : MonoBehaviour {
	int frameCount;
	float nextTime;
	
	// Use this for initialization
	void Start () {
		nextTime = Time.time + 1;
	}
	
	// Update is called once per frame
	void Update () {
		frameCount++;
		
		if ( Time.time >= nextTime ) {
			// 1秒経ったらFPSを表示
			Debug.Log("FPS : " + frameCount);
			frameCount = 0;
			nextTime += 1;
		}
	}
}