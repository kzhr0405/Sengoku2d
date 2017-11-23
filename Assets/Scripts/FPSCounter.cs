using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour {

	public float interval = 1.0f;

	// for ui.
	private int screenLongSide;
	private Rect boxRect;
	private GUIStyle style = new GUIStyle();

	// for fps calculation.
	private int frameCount;
	private float elapsedTime;
	private double frameRate;
	private static GameObject instance = null;

	/// <summary>
	/// Initialization
	/// </summary>
	private void Awake()
	{
		if(instance != null){
			Destroy(gameObject);
		}else{
			instance = gameObject;
			DontDestroyOnLoad(gameObject);
			UpdateUISize();
		}
	}

	/// <summary>
	/// Monitor changes in resolution and calcurate FPS
	/// </summary>
	private void Update()
	{
		// FPS calculation
		frameCount++;
		elapsedTime += Time.deltaTime;
		if (elapsedTime > interval)
		{
			frameRate = System.Math.Round(frameCount / elapsedTime, 1, System.MidpointRounding.AwayFromZero);
			frameCount = 0;
			elapsedTime = 0;

			// Update the UI size if the resolution has changed
			if (screenLongSide != Mathf.Max(Screen.width, Screen.height))
			{
				UpdateUISize();
			}
		}
	}

	/// <summary>
	/// Resize the UI according to the screen resolution
	/// </summary>
	private void UpdateUISize()
	{
		screenLongSide = Mathf.Max(Screen.width, Screen.height);
		var rectLongSide = screenLongSide / 10;
		boxRect = new Rect(1, 1, rectLongSide, rectLongSide / 3);
		style.fontSize = (int)(screenLongSide / 36.8);
		style.normal.textColor = Color.white;
	}

	/// <summary>
	/// Display FPS
	/// </summary>
	private void OnGUI()
	{
		GUI.Box(boxRect, "");
		GUI.Label(boxRect, " " + frameRate + "fps");
	}
}
