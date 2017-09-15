using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

//広告枠IDの設定
[System.Serializable]
public class AdfurikunMovieNativeAdViewAdConfig
{
	public string iPhoneAppID;
	//iOS
	public string androidAppID;
	//android
}

public class AdfurikunMovieNativeAdViewUtility : MonoBehaviour
{

	public enum ADF_MovieStatus
	{
		/// <summary>
		/// ネイティブ広告の読み込み成功
		/// </summary>
		LoadFinish,
		/// <summary>
		/// ネイティブ広告の読み込み失敗
		/// </summary>
		LoadError
	}

	//広告枠IDの設定
	public AdfurikunMovieNativeAdViewAdConfig config;

	private static AdfurikunMovieNativeAdViewUtility mInstance = null;
	private GameObject mMovieNativeAdViewSrcObject = null;
	//TODO:

	#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void initializeMovieNativeAdViewIOS_(string appID);

	[DllImport("__Internal")]
	private static extern void loadMovieNativeAdViewIOS_(string appID);

	[DllImport("__Internal")]
	private static extern void showMovieNativeAdViewIOS_(string appID, float x, float y, float width, float height, float screenW);

	[DllImport("__Internal")]
	private static extern void setMovieNativeAdViewFrameIOS_(string appID, float x, float y, float width, float height, float screenW);

	[DllImport("__Internal")]
	private static extern void playMovieNativeAdViewIOS_(string appID);

	[DllImport("__Internal")]
	private static extern void pauseMovieNativeAdViewIOS_(string appID);

	[DllImport("__Internal")]
	private static extern void hideMovieNativeAdViewIOS_(string appID);

	[DllImport("__Internal")]
	private static extern void stopMovieNativeAdViewAutoReloadIOS_(string appID);

	[DllImport("__Internal")]
	private static extern void startMovieNativeAdViewAutoReloadIOS_(string appID);

	

#elif UNITY_ANDROID
	private float androidDensity = 1.0f;
	#endif

	public void Awake ()
	{
		if (mInstance == null) {
			mInstance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
	}


	// Use this for initialization
	void Start ()
	{
		if (Application.isEditor)
			return;

		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass player = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = player.GetStatic<AndroidJavaObject> ("currentActivity");

			AndroidJavaObject wm = activity.Call<AndroidJavaObject> ("getWindowManager");
			if (wm != null) {
				AndroidJavaObject display = wm.Call<AndroidJavaObject> ("getDefaultDisplay");
				if (display != null) {
					AndroidJavaObject displayMetrics = new AndroidJavaObject ("android.util.DisplayMetrics");
					display.Call ("getMetrics", displayMetrics);
					androidDensity = displayMetrics.Get<float> ("density");
				}
			}
		}
		#endif

		this.initializeMovieNativeAdView ();
	}

