using UnityEditor;
using UnityEngine;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class PlayerPrefsEditor {
	
	[MenuItem("Tools/PlayerPrefs/DeleteAll")]
	static void DeleteAll(){
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Flush();
		Debug.Log("Delete All Data Of PlayerPrefs!!");
	}
}