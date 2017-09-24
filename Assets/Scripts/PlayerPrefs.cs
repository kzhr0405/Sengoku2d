/*
	PreviewLabs.PlayerPrefs
	April 1, 2014 version

	Public Domain
	
	To the extent possible under law, PreviewLabs has waived all copyright and related or neighboring rights to this document. This work is published from: Belgium.
	
	http://www.previewlabs.com
	
*/
using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace PreviewLabs
{
	public static class PlayerPrefs
	{
		private static readonly Hashtable playerPrefsHashtable = new Hashtable ();
		private static bool hashTableChanged = false;
		private static string serializedOutput = "";
		private static string serializedInput = "";
		private const string PARAMETERS_SEPERATOR = ";";
		private const string KEY_VALUE_SEPERATOR = ":";
		private static string[] seperators = new string[]{PARAMETERS_SEPERATOR,KEY_VALUE_SEPERATOR};
		private static readonly string fileName = Application.persistentDataPath + "/PlayerPrefs.txt";
		private static readonly string secureFileName = Application.persistentDataPath + "/AdvancedPlayerPrefs.txt";
		private static readonly string tempFileName = Application.persistentDataPath + "/TempPlayerPrefs.txt";
		private static readonly string workingFlushFileName = Application.persistentDataPath + "/Writing.dat";//ファイル書き込み中のみ存在するファイル
		//NOTE modify the iw3q part to an arbitrary string of length 4 for your project, as this is the encryption key
		private static byte[] bytes = ASCIIEncoding.ASCII.GetBytes ("iw3q" + SystemInfo.deviceUniqueIdentifier.Substring (0, 4));
		private static bool wasEncrypted = false;
		private static bool securityModeEnabled = false;
		private static object guard = new object();//ロック用オブジェクト
		
		static PlayerPrefs ()
		{
            
            //Debug.Log("PlayerPrefs() start. @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
            //try{
            #if !UNITY_WEBPLAYER
            //load previous settings
            StreamReader fileReader = null;
			
			//バックグラウンド移行時に作成される一時ファイルのチェックと置換処理
			if (File.Exists (tempFileName) && isExistWorkingFlushFile()) {
				Debug.Log("Temp Flush File Exist");
				//一時ファイルがあるので、置換する
				replaceFlushFile();
				removeWorkingFlushFile();
			}
			
			if (File.Exists (secureFileName)) {
				fileReader = new StreamReader (secureFileName);
				wasEncrypted = true;
				serializedInput = Decrypt (fileReader.ReadToEnd ());
			} else if (File.Exists (fileName)) {
				fileReader = new StreamReader (fileName);
				serializedInput = fileReader.ReadToEnd ();
			}
			#else
			
			if(UnityEngine.PlayerPrefs.HasKey("encryptedData")) {
				securityModeEnabled = bool.Parse(UnityEngine.PlayerPrefs.GetString("encryptedData"));
				serializedInput = (securityModeEnabled?Decrypt(UnityEngine.PlayerPrefs.GetString("data")):UnityEngine.PlayerPrefs.GetString("data"));
			}
			
			#endif
			
			if (!string.IsNullOrEmpty (serializedInput)) {
				//In the old PlayerPrefs, a WriteLine was used to write to the file.
				if (serializedInput.Length > 0 && serializedInput [serializedInput.Length - 1] == '\n') {
					serializedInput = serializedInput.Substring (0, serializedInput.Length - 1);
					
					if (serializedInput.Length > 0 && serializedInput [serializedInput.Length - 1] == '\r') {
						serializedInput = serializedInput.Substring (0, serializedInput.Length - 1);
					}
				}
				
				Deserialize ();
			}
			
			#if !UNITY_WEBPLAYER
			if (fileReader != null) {
				fileReader.Close ();
			}
            #endif
            //}catch(Exception e){
            //	Debug.Log("PlayerPrefs : exception on PlayerPrefs(): " + e.Message );
            //}

        }

        public static bool HasKey (string key)
		{
			lock(guard) {
				return playerPrefsHashtable.ContainsKey (key);
			}
		}
		
		public static void SetString (string key, string value)
		{
			lock(guard) {
				if (!playerPrefsHashtable.ContainsKey (key)) {
					playerPrefsHashtable.Add (key, value);
				} else {
					playerPrefsHashtable [key] = value;
				}
				
				hashTableChanged = true;
			}
		}
		
		public static void SetInt (string key, int value)
		{
			lock(guard) {
				if (!playerPrefsHashtable.ContainsKey (key)) {
					playerPrefsHashtable.Add (key, value);
				} else {
					playerPrefsHashtable [key] = value;
				}
				
				hashTableChanged = true;
			}
		}
		
		public static void SetFloat (string key, float value)
		{
			lock(guard) {
				if (!playerPrefsHashtable.ContainsKey (key)) {
					playerPrefsHashtable.Add (key, value);
				} else {
					playerPrefsHashtable [key] = value;
				}
				
				hashTableChanged = true;
			}
		}
		
		public static void SetBool (string key, bool value)
		{
			lock(guard) {
				if (!playerPrefsHashtable.ContainsKey (key)) {
					playerPrefsHashtable.Add (key, value);
				} else {
					playerPrefsHashtable [key] = value;
				}
				
				hashTableChanged = true;
			}
		}
		
		public static void SetLong (string key, long value)
		{
			lock(guard) {
				if (!playerPrefsHashtable.ContainsKey (key)) {
					playerPrefsHashtable.Add (key, value);
				} else {
					playerPrefsHashtable [key] = value;
				}
				
				hashTableChanged = true;
			}
		}
		
		public static string GetString (string key)
		{			
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return playerPrefsHashtable [key].ToString ();
				}
				
				return null;
			}
		}
		
		public static string GetString (string key, string defaultValue)
		{
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return playerPrefsHashtable [key].ToString ();
				} else {
					playerPrefsHashtable.Add (key, defaultValue);
					hashTableChanged = true;
					return defaultValue;
				}
			}
		}
		
		public static int GetInt (string key)
		{			
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return (int)playerPrefsHashtable [key];
				}
				
				return 0;
			}
		}
		
		public static int GetInt (string key, int defaultValue)
		{
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return (int)playerPrefsHashtable [key];
				} else {
					playerPrefsHashtable.Add (key, defaultValue);
					hashTableChanged = true;
					return defaultValue;
				}
			}
		}
		
		public static long GetLong (string key)
		{			
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return (long)playerPrefsHashtable [key];
				}
				
				return 0;
			}
		}
		
		public static long GetLong (string key, long defaultValue)
		{
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return (long)playerPrefsHashtable [key];
				} else {
					playerPrefsHashtable.Add (key, defaultValue);
					hashTableChanged = true;
					return defaultValue;
				}
			}
		}
		
		public static float GetFloat (string key)
		{			
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return (float)playerPrefsHashtable [key];
				}
				
				return 0.0f;
			}
		}
		
		public static float GetFloat (string key, float defaultValue)
		{
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return (float)playerPrefsHashtable [key];
				} else {
					playerPrefsHashtable.Add (key, defaultValue);
					hashTableChanged = true;
					return defaultValue;
				}
			}
		}
		
		public static bool GetBool (string key)
		{			
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return (bool)playerPrefsHashtable [key];
				}
				
				return false;
			}
		}
		
		public static bool GetBool (string key, bool defaultValue)
		{
			lock(guard) {
				if (playerPrefsHashtable.ContainsKey (key)) {
					return (bool)playerPrefsHashtable [key];
				} else {
					playerPrefsHashtable.Add (key, defaultValue);
					hashTableChanged = true;
					return defaultValue;
				}
			}
		}
		
		public static void DeleteKey (string key)
		{
			lock(guard) {
				playerPrefsHashtable.Remove (key);
			}
		}
		
		public static void DeleteAll ()
		{
			lock(guard) {
				playerPrefsHashtable.Clear ();
			}
		}
		
		//This is important to check to avoid a weakness in your security when you are using encryption to avoid the users from editing your playerprefs.
		public static bool WasReadPlayerPrefsFileEncrypted ()
		{
			return wasEncrypted;
		}
		
		public static void EnableEncryption(bool enabled)
		{
			securityModeEnabled = enabled;
		}
		public static void Flush (bool isOnPause = false)
		{	
			//Debug.Log("PlayerPrefs : Flush start -----------------------------------" + isOnPause.ToString());
			lock(guard) {
				try{
                    //initAndroidPath();//test
                    if (hashTableChanged) {
						Serialize ();
						string output = (securityModeEnabled ? Encrypt (serializedOutput) : serializedOutput);
						#if !UNITY_WEBPLAYER
						StreamWriter fileWriter = null;

						//ファイルの書き出しは常に一時ファイルへ行う
						initFlush();
						fileWriter = File.CreateText (tempFileName);

						if (fileWriter == null) { 
							Debug.LogWarning ("PlayerPrefs : Flush() opening file for writing failed: " + tempFileName);
							return;
						}

						fileWriter.Write (output);

						fileWriter.Close ();

						if (!isOnPause) {
							//通常の保存時はここでフラッシュデータの置換を行う
							replaceFlushFile();
						} else {
							//バックグラウンド移行時は、
							//一時ファイルの生成が正常に完了したことを示すフラグファイルを作成する
							createWorkingFlushFile();
						}

						//異なるモードのファイルは削除する（既存処理）
						File.Delete((securityModeEnabled ? fileName : secureFileName));

						#else
						UnityEngine.PlayerPrefs.SetString("data", output);
						UnityEngine.PlayerPrefs.SetString("encryptedData", securityModeEnabled.ToString());

						UnityEngine.PlayerPrefs.Save();
						#endif

						serializedOutput = "";

						//Debug.Log("PlayerPrefs : Flush end *******************************************");
					}
				}catch(Exception e){
					Debug.Log("PlayerPrefs : exception on flushing: " + e.Message );
				}
			}
		}
		
		private static void Serialize ()
		{
			IDictionaryEnumerator myEnumerator = playerPrefsHashtable.GetEnumerator ();
			System.Text.StringBuilder sb = new System.Text.StringBuilder ();
			bool firstString = true;
			while (myEnumerator.MoveNext()) {
				//if(serializedOutput != "")
				if (!firstString) {
					sb.Append (" ");
					sb.Append (PARAMETERS_SEPERATOR);
					sb.Append (" ");
				}
				sb.Append (EscapeNonSeperators (myEnumerator.Key.ToString (), seperators));
				sb.Append (" ");
				sb.Append (KEY_VALUE_SEPERATOR);
				sb.Append (" ");
				sb.Append (EscapeNonSeperators (myEnumerator.Value.ToString (), seperators));
				sb.Append (" ");
				sb.Append (KEY_VALUE_SEPERATOR);
				sb.Append (" ");
				sb.Append (myEnumerator.Value.GetType ());
				firstString = false;
			}
			serializedOutput = sb.ToString ();
		}
		
		private static void Deserialize ()
		{
			string[] parameters = serializedInput.Split (new string[] {" " + PARAMETERS_SEPERATOR + " "}, StringSplitOptions.RemoveEmptyEntries);
			
			foreach (string parameter in parameters) {
				string[] parameterContent = parameter.Split (new string[]{" " + KEY_VALUE_SEPERATOR + " "}, StringSplitOptions.None);
				
				playerPrefsHashtable.Add (DeEscapeNonSeperators (parameterContent [0], seperators), GetTypeValue (parameterContent [2], DeEscapeNonSeperators (parameterContent [1], seperators)));
				
				if (parameterContent.Length > 3) {
					Debug.LogWarning ("PlayerPrefs::Deserialize() parameterContent has " + parameterContent.Length + " elements");
				}
			}
		}
		
		public static string EscapeNonSeperators(string inputToEscape, string[] seperators)
		{
			inputToEscape = inputToEscape.Replace("\\", "\\\\");
			
			for (int i = 0; i < seperators.Length; ++i)
			{
				inputToEscape = inputToEscape.Replace(seperators[i], "\\" + seperators[i]);
			}
			
			return inputToEscape;
		}
		
		public static string DeEscapeNonSeperators(string inputToDeEscape, string[] seperators)
		{
			
			for (int i = 0; i < seperators.Length; ++i)
			{
				inputToDeEscape = inputToDeEscape.Replace("\\" + seperators[i], seperators[i]);
			}
			
			inputToDeEscape = inputToDeEscape.Replace("\\\\", "\\");
			
			return inputToDeEscape;
		}
		
		private static string Encrypt (string originalString)
		{
			if (String.IsNullOrEmpty (originalString)) {
				return "";
			}
			
			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider ();
			MemoryStream memoryStream = new MemoryStream ();
			CryptoStream cryptoStream = new CryptoStream (memoryStream, cryptoProvider.CreateEncryptor (bytes, bytes), CryptoStreamMode.Write);
			StreamWriter writer = new StreamWriter (cryptoStream);
			writer.Write (originalString);
			writer.Flush ();
			cryptoStream.FlushFinalBlock ();
			writer.Flush ();
			return Convert.ToBase64String (memoryStream.GetBuffer (), 0, (int)memoryStream.Length);
		}
		
		private static string Decrypt (string cryptedString)
		{
			if (String.IsNullOrEmpty (cryptedString)) {
				return "";
			}
			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider ();
			MemoryStream memoryStream = new MemoryStream (Convert.FromBase64String (cryptedString));
			CryptoStream cryptoStream = new CryptoStream (memoryStream, cryptoProvider.CreateDecryptor (bytes, bytes), CryptoStreamMode.Read);
			StreamReader reader = new StreamReader (cryptoStream);
			return reader.ReadToEnd ();
		}
		
		private static object GetTypeValue (string typeName, string value)
		{
			if (typeName == "System.String") {
				return (object)value.ToString ();
			}
			if (typeName == "System.Int32") {
				return Convert.ToInt32 (value);
			}
			if (typeName == "System.Boolean") {
				return Convert.ToBoolean (value);
			}
			if (typeName == "System.Single") { //float
				return Convert.ToSingle (value);
			}
			if (typeName == "System.Int64") { //long
				return Convert.ToInt64 (value);
			} else {
				Debug.LogError ("Unsupported type: " + typeName);
			}	
			
			return null;
		}
		
		//書き込み中を示すファイルを作成する
		private static void createWorkingFlushFile()
		{
			using (FileStream hStream = File.Create(workingFlushFileName)) {
				// 作成時に返される FileStream を利用して閉じる
				if (hStream != null) {
					hStream.Close();
				}
			}
		}

		private static void removeWorkingFlushFile()
		{
			File.Delete(workingFlushFileName);
		}

		private static bool isExistWorkingFlushFile()
		{
			return File.Exists(workingFlushFileName);
		}

		//Flush前に不要ファイルを削除する
		private static void initFlush()
		{
			File.Delete(workingFlushFileName);
			File.Delete(tempFileName);
		}

		//一時ファイルをデータファイルとして置換する
		private static void replaceFlushFile()
		{
			string writeFileName = securityModeEnabled ? secureFileName : fileName;

			//バックアップファイルがあれば消す
			string backupFileName = writeFileName + "_bkup";
			if (File.Exists(backupFileName)) {
				File.Delete(backupFileName);
			}

			//元データファイルをバックアップファイルとしてリネーム
			if (File.Exists(writeFileName)) {
				File.Move(writeFileName, backupFileName);
			}

			//一時データファイルを元データファイルとしてリネーム
			File.Move(tempFileName, writeFileName);//Flush完了

			//バックアップファイルがあれば消す
			if (File.Exists(backupFileName)) {
				File.Delete(backupFileName);
			}

			//保存完了
			//Debug.Log("Flush Data Replace Completed !");
		}

        private static string _FilesDir;
        private static string _CacheDir;
        private static string _ExternalFilesDir;
        private static string _ExternalCacheDir;
        private static void initAndroidPath() {
        #if !UNITY_EDITOR && UNITY_ANDROID
			        using( AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer") )
			        {
				        using( AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity") )
				        {
					        using( AndroidJavaObject filesDir = currentActivity.Call<AndroidJavaObject>( "getFilesDir" ) )
					        {
						        _FilesDir = filesDir.Call<string>( "getCanonicalPath" );
					        }

					        using( AndroidJavaObject cacheDir = currentActivity.Call<AndroidJavaObject>( "getCacheDir" ) )
					        {
						        _CacheDir = cacheDir.Call<string>( "getCanonicalPath" );
					        }

					        using( AndroidJavaObject externalFilesDir = currentActivity.Call<AndroidJavaObject>("getExternalFilesDir",null ) )
					        {
						        _ExternalFilesDir = externalFilesDir.Call<string>("getCanonicalPath");
					        }

					        using( AndroidJavaObject externalCacheDir = currentActivity.Call<AndroidJavaObject>("getExternalCacheDir" ) )
					        {
						        _ExternalCacheDir = externalCacheDir.Call<string>("getCanonicalPath");
					        }
				        }
			        }
			        Debug.Log( "PlayerPrefs : getFilesDir : " + _FilesDir );
			        Debug.Log( "PlayerPrefs : getCacheDir : " + _CacheDir );
			        Debug.Log( "PlayerPrefs : getExternalFilesDir : " + _ExternalFilesDir );
			        Debug.Log( "PlayerPrefs : getExternalCacheDir : " + _ExternalCacheDir );

        #endif
        }

    }    
}