	public void OnApplicationPause (bool pause)
	{
		if (Application.isEditor)
			return;

		#if UNITY_IPHONE
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			if (pause) {
				this.callAndroidMovieNativeAdViewMethod ("onPause");
			} else {
				this.callAndroidMovieNativeAdViewMethod ("onResume");
			}
		}
		#endif
	}

	public void initializeMovieNativeAdView ()
	{
		this.initializeMovieNativeAdView (this.getAppID ());
	}

	public void initializeMovieNativeAdView (string appId)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			initializeMovieNativeAdViewIOS_(appId);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass player = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = player.GetStatic<AndroidJavaObject> ("currentActivity");
			this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic ("initialize", activity, appId);
		}
		#endif
	}

	public void loadMovieNativeAdView ()
	{
		this.loadMovieNativeAdView (this.getAppID ());
	}

	public void loadMovieNativeAdView (string appId)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			loadMovieNativeAdViewIOS_(appId);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic ("load", appId);

		}
		#endif
	}

	public void setMovieNativeAdView (float x, float y, float width, float height)
	{
		this.setMovieNativeAdView (this.getAppID (), x, y, width, height);
	}

	public void setMovieNativeAdView (string appId, float x, float y, float width, float height)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			showMovieNativeAdViewIOS_(appId, x, y, width, height, Screen.width);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			AndroidJavaClass player = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = player.GetStatic<AndroidJavaObject> ("currentActivity");
			AndroidJavaObject window = activity.Call<AndroidJavaObject> ("getWindow");
			AndroidJavaObject decorView = window.Call<AndroidJavaObject> ("getDecorView");
			int decorViewW  = decorView.Call<int> ("getWidth");
			int decorViewH  = decorView.Call<int> ("getHeight");

			float densityW = decorViewW * (width/Screen.width);
			float densityH = decorViewH * (height/Screen.height);
			float pointX   = decorViewW * (x/Screen.width);
			float pointY   = decorViewH * (y/Screen.height);

			this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic ("show", appId, pointX, pointY, densityW, densityH);
		}
		#endif
	}

	public void setMovieNativeAdViewFrame (float x, float y, float width, float height)
	{
		this.setMovieNativeAdViewFrame (this.getAppID (), x, y, width, height);
	}

	public void setMovieNativeAdViewFrame (string appId, float x, float y, float width, float height)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			setMovieNativeAdViewFrameIOS_(appId, x, y, width, height, Screen.width);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			this.setMovieNativeAdView(appId, x, y, width, height);
		}
		#endif
	}

	public void playMovieNativeAdView ()
	{
		this.playMovieNativeAdViewNative (this.getAppID ());
	}

	public void playMovieNativeAdViewNative (string appId)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			playMovieNativeAdViewIOS_(appId);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic ("play", appId);
		}
		#endif
	}

	public void pauseMovieNativeAdView ()
	{
		this.pauseMovieNativeAdView (this.getAppID ());
	}

	public void pauseMovieNativeAdView (string appId)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			pauseMovieNativeAdViewIOS_(appId);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic ("pause", appId);
		}
		#endif
	}

	public void hideMovieNativeAdView ()
	{
		this.hideMovieNativeAdView (this.getAppID ());
	}

	public void hideMovieNativeAdView (string appId)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			hideMovieNativeAdViewIOS_(appId);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic ("hide", appId);
		}
		#endif
	}

	public void stopAutoReload ()
	{
		this.stopAutoReload (this.getAppID ());
	}

	public void stopAutoReload (string appId)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			stopMovieNativeAdViewAutoReloadIOS_(appId);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic ("stopAutoReload", appId);
		}
		#endif
	}

	public void startAutoReload ()
	{
		this.startAutoReload (this.getAppID ());
	}

	public void startAutoReload (string appId)
	{
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			startMovieNativeAdViewAutoReloadIOS_(appId);
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic ("startAutoReload", appId);
		}
		#endif
	}

	public void dispose ()
	{
		this.disposeResource ();
	}

	public void disposeResource ()
	{
		#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			this.callAndroidMovieNativeAdViewMethod ("onDestroy");
		}
		#endif
	}

	public void setMovieNativeAdViewSrcObject (GameObject movieNativeAdViewSrcObject)
	{
		this.setMovieNativeAdViewSrcObject (movieNativeAdViewSrcObject, this.getAppID ());
	}

	public void setMovieNativeAdViewSrcObject (GameObject movieNativeAdViewSrcObject, string appId)
	{
		this.mMovieNativeAdViewSrcObject = movieNativeAdViewSrcObject;
	}

	// Update is called once per frame
	void Update ()
	{
		
	}

	private string getAppID ()
	{
		string appId = "";
		#if UNITY_IPHONE
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
		    appId = config.iPhoneAppID;
		}
		#elif UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android) {
			appId = config.androidAppID;
		}
		#endif
		return appId;
	}


	/**
	 * コールバック受け取りメソッド
	 */
	public void MovieNativeAdViewCallback (string param_str)
	{

		string[] splitParamRoot = param_str.Split (';');
		string stateName = splitParamRoot [0].Split (':') [1];
		string appID = splitParamRoot [1].Split (':') [1];
		string errcode = "";
		if (splitParamRoot.Length > 2) {
			errcode = splitParamRoot [2].Split (':') [1];
		}

		ADF_MovieStatus messageStateName;
		//状態に応じて分岐
		switch (stateName) {
		case "LoadFinish":
			messageStateName = ADF_MovieStatus.LoadFinish;
			break;
		case "LoadError":
			messageStateName = ADF_MovieStatus.LoadError;
			break;
		default:
			return;
		}
		ArrayList arr = new ArrayList ();
		arr.Add ((int)messageStateName);
		arr.Add (appID);
		arr.Add (errcode);
		if (this.mMovieNativeAdViewSrcObject != null) {
			this.mMovieNativeAdViewSrcObject.SendMessage ("MovieNativeAdViewCallback", arr);
		}
	}

	#if UNITY_ANDROID
	private AndroidJavaClass makeInstance_AdfurikunMovieNativeAdViewController ()
	{
		return new AndroidJavaClass ("jp.tjkapp.adfurikunsdk.moviereward.AdfurikunMovieNativeAdViewController");
	}

	private void callAndroidMovieNativeAdViewMethod (string methodName)
	{
		this.makeInstance_AdfurikunMovieNativeAdViewController ().CallStatic (methodName);
	}
	#endif
}
