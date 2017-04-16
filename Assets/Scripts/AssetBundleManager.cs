using UnityEngine;
using System;
using System.Collections;

public class AssetBundleManager : MonoBehaviour {
    // AssetBundleのキャッシュ
    public AssetBundle assetBundleCache;


    // Asset Bundleをロードするコルーチンです
    public IEnumerator LoadAssetBundleCoroutine() {
        // Asset BundleのURL
        var url = "";
        #if UNITY_ANDROID
                url = "http://Android用配置したURL";
        #else
                url = "https://www.dropbox.com/s/6qa66syngfifo3l/sengoku2d?dl=1";
        #endif


        // ダウンロード処理
        using (WWW www = WWW.LoadFromCacheOrDownload(url, 0)) {
            while (!www.isDone) {
                yield return null;
            }
            if (www.error != null) {
                throw new Exception("Error while WWW was downloading:" + www.error);
            }

            // Asset Bundleをキャッシュ
            assetBundleCache = www.assetBundle;

            // リクエストは開放
            www.Dispose();

        }

    }

    // Asset BundleからSpriteを取得します
    public Sprite GetSpriteFromAssetBundle(string assetName) {
        
        try {
            return assetBundleCache.LoadAsset<Sprite>(assetName);
        }
        catch (NullReferenceException e) {
            Debug.Log(e.ToString());
            return null;
        }
    }
}